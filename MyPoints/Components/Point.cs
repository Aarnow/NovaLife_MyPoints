using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Life.CheckpointSystem;
using Life;
using System.IO;
using MyPoints.Interfaces;
using static PointActionManager;
using Life.Network;
using MyPoints.Managers;

namespace MyPoints.Components
{
    [Serializable]
    public class Point
    {
        public uint playerId { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public string dataFilePath { get; set; }
        public PointActionKeys actionKey { get; set; }

        [JsonIgnore]
        public IPointAction action { get; set; }

        public bool isOpen { get; set; }
        public List<int> allowedBizs { get; set; } = new List<int>();
        [JsonIgnore] public Vector3 position { get; set; }
        public float[] positionAxis
        {
            get => new float[] { position.x, position.y, position.z };
            set => position = (value.Length == 3) ? new Vector3(value[0], value[1], value[2]) : position;
        }

        public Point(uint playerId, string slug, string name, string dataFilePath, PointActionKeys actionKey, bool isOpen, List<int> allowedBizs, float[] positionAxis)
        {
            this.playerId = playerId;
            this.slug = slug;
            this.name = name;
            this.dataFilePath = dataFilePath;
            this.actionKey = actionKey;
            this.isOpen = isOpen;
            this.allowedBizs = allowedBizs;
            this.positionAxis = positionAxis;
        }

        public NCheckpoint Build(Player player)
        {  
            try
            {
                string json = File.ReadAllText(dataFilePath);
                IPointAction Action = GetActionByKey(actionKey);
                if (Action != null)
                {
                    Action.UpdateProps(json);

                    NCheckpoint newCheckpoint = new NCheckpoint(playerId, position, delegate
                    {
                        if (isOpen)
                        {
                            if(allowedBizs.Count > 0 && allowedBizs.Contains(player.character.BizId) || allowedBizs.Count == 0)
                            { 
                                Action.OnPlayerTrigger(player);                   
                            }
                            else UIPanelManager.Notification(player, "Accès refusé", "Votre société n'est pas autorisée à intéragir avec ce point.", NotificationManager.Type.Warning);
                        }
                        else UIPanelManager.Notification(player, "Accès refusé", "Ce point est actuellement hors service.", NotificationManager.Type.Warning);
                        
                    });

                    return newCheckpoint;
                } 
                else throw new Exception("Erreur: Action est null");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du fichier JSON : {ex.Message}");
                return null;
            }
        }

        public void Create(Player player)
        {
            NCheckpoint newCheckpoint = Build(player);
            if (newCheckpoint != null)
            {
                Nova.server.Players.ForEach(p => p.CreateCheckpoint(newCheckpoint));
                Save();
            }
            else Console.WriteLine("Erreur lors du build d'un point.");
        }

        private void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.pointPath, $"point_{slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
