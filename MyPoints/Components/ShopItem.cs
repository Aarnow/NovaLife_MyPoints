using Life;
using Life.InventorySystem;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace MyPoints.Components
{
    public class ShopItem
    {
        public double Price { get; set; }
        public int ItemId { get; set; }
        public bool Buyable { get; set; }
        public bool Resellable { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [JsonIgnore]
        public int ItemIconId { get; set; }

        public ShopItem (double price, int itemId, bool buyable, bool resellable)
        {
            Price = price;
            ItemId = itemId;
            Buyable = buyable;
            Resellable = resellable;

            Item = LifeManager.instance.item.GetItem(itemId);
            ItemIconId = getIconId(itemId);
        }

        private int getIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : getIconId(1112);
        }
    }
}
