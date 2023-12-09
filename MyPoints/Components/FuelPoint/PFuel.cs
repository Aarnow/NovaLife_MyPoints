using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components.FuelPoint
{
    public class PFuel : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public int Capacity { get; set; }
        public int CurrentQuantity { get; set; }

        public PFuel():base(PointActionKeys.Fuel, "default_fuel")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
            FuelActionPanels.OpenFuelCan(player, this);
        }

        public override void CreateData(Player player)
        {
            PFuel pFuel = new PFuel();
            pFuel.CurrentQuantity = 0;
            FuelDataPanels.SetFuelCapacity(player, pFuel);
        }

        public override void UpdateProps()
        {
            JsonConvert.PopulateObject(File.ReadAllText(Main.dataPath + "/" + ActionKeys + "/" + $"{ActionKeys}_{Slug}.json"), this);
        }

        public override object Clone()
        {
            return new PFuel();
        }

        public void UpdateData()
        {
            string filePath = Path.Combine(Main.dataPath + "/" + ActionKeys, $"{ActionKeys}_{Slug}.json");
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
