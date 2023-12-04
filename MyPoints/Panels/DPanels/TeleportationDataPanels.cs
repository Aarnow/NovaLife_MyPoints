using Life.UI;
using Life.Network;
using Life;
using MyPoints.Components.TeleportationPoint;
using UIPanelManager;

namespace MyPoints.Panels.DPanels
{
    abstract class TeleportationDataPanels
    {
        public static void SetTeleportationSlug(Player player, PTeleportation pTeleportation)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Nom de la destination");

            panel.inputPlaceholder = "Donner un nom à votre destination (ex: Mairie)";

            panel.AddButton("Confirmer", (ui) =>
            {
                if (ui.inputText.Length != 0)
                {
                    pTeleportation.Slug = ui.inputText;
                    PanelManager.NextPanel(player, ui, () => SetTeleportationCoordinates(player, pTeleportation));
                } else PanelManager.Notification(player, "Erreur", "Veuillez nommer votre destination.", NotificationManager.Type.Error);
                
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetTeleportationCoordinates(Player player, PTeleportation pTeleportation)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Coordonnées de la destination");

            panel.text = "En confirmant, votre position actuelle correspondra aux coordonnées de destination.";

            panel.AddButton("Confirmer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pTeleportation.SetPositionAxis(player.setup.transform.position);
                    pTeleportation.Save();
                    PanelManager.Notification(player, "Succès", "Les données de votre téléporteur ont bien été sauvegardées.", NotificationManager.Type.Success);
                });              
            });
            panel.AddButton("Retour", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () => SetTeleportationSlug(player, pTeleportation));
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
