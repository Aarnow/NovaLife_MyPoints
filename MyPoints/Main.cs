using Life;
using Life.Network;
using MyPoints.Components;
using System;
using System.Collections.Generic;
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
                    Vector3 position = player.setup.transform.position;

                    Point newPoint = new Point(player.netId, "mySlug", true, new List<int>(), new float[] { position.x, position.y, position.z });

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