using Life.Network;
using MyPoints.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PShop : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public PShop()
        {
            ActionKeys = PointActionKeys.Text;
        }

        public void OnPlayerTrigger(Player player)
        {
            Console.WriteLine("Boutique ouverte");
        }

        public void CreateData(Player player)
        {
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
    }
}
