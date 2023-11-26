using MyPoints.Interfaces;
using System;
using Life.Network;
using MyPoints.Panels.PanelsData;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components
{
    [Serializable]
    public class PTeleportation : IPointAction
    {
        [JsonIgnore]
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public PTeleportation(string slug = "Default", float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            ActionKeys = PointActionKeys.Teleportation;
            Slug = slug;
            X = x;
            Y = y;
            Z = z;
        }

        public void OnPlayerTrigger(Player player)
        {
            player.setup.TargetSetPosition(new Vector3(X, Y, Z));
        }

        public void CreateData(Player player)
        {
            PTeleportation pTeleportation = new PTeleportation();
            TeleportationDataPanels.SetTeleportationSlug(player, pTeleportation);
        }

        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + PointActionKeys.Teleportation, $"teleportation_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void SetPositionAxis(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }
    }
}
