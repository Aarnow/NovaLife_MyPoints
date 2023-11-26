using Life.Network;
using MyPoints.Interfaces;
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

        public void OnPlayerTrigger()
        {
            Console.WriteLine("Boutique ouverte");
        }

        public void CreateData(Player player)
        {
        }
    }
}
