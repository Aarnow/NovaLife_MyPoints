using MyPoints.Interfaces;
using System;

namespace MyPoints.Components
{
    public class PText : IPointAction
    {
        public string name { get; set; } 
        public string content { get; set; }

        public PText()
        {

        }

        public void OnPlayerTrigger()
        {
            Console.WriteLine("Afficher le panel textuel");
        }
    }
}
