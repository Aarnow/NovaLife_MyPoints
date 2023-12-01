﻿using Life.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using MyPoints.Panels.PanelsData;
using MyPoints.Panels.ActionPanels;
using MyPoints.Common;
using static PointActionManager;

namespace MyPoints.Components.ShopPoint
{
    public class PShop : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }
        public List<ShopItem> ShopItems { get; set; }
        public PShop()
            :base(PointActionKeys.Shop, "default_shop")
        {
            ShopItems = new List<ShopItem>();
        }

        public override void OnPlayerTrigger(Player player)
        {
            ShopActionPanels.OpenShop(player, this);
        }

        public override void CreateData(Player player)
        {
            PShop pShop = new PShop();
            ShopDataPanels.ShopCreationInstructions(player, pShop);
        }
        public override void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public override object Clone()
        {
            return new PShop();
        }
    }
}