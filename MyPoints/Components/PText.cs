using Life.Network;
using MyPoints.Interfaces;
using Newtonsoft.Json;
using System;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PText : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Title { get; set; } 
        public string Content { get; set; }

        public PText()
        {
            ActionKeys = PointActionKeys.Text;
        }

        public void OnPlayerTrigger(Player player)
        {
            Console.WriteLine("Afficher le panel textuel");
        }

        public void CreateData(Player player)
        {
            Console.WriteLine("création des données pour un point textuel");
        }

        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }
    }
}
