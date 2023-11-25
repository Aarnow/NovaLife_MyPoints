using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Life.Network;
using Life.CheckpointSystem;
using Life;

namespace MyPoints.Components
{
    [Serializable]
    public class Point
    {
        public uint playerId { get; set; }
        public string slug { get; set; }
        public bool isOpen { get; set; }
        public List<int> allowedBizs { get; set; } = new List<int>();
        [JsonIgnore] public Vector3 position { get; set; }
        public float[] positionAxis
        {
            get => new float[] { position.x, position.y, position.z };
            set => position = (value.Length == 3) ? new Vector3(value[0], value[1], value[2]) : position;
        }

        public Point(uint playerId, string slug, bool isOpen, List<int> allowedBizs, float[] positionAxis)
        {
            this.playerId = playerId;
            this.slug = slug;
            this.isOpen = isOpen;
            this.allowedBizs = allowedBizs;
            this.positionAxis = positionAxis;

            Create();
        }

        private void Create()
        {

            NCheckpoint newCheckpoint = new NCheckpoint(playerId, position, delegate
            {
                Console.WriteLine("Bienvenue dans le checkpoint !");
            });

            Nova.server.Players.ForEach(p => p.CreateCheckpoint(newCheckpoint));
        }
    }
}
