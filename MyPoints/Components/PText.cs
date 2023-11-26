using Life.Network;
using MyPoints.Interfaces;
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

        public void OnPlayerTrigger()
        {
            Console.WriteLine("Afficher le panel textuel");
        }

        public void CreateData(Player player)
        {
            Console.WriteLine("création des données pour un point textuel");
        }
    }
}
