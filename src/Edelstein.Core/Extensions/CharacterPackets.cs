using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Types;
using Edelstein.Database;
using Edelstein.Database.Inventories;
using Edelstein.Network.Packets;

namespace Edelstein.Core.Extensions
{
    public static class CharacterPackets
    {
        public static void EncodeData(this Character c, IPacket p, DbChar flags = DbChar.All)
        {
            p.Encode<long>((long) flags);
            p.Encode<byte>(0);
            p.Encode<byte>(0);

            if (flags.HasFlag(DbChar.Character))
            {
                EncodeStats(c, p);
                p.Encode<byte>(250); // nFriendMax
                p.Encode<bool>(false);
            }

            if (flags.HasFlag(DbChar.Money)) p.Encode<int>(c.Money);

            if (flags.HasFlag(DbChar.InventorySize))
            {
                if (flags.HasFlag(DbChar.ItemSlotEquip))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Equip].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotConsume))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Consume].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotInstall))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Install].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotEtc))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Etc].SlotMax);
                if (flags.HasFlag(DbChar.ItemSlotCash))
                    p.Encode<byte>((byte) c.Inventories[ItemInventoryType.Cash].SlotMax);
            }

            if (flags.HasFlag(DbChar.AdminShopCount))
            {
                p.Encode<int>(0);
                p.Encode<int>(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotEquip))
            {
                p.Encode<short>(0);
                p.Encode<short>(0);
                p.Encode<short>(0);
                p.Encode<short>(0);
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotConsume))
            {
                p.Encode<byte>(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotInstall))
            {
                p.Encode<byte>(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotEtc))
            {
                p.Encode<byte>(0);
            }

            if (flags.HasFlag(DbChar.ItemSlotCash))
            {
                p.Encode<byte>(0);
            }

            if (flags.HasFlag(DbChar.SkillRecord))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.SkillCooltime))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.QuestRecord))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.QuestComplete))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.MinigameRecord))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.CoupleRecord))
            {
                p.Encode<short>(0); // Couple
                p.Encode<short>(0); // Friend
                p.Encode<short>(0); // Marriage
            }

            if (flags.HasFlag(DbChar.MapTransfer))
            {
                for (var i = 0; i < 5; i++) p.Encode<int>(0);
                for (var i = 0; i < 10; i++) p.Encode<int>(0);
            }

            if (flags.HasFlag(DbChar.NewYearCard))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.QuestRecordEx))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.WildHunterInfo))
            {
                if (c.Job / 100 == 33)
                {
                    p.Encode<byte>(0);
                    for (var i = 0; i < 5; i++) p.Encode<int>(0);
                }
            }

            if (flags.HasFlag(DbChar.QuestCompleteOld))
            {
                p.Encode<short>(0);
            }

            if (flags.HasFlag(DbChar.VisitorLog))
            {
                p.Encode<short>(0);
            }
        }

        public static void EncodeStats(this Character c, IPacket p)
        {
            p.Encode<int>(c.ID);
            p.EncodeFixedString(c.Name, 13);

            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);
            p.Encode<int>(c.Hair);

            for (var i = 0; i < 3; i++)
                p.Encode<long>(0); // Pet stuff.

            p.Encode<byte>(c.Level);
            p.Encode(c.Job);
            p.Encode<short>(c.STR);
            p.Encode<short>(c.DEX);
            p.Encode<short>(c.INT);
            p.Encode<short>(c.LUK);
            p.Encode<int>(c.HP);
            p.Encode<int>(c.MaxHP);
            p.Encode<int>(c.MP);
            p.Encode<int>(c.MaxMP);

            p.Encode<short>(c.AP);
            if (SkillConstants.IsExtendSPJob(c.Job))
                p.Encode<short>(c.SP);
            else p.Encode<byte>(0); // TODO: extendedSP

            p.Encode<int>(c.EXP);
            p.Encode<short>(c.POP);

            p.Encode<int>(c.TempEXP);
            p.Encode<int>(c.FieldID);
            p.Encode<byte>(c.FieldPortal);
            p.Encode<int>(c.PlayTime);
            p.Encode<short>(c.SubJob);
        }

        public static void EncodeLook(this Character c, IPacket p)
        {
            p.Encode<byte>(c.Gender);
            p.Encode<byte>(c.Skin);
            p.Encode<int>(c.Face);

            p.Encode<bool>(false);
            p.Encode<int>(c.Hair);

            p.Encode<byte>(0xFF);
            p.Encode<byte>(0xFF);

            p.Encode<int>(0);

            for (var i = 0; i < 3; i++)
                p.Encode<int>(0);
        }
    }
}