using Life;
using System;

namespace MyPoints
{
    public class Main : Plugin
    {
        public Main(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();

            Console.WriteLine($"Plugin \"MyPoints\" initialisé avec succès.");
        }
    }
}