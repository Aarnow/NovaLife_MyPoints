using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components.TrashPoint
{
    public class PTrash : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public List<int> AllowedBizs { get; set; } = new List<int>();

        public PTrash(): base(PointActionKeys.Trash, "default_trash")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
            TrashActionPanels.OpenTrash(player, this);
        }

        public override void CreateData(Player player)
        {
            PTrash pTrash = new PTrash();
            TrashDataPanels.SetTrashAllowedBizs(player, pTrash);
        }

        public override void UpdateProps()
        {
            JsonConvert.PopulateObject(File.ReadAllText(Main.dataPath + "/" + ActionKeys + "/" + $"{ActionKeys}_{Slug}.json"), this);
        }

        public override object Clone()
        {
            return new PTrash();
        }
    }
}
