using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.FuelPoint;
using System;
using UIPanelManager;

namespace MyPoints.Panels.DPanels
{
    abstract class FuelDataPanels
    {
        public static void SetFuelCapacity(Player player, PFuel pFuel)
        {
            int canCapacity = 15;
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Créer une cuve d'essence");

            panel.inputPlaceholder = "Quantité maximum d'essence (valeur divisible par 15)";

            panel.AddButton("Confirmer", (ui) => {
                if (ui.inputText.Length > 0)
                {
                    if (int.TryParse(ui.inputText, out int capacity))
                    {
                        if(capacity % canCapacity == 0)
                        {
                            PanelManager.NextPanel(player, ui, () =>
                            {
                                pFuel.Capacity = capacity;
                                SetFuelSlug(player, pFuel);
                            });
                        } else
                        {
                            PanelManager.NextPanel(player, ui, () =>
                            {
                                int lowerMultiple = (int)(Math.Floor((double)capacity / canCapacity) * canCapacity);
                                int upperMultiple = lowerMultiple + canCapacity;
                                ConfirmFuelCapacity(player, pFuel, lowerMultiple, upperMultiple);
                            });
                        }                      
                    }
                    else PanelManager.Notification(player, "Erreur", "Vous devez définir une valeur au bon format et positif.", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez fournir une quantité", NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ConfirmFuelCapacity(Player player, PFuel pFuel, int lowerMultiple, int upperMultiple)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Créer une cuve d'essence");

            panel.text = $"Vous avez renseignez une valeur qui n'est pas divisible par 15.\n Nous vous proposons de faire un choix parmis les valeurs suivantes:\n {lowerMultiple}L ou {upperMultiple}L";

            panel.AddButton($"{lowerMultiple}L", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pFuel.Capacity = lowerMultiple;
                    SetFuelSlug(player, pFuel);
                });
            });
            panel.AddButton($"{upperMultiple}L", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pFuel.Capacity = upperMultiple;
                    SetFuelSlug(player, pFuel);
                });
            });
            panel.AddButton("Retour", (ui) => PanelManager.Quit(ui, player));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetFuelSlug(Player player, PFuel pFuel)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Créer une cuve d'essence");

            panel.inputPlaceholder = "Nommer votre cuve d'essence";           

            panel.AddButton("Sauvegarder", (ui) => PanelManager.NextPanel(player, ui, ()=>
            {
                pFuel.Slug = ui.inputText;
                pFuel.Save();
                PanelManager.Notification(player, "Succès", "Les données de votre cuve d'essence ont bien été sauvegardées.", NotificationManager.Type.Success);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetFuelCapacity(player, pFuel)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

    }
}
