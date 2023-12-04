using Life.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using MyPoints.Panels.ActionPanels;
using MyPoints.Common;
using static PointActionManager;
using MyPoints.Panels.DPanels;

namespace MyPoints.Components.CarDealerPoint
{
    public class PCarDealer : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public List<CarDealerVehicle> CarDealerVehicles { get; set; }
        public PCarDealer()
            :base(PointActionKeys.CarDealer, "default_carDealer")
        {
            CarDealerVehicles = new List<CarDealerVehicle>();
        }

        public override void OnPlayerTrigger(Player player)
        {
            CarDealerActionPanels.CarDealerVehiculeList(player, this);
        }

        public override void CreateData(Player player)
        {
            PCarDealer pCarDealer = new PCarDealer();
            CarDealerDataPanels.CarDealerCreationInstructions(player, pCarDealer);
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public override object Clone()
        {
            return new PCarDealer();
        }
    }
}
