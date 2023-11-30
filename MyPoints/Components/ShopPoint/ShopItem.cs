using Life;
using Life.InventorySystem;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace MyPoints.Components.ShopPoint
{
    public class ShopItem
    {
        public double Price { get; set; }
        public int ItemId { get; set; }
        public bool Buyable { get; set; }
        public bool Resellable { get; set; }
        public string Data { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [JsonIgnore]
        public int ItemIconId { get; set; }

        public ShopItem(double price, int itemId, bool buyable, bool resellable, string data = null)
        {
            Price = price;
            ItemId = itemId;
            Buyable = buyable;
            Resellable = resellable;
            Data = itemWithData(itemId) ? "{\"statePercentage\":100.0}" : null;

            Item = LifeManager.instance.item.GetItem(itemId);
            ItemIconId = getIconId(itemId);
        }

        private int getIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : getIconId(1112);
        }

        private bool itemWithData(int itemId)
        {
            int[] allowedItemIDs = { 3, 4, 5 };
            return allowedItemIDs.Contains(itemId) ? true : false;
        }
    }
}
