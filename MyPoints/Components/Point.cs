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
        public uint PlayerId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string DataFilePath { get; set; }
        public PointActionKeys ActionKey { get; set; }

        [JsonIgnore]
        public IPointAction Action { get; set; }

        public bool IsOpen { get; set; }
        public List<int> AllowedBizs { get; set; } = new List<int>();
        [JsonIgnore] public Vector3 Position { get; set; }
        public float[] PositionAxis
        {
            get => new float[] { Position.x, Position.y, Position.z };
            set => Position = (value.Length == 3) ? new Vector3(value[0], value[1], value[2]) : Position;
        }

        public Point(uint playerId, string slug, string name, string dataFilePath, PointActionKeys actionKey, bool isOpen, List<int> allowedBizs, float[] positionAxis)
        {
            this.PlayerId = playerId;
            this.Slug = slug;
            this.Name = name;
            this.DataFilePath = dataFilePath;
            this.ActionKey = actionKey;
            this.IsOpen = isOpen;
            this.AllowedBizs = allowedBizs;
            this.PositionAxis = positionAxis;
        }

        public NCheckpoint Build(Player player)
        {  
            try
            {
                string json = File.ReadAllText(DataFilePath);
                IPointAction Action = GetActionByKey(ActionKey);
                if (Action != null)
                {
                    Action.UpdateProps(json);

                    NCheckpoint newCheckpoint = new NCheckpoint(PlayerId, Position, delegate
                    {
                        if (IsOpen)
                        {
                            if(AllowedBizs.Count > 0 && AllowedBizs.Contains(player.character.BizId) || AllowedBizs.Count == 0)
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

        public void Delete(Player player)
        {
            string path = Main.pointPath + "/Point_" + Slug + ".json";
            Console.WriteLine(path);
            if (File.Exists(path)) File.Delete(path);

            foreach (NCheckpoint checkpoint in Nova.server.checkpoints)
                if (checkpoint.position == Position)
                    foreach (Player p in Nova.server.GetAllInGamePlayers())
                        p.DestroyCheckpoint(checkpoint);
            UIPanelManager.Notification(player, "Succès", "Le point à bien été supprimé.", NotificationManager.Type.Success);
        }

        private void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                Slug += $"_{number}";
                filePath = Path.Combine(Main.pointPath, $"Point_{Slug}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
