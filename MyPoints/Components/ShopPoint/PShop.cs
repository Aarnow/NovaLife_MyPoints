using Life.Network;
using MyPoints.Interfaces;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System.IO;
using static PointActionManager;
using System.Collections.Generic;
using MyPoints.Panels.ActionPanels;

namespace MyPoints.Components.ShopPoint
{
    public class PShop : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public List<ShopItem> shopItems { get; set; }
        public PShop()
        {
            ActionKeys = PointActionKeys.Shop;
            shopItems = new List<ShopItem>();
        }

        public void OnPlayerTrigger(Player player)
        {
            ShopActionPanels.OpenShop(player, this);
        }

        public void CreateData(Player player)
        {
            PShop pShop = new PShop();
            ShopDataPanels.ShopCreationInstructions(player, pShop);
        }
        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + PointActionKeys.Shop, $"shop_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public object Clone()
        {
            return new PShop();
        }
    }
}
