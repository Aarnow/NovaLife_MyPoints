using Life.Network;
using MyPoints.Interfaces;
using Newtonsoft.Json;
using System;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PShop : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
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
    }
}
