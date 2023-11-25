using Life;
using Life.CheckpointSystem;
using Life.DB;
using Life.Network;
using Mirror;
using MyPoints.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MyPoints
{
    public class Main : Plugin
    {
        public static string directoryPath;
        public static string pointsPath;

        public Main(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();

            new SChatCommand("/mypoints", "Permet d'ouvrir le panel du plugin MyPoints", "/mypoints", (player, arg) =>
                {
                    /* étapes préliminaires à la création d'un point
                     * ACTION à réaliser (boutique, téléportation, information)
                     * DATA à connecter au point (articles de la boutique ? coordoonnées du lieu de tp ? texte ?)
                     * ALLOWED BIZS à renseigner via une liste des sociétés existantes
                     * IS OPEN à définir sur VRAI ou FAUX
                     * NAME et confirmer la création du point aux coordonnées du joueur
                     */

                    Vector3 position = player.setup.transform.position;
                    Point newPoint = new Point(player.netId, "mySlug", "Nom du point", "Nom du jeu de données", "Shop", true, new List<int>(), new float[] { position.x, position.y, position.z });
                    newPoint.Create();
                }).Register();
            
            Console.WriteLine($"Plugin \"MyPoints\" initialisé avec succès.");
        }

        public override void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);


            try
            {
                string[] jsonFiles = Directory.GetFiles(pointsPath, "*.json");
                foreach (string jsonFile in jsonFiles)
                {
                    string json = File.ReadAllText(jsonFile);
                    Point point = JsonConvert.DeserializeObject<Point>(json);
                    NCheckpoint newCheckpoint = point.Build();
                    player.CreateCheckpoint(newCheckpoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la lecture des fichiers JSON : " + ex.Message);
            }
        }

        public void InitDirectory()
        {
            directoryPath = pluginsPath + "/MyPoints";
            pointsPath = directoryPath + "/Points";

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            if (!Directory.Exists(pointsPath)) Directory.CreateDirectory(pointsPath);
        }
    }
}