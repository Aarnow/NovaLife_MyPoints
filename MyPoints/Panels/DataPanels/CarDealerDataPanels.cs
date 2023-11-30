using Life;
using Life.Network;
using Life.UI;
using Life.VehicleSystem;
using MyPoints.Components;
using MyPoints.Managers;
using System;
using System.Linq;

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
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"Aperçu de votre boutique");

            foreach ((CarDealerVehicle carDealerVehicle, int index) in pCarDealer.carDealerVehicles.Select((carDealerVehicle, index) => (carDealerVehicle, index)))
            {
                panel.AddTabLine($"{carDealerVehicle.Vehicle.vehicleName}", carDealerVehicle.Price.ToString("F2") + "€", carDealerVehicle.VehicleIconId, (ui) => { ui.selectedTab = index; });
            }

            panel.AddButton("Ajouter", (ui) => UIPanelManager.NextPanel(player, ui, () => AddVehicle(player, pCarDealer)));
            panel.AddButton("Supprimer", (ui) => UIPanelManager.NextPanel(player, ui, () =>
            {
                pCarDealer.carDealerVehicles.RemoveAt(ui.selectedTab);
                SetVehicleList(player, pCarDealer);
            }));
            panel.AddButton("Valider", (ui) => UIPanelManager.NextPanel(player, ui, () => SetSlug(player, pCarDealer)));
            panel.AddButton("Quitter", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void AddVehicle(Player player, PCarDealer pCarDealer, bool buyable = true, bool resellable = false)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Ajouter un véhicule");

            panel.inputPlaceholder = "ID du véhicule [ESPACE] Prix du véhicule";

            panel.AddButton($"Achetable: {(buyable ? $"<color={UIPanelManager.Colors[NotificationManager.Type.Success]}>OUI</color>" : $"<color={UIPanelManager.Colors[NotificationManager.Type.Error]}>NON</color>")}", (ui) => UIPanelManager.NextPanel(player, ui, () => AddVehicle(player, pCarDealer, !buyable, resellable)));
            panel.AddButton($"Vendable: {(resellable ? $"<color={UIPanelManager.Colors[NotificationManager.Type.Success]}>OUI</color>" : $"<color={UIPanelManager.Colors[NotificationManager.Type.Error]}>NON</color>")}", (ui) => UIPanelManager.NextPanel(player, ui, () => AddVehicle(player, pCarDealer, buyable, !resellable)));
            panel.AddButton("Confirmer", (ui) =>
            {
                string[] inputText = ui.inputText.Split(' ');
                if (inputText.Length == 2)
                {
                    if (int.TryParse(inputText[0], out int vehicleId))
                    {
                        if (vehicleId >= 0 && vehicleId < Nova.v.vehicleModels.Length)
                        {
                            if (!Nova.v.vehicleModels[vehicleId].isDeprecated)
                            {
                                inputText[1] = inputText[1].Replace(',', '.');
                                if (double.TryParse(inputText[1], out double vehiclePrice) && vehiclePrice >= 0)
                                {
                                    UIPanelManager.NextPanel(player, ui, () =>
                                    {
                                        vehiclePrice = Math.Ceiling(vehiclePrice * 100) / 100;
                                        CarDealerVehicle carDealerVehicle = new CarDealerVehicle(vehiclePrice, vehicleId, buyable, resellable);
                                        pCarDealer.carDealerVehicles.Add(carDealerVehicle);
                                        SetVehicleList(player, pCarDealer);
                                    });
                                }
                                else UIPanelManager.Notification(player, "Erreur", "Vous devez définir un prix au bon format et positif.", NotificationManager.Type.Error);
                            }
                            else UIPanelManager.Notification(player, "Erreur", $"Vous ne pouvez pas importer un véhicule qui est déprécié.", NotificationManager.Type.Error);
                        }
                        else UIPanelManager.Notification(player, "Erreur", $"Nous n'avons aucun véhicule correspondant à l'ID: {vehicleId}", NotificationManager.Type.Error);
                    }
                    else UIPanelManager.Notification(player, "Erreur", "Vous devez indiquer l'identifiant du véhicule.\nID [ESPACE] PRIX", NotificationManager.Type.Error);
                }
                else UIPanelManager.Notification(player, "Erreur", "Vous devez respecter le format et ne fournir que l'ID et le prix.\nID [ESPACE] PRIX", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => UIPanelManager.NextPanel(player, ui, () => SetVehicleList(player, pCarDealer)));

            player.ShowPanelUI(panel);
        }

        public static void SetSlug(Player player, PCarDealer pCarDealer)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Confirmer votre boutique");

            panel.inputPlaceholder = "Quel est le nom de votre boutique ?";

            panel.AddButton("Confirmer", (ui) => UIPanelManager.NextPanel(player, ui, () =>
            {
                pCarDealer.Slug = ui.inputText;
                pCarDealer.Save();
                UIPanelManager.Notification(player, "Succès", "Les données de votre boutique ont bien été sauvegardées.", NotificationManager.Type.Success);
            }));
            panel.AddButton("Quitter", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void SetVehicleListDEBUG(Player player, PCarDealer pCarDealer)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"Sélectionner un véhicule");

            //DEBUG
            foreach ((Vehicle vehicle, int index) in Nova.v.vehicleModels.Select((vehicle, index) => (vehicle, index)))
            {
                if (!vehicle.isDeprecated)
                {
                    int iconId = Array.IndexOf(LifeManager.instance.icons, vehicle.icon);
                    if (iconId == -1) iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
                    panel.AddTabLine($"{index}:{vehicle.vehicleName}", $"<color={UIPanelManager.Colors[NotificationManager.Type.Success]}>ajouté</color>", iconId, (ui) => { ui.selectedTab = index; });
                } 
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
