using Life;
using Life.VehicleSystem;
using Newtonsoft.Json;
using System;

namespace MyPoints.Components.CarDealerPoint
{
    public class CarDealerVehicle
    {
        public double Price { get; set; }
        public int ModelId { get; set; }
        public bool Buyable { get; set; }
        public bool Resellable { get; set; }
        [JsonIgnore]
        public Vehicle Vehicle { get; set; }
        [JsonIgnore]
        public int VehicleIconId { get; set; }

        public CarDealerVehicle(double price, int modelId, bool buyable, bool resellable)
        {
            Price = price;
            ModelId = modelId;
            Buyable = buyable;
            Resellable = resellable;

            Vehicle = Nova.v.vehicleModels[modelId];
            VehicleIconId = getIconId(modelId);
        }

        private int getIconId(int modelId)
        {
            Vehicle vehicle = Nova.v.vehicleModels[modelId];
            int iconId = Array.IndexOf(LifeManager.instance.icons, vehicle.icon);
            return iconId >= 0 ? iconId : Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
        }
    }
}
