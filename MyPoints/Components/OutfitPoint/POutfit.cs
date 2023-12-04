using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using System.Collections.Generic;
using static PointActionManager;

namespace MyPoints.Components.OutfitPoint
{
    public class POutfit : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }

        public List<Outfit> Outfits = new List<Outfit>();

        public POutfit():base(PointActionKeys.Outfit, "default_job")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {           
        }

        public override void CreateData(Player player)
        {
            POutfit pOutfit = new POutfit();
            OutfitDataPanels.SetOutfitList(player, pOutfit);
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public override object Clone()
        {
            return new POutfit();
        }
    }
}
