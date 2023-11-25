using MyPoints.Interfaces;
using System;

namespace MyPoints.Components
{
    public class PShop : IPointAction
    {
        public PShop()
        {
        }

        public void OnPlayerTrigger()
        {
            Console.WriteLine("Boutique ouverte");
        }
    }
}
