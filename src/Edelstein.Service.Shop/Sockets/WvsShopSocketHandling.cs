using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Extensions;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Server.Best;
using Edelstein.Provider.Templates.Server.CategoryDiscount;
using Edelstein.Provider.Templates.Server.ModifiedCommodity;
using Edelstein.Provider.Templates.Server.NotSale;
using Edelstein.Service.Shop.Logging;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Edelstein.Service.Shop.Sockets
{
    public partial class WvsShopSocket
    {
        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.MigrateIn:
                    return OnMigrateIn(packet);
                case RecvPacketOperations.UserTransferFieldRequest:
                    return OnTransferFieldRequest(packet);
                case RecvPacketOperations.CashShopCashItemRequest:
                    return OnCashShopCashItemRequest(packet);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }

        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            using (var db = WvsShop.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Locker)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Trunk)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Single(c => c.ID == characterID);

                if (!await WvsShop.TryMigrateFrom(character, WvsShop.Info))
                    await Disconnect();
                Character = character;

                using (var p = new Packet(SendPacketOperations.SetCashShop))
                {
                    character.EncodeData(p);

                    p.Encode<bool>(true); // m_bCashShopAuthorized
                    p.Encode<string>(character.Data.Account.Username); // m_sNexonClubID

                    var templates = WvsShop.TemplateManager;

                    var notSales = templates.GetAll<NotSaleTemplate>().ToList();
                    p.Encode<int>(notSales.Count);
                    notSales.ForEach(n => p.Encode<int>(n.ID));

                    var modifiedCommodities = templates.GetAll<ModifiedCommodityTemplate>().ToList();
                    p.Encode<short>((short) modifiedCommodities.Count);
                    modifiedCommodities.ForEach(m =>
                    {
                        var flag = 0;

                        if (m.ItemID.HasValue) flag |= 0x1;
                        if (m.Count.HasValue) flag |= 0x2;
                        if (m.Priority.HasValue) flag |= 0x10;
                        if (m.Price.HasValue) flag |= 0x4;
                        if (m.Bonus.HasValue) flag |= 0x8;
                        if (m.Period.HasValue) flag |= 0x20;
                        if (m.ReqPOP.HasValue) flag |= 0x20000;
                        if (m.ReqLEV.HasValue) flag |= 0x40000;
                        if (m.MaplePoint.HasValue) flag |= 0x40;
                        if (m.Meso.HasValue) flag |= 0x80;
                        if (m.ForPremiumUser.HasValue) flag |= 0x100;
                        if (m.Gender.HasValue) flag |= 0x200;
                        if (m.OnSale.HasValue) flag |= 0x400;
                        if (m.Class.HasValue) flag |= 0x800;
                        if (m.Limit.HasValue) flag |= 0x1000;
                        if (m.PbCash.HasValue) flag |= 0x2000;
                        if (m.PbPoint.HasValue) flag |= 0x4000;
                        if (m.PbGift.HasValue) flag |= 0x8000;
                        if (m.PackageSN != null) flag |= 0x10000;

                        p.Encode<int>(m.ID);
                        p.Encode<int>(flag);

                        if ((flag & 0x1) != 0) p.Encode<int>(m.ItemID.Value);
                        if ((flag & 0x2) != 0) p.Encode<short>(m.Count.Value);
                        if ((flag & 0x10) != 0) p.Encode<byte>(m.Priority.Value);
                        if ((flag & 0x4) != 0) p.Encode<int>(m.Price.Value);
                        if ((flag & 0x8) != 0) p.Encode<byte>(m.Bonus.Value);
                        if ((flag & 0x20) != 0) p.Encode<short>(m.Period.Value);
                        if ((flag & 0x20000) != 0) p.Encode<short>(m.ReqPOP.Value);
                        if ((flag & 0x40000) != 0) p.Encode<short>(m.ReqLEV.Value);
                        if ((flag & 0x40) != 0) p.Encode<int>(m.MaplePoint.Value);
                        if ((flag & 0x80) != 0) p.Encode<int>(m.Meso.Value);
                        if ((flag & 0x100) != 0) p.Encode<bool>(m.ForPremiumUser.Value);
                        if ((flag & 0x200) != 0) p.Encode<byte>(m.Gender.Value);
                        if ((flag & 0x400) != 0) p.Encode<bool>(m.OnSale.Value);
                        if ((flag & 0x800) != 0) p.Encode<byte>(m.Class.Value);
                        if ((flag & 0x1000) != 0) p.Encode<byte>(m.Limit.Value);
                        if ((flag & 0x2000) != 0) p.Encode<short>(m.PbCash.Value);
                        if ((flag & 0x4000) != 0) p.Encode<short>(m.PbPoint.Value);
                        if ((flag & 0x8000) != 0) p.Encode<short>(m.PbGift.Value);
                        if ((flag & 0x10000) == 0) return;
                        p.Encode<byte>((byte) m.PackageSN.Length);
                        m.PackageSN.ForEach(sn => p.Encode<int>(sn));
                    });

                    var categoryDiscounts = templates.GetAll<CategoryDiscountTemplate>().ToList();
                    p.Encode<byte>((byte) categoryDiscounts.Count);
                    categoryDiscounts.ForEach(d =>
                    {
                        p.Encode<byte>(d.Category);
                        p.Encode<byte>(d.CategorySub);
                        p.Encode<byte>(d.DiscountRate);
                    });

                    const int bestLimit = 90;
                    var best = templates.GetAll<BestTemplate>().ToList();
                    best.Take(bestLimit).ForEach(b =>
                    {
                        p.Encode<int>(b.Category);
                        p.Encode<int>(b.Gender);
                        p.Encode<int>(b.CommoditySN);
                    });
                    if (best.Count < bestLimit)
                    {
                        Enumerable.Repeat(0, bestLimit - best.Count).ForEach(i =>
                        {
                            p.Encode<int>(0);
                            p.Encode<int>(0);
                            p.Encode<int>(0);
                        });
                    }

                    // DecodeStock
                    p.Encode<short>(0);
                    // DecodeLimitGoods
                    p.Encode<short>(0);
                    // DecodeZeroGoods
                    p.Encode<short>(0);

                    p.Encode<bool>(false); // m_bEventOn
                    p.Encode<int>(0); // m_nHighestCharacterLevelInThisAccount
                    await SendPacket(p);
                }

                using (var p = new Packet(SendPacketOperations.CashShopQueryCashResult))
                {
                    p.Encode<int>(character.Data.Account.NexonCash);
                    p.Encode<int>(character.Data.Account.MaplePoint);
                    p.Encode<int>(character.Data.Account.PrepaidNXCash);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnTransferFieldRequest(IPacket packet)
        {
            var service = WvsShop.Peers
                .OfType<GameServiceInfo>()
                .Where(g => g.WorldID == Character.Data.WorldID)
                .FirstOrDefault(g => g.Name == Character.Data.Account.PreviousConnectedService);

            if (service != null &&
                !await WvsShop.TryMigrateTo(this, Character, service))
                await Disconnect();
        }
        
        private Task OnCashShopCashItemRequest(IPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}