using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Handlers.Users
{
    public class PartyRequestHandler : AbstractFieldUserHandler
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        protected override async Task Handle(
            FieldUser user,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var type = (PartyRequestType) packet.Decode<byte>();

            switch (type)
            {
                case PartyRequestType.LoadParty:
                {
                    using var p = new Packet(SendPacketOperations.PartyResult);

                    p.Encode<byte>((byte) PartyResultType.LoadParty_Done);
                    p.Encode<int>(user.Party.ID);
                    user.Party.EncodeData(user.Service.State.ChannelID, p);

                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.CreateNewParty:
                {
                    if (user.Party != null) return;

                    var result = PartyResultType.CreateNewParty_Done;

                    try
                    {
                        // TODO: beginner check

                        if (result == PartyResultType.CreateNewParty_Done)
                        {
                            await user.Service.PartyManager.Create(user.Character);
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.CreateNewParty_Unknown;
                    }

                    using var p = new Packet(SendPacketOperations.PartyResult);
                    p.Encode<byte>((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.WithdrawParty:
                {
                    if (user.Party == null) return;

                    try
                    {
                        if (user.Party.BossCharacterID == user.ID)
                            await user.Party.Disband();
                        else
                            await user.Party.Members
                                .First(m => m.CharacterID == user.ID)
                                .Withdraw();
                    }
                    catch
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);
                        p.Encode<byte>((byte) PartyResultType.WithdrawParty_Unknown);
                        await user.SendPacket(p);
                    }

                    break;
                }
                case PartyRequestType.KickParty:
                {
                    if (user.Party == null || user.Party.BossCharacterID != user.ID) return;

                    var result = PartyResultType.KickParty_Done;
                    var target = packet.Decode<int>();

                    try
                    {
                        var member = user.Party.Members.First(m => m.CharacterID == target);

                        // TODO: fieldlimit

                        if (result == PartyResultType.JoinParty_Done)
                        {
                            await member.Kick();
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.KickParty_Unknown;
                    }

                    using var p = new Packet(SendPacketOperations.PartyResult);
                    p.Encode<byte>((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                case PartyRequestType.ChangePartyBoss:
                {
                    if (user.Party == null || user.Party.BossCharacterID != user.ID) return;

                    var result = PartyResultType.ChangePartyBoss_Done;
                    var target = packet.Decode<int>();

                    try
                    {
                        var member = user.Party.Members
                            .Where(m => m.ChannelID >= 0)
                            .First(m => m.CharacterID == target);

                        if (member.ChannelID != user.Service.State.ChannelID)
                            result = PartyResultType.ChangePartyBoss_NotSameChannel;
                        if (member.FieldID != user.Field.Template.ID)
                            result = PartyResultType.ChangePartyBoss_NotSameField;

                        if (result == PartyResultType.ChangePartyBoss_Done)
                        {
                            await member.ChangeBoss();
                            return;
                        }
                    }
                    catch
                    {
                        result = PartyResultType.ChangePartyBoss_Unknown;
                    }

                    using var p = new Packet(SendPacketOperations.PartyResult);
                    p.Encode<byte>((byte) result);
                    await user.SendPacket(p);
                    break;
                }
                default:
                    Logger.Warn($"Unhandled party request type: {type}");
                    break;
            }
        }
    }
}