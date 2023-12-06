using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.FuelPoint;
using System.IO;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    public class FuelActionPanels
    {
        public static void OpenFuelCan(Player player, PFuel pFuel)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pFuel.Slug}");

            panel.text = $"{pFuel.CurrentQuantity} / {pFuel.Capacity}L";

            panel.AddButton("Ajouter", (ui) =>
            {
                string json = File.ReadAllText(Main.dataPath + "/" + pFuel.ActionKeys + "/" + $"{pFuel.ActionKeys}_{pFuel.Slug}.json");
                pFuel.UpdateProps(json);

                if (pFuel.CurrentQuantity >= pFuel.Capacity) PanelManager.Notification(player, "Erreur", "La cuve d'essence est pleine !", NotificationManager.Type.Error);
                else
                {
                    if (player.setup.inventory.RemoveItem(1564, 1, false))
                    {
                        player.setup.inventory.AddItem(1183, 1, null);
                        pFuel.CurrentQuantity += 15;

                        pFuel.UpdateData();
                        PanelManager.NextPanel(player, ui, () => OpenFuelCan(player, pFuel));
                    } else PanelManager.Notification(player, "Erreur", "Vous n'avez pas de bidon d'essence plein", NotificationManager.Type.Error);
                }
               
            });
            panel.AddButton("Récupérer", (ui) =>
            {
                string json = File.ReadAllText(Main.dataPath + "/" + pFuel.ActionKeys + "/" + $"{pFuel.ActionKeys}_{pFuel.Slug}.json");
                pFuel.UpdateProps(json);

                if (pFuel.CurrentQuantity < 0) PanelManager.Notification(player, "Erreur", "La cuve d'essence est vide !", NotificationManager.Type.Error);
                else
                {
                    if (player.setup.inventory.RemoveItem(1183, 1, false))
                    {
                        player.setup.inventory.AddItem(1564, 1, null);
                        pFuel.CurrentQuantity -= 15;

                        pFuel.UpdateData();
                        PanelManager.NextPanel(player, ui, () => OpenFuelCan(player, pFuel));
                    }
                    else PanelManager.Notification(player, "Erreur", "Vous n'avez pas de bidon d'essence vide", NotificationManager.Type.Error);
                }

            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
