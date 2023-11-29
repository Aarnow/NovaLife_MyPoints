
using Life.Network;
using MyPoints.Interfaces;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using static PointActionManager;

namespace MyPoints.Components
{
    public class PCarDealer : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }

        public PCarDealer()
        {

        }

        public void OnPlayerTrigger(Player player)
        {

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
        }

        public object Clone()
        {
            return new PCarDealer();
        }

    }
}
