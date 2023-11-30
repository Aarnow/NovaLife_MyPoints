using Life.Network;
using MyPoints.Interfaces;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PCarDealer : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public List<CarDealerVehicle> carDealerVehicles { get; set; }

        public PCarDealer()
        {
            ActionKeys = PointActionKeys.CarDealer;
            carDealerVehicles = new List<CarDealerVehicle>();
        }

        public void OnPlayerTrigger(Player player)
        {
            CarDealerActionPanels.CarDealerVehiculeList(player, this);
        }

        public void CreateData(Player player)
        {
            PCarDealer pCarDealer = new PCarDealer();
            CarDealerDataPanels.CarDealerCreationInstructions(player, pCarDealer);
        }

        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + ActionKeys, $"{ActionKeys}_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public object Clone()
        {
            return new PCarDealer();
        }

    }
}
