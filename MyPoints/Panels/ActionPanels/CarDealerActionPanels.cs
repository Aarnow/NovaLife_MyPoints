using Life;
using Life.DB;
using Life.Network;
using Life.UI;
using MyPoints.Components.CarDealerPoint;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class CarDealerActionPanels
    {
        public static void CarDealerVehiculeList(Player player, PCarDealer pCarDealer)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"{pCarDealer.GetSlug()}");

            foreach ((CarDealerVehicle carDealerVehicle, int index) in pCarDealer.CarDealerVehicles.Select((carDealerVehicle, index) => (carDealerVehicle, index)))
            {
                panel.AddTabLine($"{carDealerVehicle.Vehicle.vehicleName}", carDealerVehicle.Price.ToString("F2") + "€", carDealerVehicle.VehicleIconId, (ui) => { ui.selectedTab = index; });
            }

            panel.AddButton("Acheter", (ui) =>
            {
                if (pCarDealer.CarDealerVehicles[ui.selectedTab].Buyable) PanelManager.NextPanel(player, ui, () => ConfirmPurchase(player, pCarDealer, pCarDealer.CarDealerVehicles[ui.selectedTab]));
                else PanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être acheté.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ConfirmPurchase(Player player, PCarDealer pCarDealer, CarDealerVehicle carDealerVehicle)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Confirmation d'achat");

            panel.text = $"Confirmez-vous l'achat du véhicule modèle: \"{carDealerVehicle.Vehicle.vehicleName}\" au prix de {carDealerVehicle.Price}€ ?";

            panel.AddButton("Confirmer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                if (player.character.Money >= carDealerVehicle.Price)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        LifeDB.CreateVehicle(carDealerVehicle.ModelId, $"{{\"owner\":{{\"groupId\":0,\"characterId\":{player.character.Id}}},\"coOwners\":[]}}");
                        PanelManager.Notification(player, "Succès", $"Félicitation, vous venez d'acquérir le véhicule modèle: \"{carDealerVehicle.Vehicle.vehicleName}\" pour {carDealerVehicle.Price}€.", NotificationManager.Type.Success);
                        player.AddMoney(-carDealerVehicle.Price, "transaction");
                        PurchaseCompleted(player, pCarDealer, carDealerVehicle);
                    });
                }
                else
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        PanelManager.Notification(player, "Erreur", "Vous n'avez pas les moyens pour acheter ce véhicule.", NotificationManager.Type.Error);
                        pCarDealer.OnPlayerTrigger(player);
                    });
                };
            }));
            panel.AddButton("Annuler", (ui) => PanelManager.NextPanel(player, ui, () => pCarDealer.OnPlayerTrigger(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PurchaseCompleted(Player player, PCarDealer pCarDealer, CarDealerVehicle carDealerVehicle)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Félicitation");

            panel.text = $"Votre véhicule modèle: \"{carDealerVehicle.Vehicle.name}\" est disponible dans votre garage virtuel.\nRendez-vous chez votre concessionnaire !";

            panel.AddButton("Boutique", (ui) => PanelManager.NextPanel(player, ui, () => pCarDealer.OnPlayerTrigger(player)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
