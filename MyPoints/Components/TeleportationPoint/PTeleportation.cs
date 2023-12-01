using System;
using Life.Network;
using Newtonsoft.Json;
using MyPoints.Common;
using MyPoints.Panels.PanelsData;
using static PointActionManager;
using UnityEngine;

namespace MyPoints.Components.TeleportationPoint
{
    public class PTeleportation : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public PTeleportation(float x = 0.0f, float y = 0.0f, float z = 0.0f)
            :base(PointActionKeys.Teleportation, "default_tp")
        {
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

        public override object Clone()
        {
            return new PTeleportation();
        }
    }
}
