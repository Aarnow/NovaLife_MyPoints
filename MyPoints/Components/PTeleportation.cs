using MyPoints.Interfaces;
using System;

namespace MyPoints.Components
{
    public class PTeleportation : IPointAction
    {
        public PTeleportation()
        {
        }

        public void OnPlayerTrigger()
        {
            Console.WriteLine("Action téléportation");
        }
    }
}
