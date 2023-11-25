using Life;
using Life.CheckpointSystem;
using Life.Network;
using System;
using System.IO;
using UnityEngine;

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
            InitDirectory();

            new SChatCommand("/mypoints", "Permet d'ouvrir le panel du plugin MyPoints", "/mypoints", (player, arg) =>
                {

                    Vector3 pos = player.setup.transform.position;
                    NCheckpoint newCheckpoint = new NCheckpoint(player.setup.areaId, pos, delegate
                    {
                        Console.WriteLine("Bienvenue dans le checkpoint !");
                    });

                    player.CreateCheckpoint(newCheckpoint);

                }).Register();
            
            Console.WriteLine($"Plugin \"MyPoints\" initialisé avec succès.");
        }

        public void InitDirectory()
        {
            string directoryPath = pluginsPath + "/MyPoints";
            string pointsPath = directoryPath + "/Points";

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!Directory.Exists(pointsPath)) Directory.CreateDirectory(pointsPath);
        }
    }
}