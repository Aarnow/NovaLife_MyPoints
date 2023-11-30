using Life;
using Life.DB;
using Life.Network;
using Life.UI;
using MyPoints.Components.CarDealerPoint;
using MyPoints.Managers;
using System.Linq;

namespace MyPoints.Panels.ActionPanels
{
    abstract class CarDealerActionPanels
    {
        public static void CarDealerVehiculeList(Player player, PCarDealer pCarDealer)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"{pCarDealer.Slug}");

            foreach ((CarDealerVehicle carDealerVehicle, int index) in pCarDealer.carDealerVehicles.Select((carDealerVehicle, index) => (carDealerVehicle, index)))
            {
                panel.AddTabLine($"{carDealerVehicle.Vehicle.vehicleName}", carDealerVehicle.Price.ToString("F2") + "€", carDealerVehicle.VehicleIconId, (ui) => { ui.selectedTab = index; });
            }

            panel.AddButton("Acheter", (ui) =>
            {
                if (pCarDealer.carDealerVehicles[ui.selectedTab].Buyable) UIPanelManager.NextPanel(player, ui, () => ConfirmPurchase(player, pCarDealer, pCarDealer.carDealerVehicles[ui.selectedTab]));
                else UIPanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être acheté.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ConfirmPurchase(Player player, PCarDealer pCarDealer, CarDealerVehicle carDealerVehicle)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Confirmation d'achat");

            panel.text = $"Confirmez-vous l'achat du véhicule modèle: \"{carDealerVehicle.Vehicle.vehicleName}\" au prix de {carDealerVehicle.Price}€ ?";

            panel.AddButton("Confirmer", (ui) => UIPanelManager.NextPanel(player, ui, () =>
            {
                if (player.character.Money >= carDealerVehicle.Price)
                {
                    UIPanelManager.NextPanel(player, ui, () =>
                    {
                        LifeDB.CreateVehicle(carDealerVehicle.ModelId, $"{{\"owner\":{{\"groupId\":0,\"characterId\":{player.character.Id}}},\"coOwners\":[]}}");
                        UIPanelManager.Notification(player, "Succès", $"Félicitation, vous venez d'acquérir le véhicule modèle: \"{carDealerVehicle.Vehicle.vehicleName}\" pour {carDealerVehicle.Price}€.", NotificationManager.Type.Success);
                        player.AddMoney(-carDealerVehicle.Price, "transaction");
                        PurchaseCompleted(player, pCarDealer, carDealerVehicle);
                    });
                }
                else
                {
                    UIPanelManager.NextPanel(player, ui, () =>
                    {
                        UIPanelManager.Notification(player, "Erreur", "Vous n'avez pas les moyens pour acheter ce véhicule.", NotificationManager.Type.Error);
                        pCarDealer.OnPlayerTrigger(player);
                    });
                };
            }));
            panel.AddButton("Annuler", (ui) => UIPanelManager.NextPanel(player, ui, () => pCarDealer.OnPlayerTrigger(player)));
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PurchaseCompleted(Player player, PCarDealer pCarDealer, CarDealerVehicle carDealerVehicle)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Félicitation");

            panel.text = $"Votre véhicule modèle: \"{carDealerVehicle.Vehicle.name}\" est disponible dans votre garage virtuel.\nRendez-vous chez votre concessionnaire !";

            panel.AddButton("Boutique", (ui) => UIPanelManager.NextPanel(player, ui, () => pCarDealer.OnPlayerTrigger(player)));
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
