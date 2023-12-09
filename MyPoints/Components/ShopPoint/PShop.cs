using Life.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using MyPoints.Panels.DPanels;
using MyPoints.Panels.ActionPanels;
using MyPoints.Common;
using static PointActionManager;
using System.IO;

namespace MyPoints.Components.ShopPoint 
{
    public class PShop : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public List<ShopItem> ShopItems { get; set; }
        public PShop()
            :base(PointActionKeys.Shop, "default_shop")
        {
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
        public override void UpdateProps()
        {
            JsonConvert.PopulateObject(File.ReadAllText(Main.dataPath + "/" + ActionKeys + "/" + $"{ActionKeys}_{Slug}.json"), this);
        }

        public override object Clone()
        {
            return new PShop();
        }
    }
}
