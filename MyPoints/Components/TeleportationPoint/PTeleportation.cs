using MyPoints.Interfaces;
using System;
using Life.Network;
using MyPoints.Panels.PanelsData;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using static PointActionManager;
using MyPoints.Common;

namespace MyPoints.Components.TeleportationPoint
{
    [Serializable]
    public class PTeleportation : PointAction
    {
        [JsonIgnore]
        public override PointActionKeys ActionKeys { get; }
        public override string Slug { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public PTeleportation(float x = 0.0f, float y = 0.0f, float z = 0.0f)
        {
            ActionKeys = PointActionKeys.Teleportation;
            X = x;
            Y = y;
            Z = z;
        }

        public override void OnPlayerTrigger(Player player)
        {
            player.setup.TargetSetPosition(new Vector3(X, Y, Z));
        }

        public override void CreateData(Player player)
        {
            PTeleportation pTeleportation = new PTeleportation();
            TeleportationDataPanels.SetTeleportationSlug(player, pTeleportation);
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void SetPositionAxis(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }
    }
}
