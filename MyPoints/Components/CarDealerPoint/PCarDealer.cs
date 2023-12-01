using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System.Collections.Generic;
using static PointActionManager;

namespace MyPoints.Components.CarDealerPoint
{
    public class PCarDealer : PointAction
    {
        public override PointActionKeys ActionKeys { get; }
        public override string Slug { get; set; }
        public List<CarDealerVehicle> CarDealerVehicles { get; set; }
        public PCarDealer()
        {
            ActionKeys = PointActionKeys.CarDealer;
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
    }
}
