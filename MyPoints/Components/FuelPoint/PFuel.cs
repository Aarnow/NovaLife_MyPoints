using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components.FuelPoint
{
    public class PFuel : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public int Capacity { get; set; }

        public PFuel():base(PointActionKeys.Fuel, "default_fuel")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
        }

        public override void CreateData(Player player)
        {
            PFuel pFuel = new PFuel();
            FuelDataPanels.SetFuelCapacity(player, pFuel);
        }

        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public override object Clone()
        {
            return new PFuel();
        }
    }
}
