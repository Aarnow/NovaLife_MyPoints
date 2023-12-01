﻿using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components.TextPoint
{
    public class PText : PointAction
    {
        public override PointActionKeys ActionKeys { get; }
        public override string Slug { get; set; }
        public string Content { get; set; }

        public PText()
        {
            ActionKeys = PointActionKeys.Text;
        }

        public override void OnPlayerTrigger(Player player)
        {
            TextActionPanels.ShowTextPanel(player, this);
        }

        public override void CreateData(Player player)
        {
            PText pText = new PText();
            TextDataPanels.SetTextSlug(player, pText);
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }
    }
}
