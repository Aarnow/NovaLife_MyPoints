using Life;
using Life.Network;
using Life.UI;
using Life.VehicleSystem;
using MyPoints.Components;
using MyPoints.Managers;
using System;

namespace MyPoints.Panels.PanelsData
{
    abstract class CarDealerDataPanels
    {

        public static void CarDealerCreationInstructions(Player player, PCarDealer pCarDealer)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Création d'une boutique de véhicules");

            panel.text = $"\nRendez-vous sur le wiki officiel pour obtenir les ID des véhicules que vous souhaitez intégrer à votre boutique.\n\n<color={UIPanelManager.Colors[NotificationManager.Type.Warning]}>https://sites.google.com/view/nova-life-wiki/aides/</color>";

            panel.AddButton("Commencer", (ui) => UIPanelManager.NextPanel(player, ui, () => SetVehicleList(player, pCarDealer)));
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetVehicleList(Player player, PCarDealer pCarDealer)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"Sélectionner un véhicule");

            //DEBUG
            foreach (Vehicle v in Nova.v.vehicleModels)
            {
                int iconId = Array.IndexOf(LifeManager.instance.icons, v.icon);
                panel.AddTabLine($"{v.vehicleName}", "€", iconId, (ui) => { ui.selectedTab = 0; });
            }

            /*foreach ((ShopItem shopItem, int index) in pShop.shopItems.Select((shopItem, index) => (shopItem, index)))
            {
                panel.AddTabLine($"{shopItem.Item.itemName}", shopItem.Price.ToString("F2") + "€", shopItem.ItemIconId, (ui) => { ui.selectedTab = 0; });
            }*/

            //panel.AddButton("Ajouter", (ui) => UIPanelManager.NextPanel(player, ui, () => AddItem(player, pShop)));
            /* panel.AddButton("Supprimer", (ui) => UIPanelManager.NextPanel(player, ui, () =>
             {
                 pShop.shopItems.RemoveAt(ui.selectedTab);
                 SetItemList(player, pShop);
             }));*/
            //panel.AddButton("Valider", (ui) => UIPanelManager.NextPanel(player, ui, () => SetSlug(player, pShop)));
            panel.AddButton("Quitter", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
