using Life.Network;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System.IO;
using static PointActionManager;
using System.Collections.Generic;
using MyPoints.Panels.ActionPanels;
using MyPoints.Common;

namespace MyPoints.Components.ShopPoint
{
    public class PShop : PointAction
    {
        public override PointActionKeys ActionKeys { get; }
        public override string Slug { get; set; }
        public List<ShopItem> ShopItems { get; set; }
        public PShop()
        {
            ActionKeys = PointActionKeys.Shop;
            ShopItems = new List<ShopItem>();
        }

        public override void OnPlayerTrigger(Player player)
        {
            ShopActionPanels.OpenShop(player, this);
        }

        public override void CreateData(Player player)
        {
            PShop pShop = new PShop();
            ShopDataPanels.ShopCreationInstructions(player, pShop);
        }
        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }
    }
}
