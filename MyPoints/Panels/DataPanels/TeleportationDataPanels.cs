using MyPoints.Components;
using Life.UI;
using Life.Network;
using MyPoints.Managers;
using Life;

namespace MyPoints.Panels.PanelsData
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
                    UIPanelManager.NextPanel(player, ui, () => SetTeleportationCoordinates(player, pTeleportation));
                } else UIPanelManager.Notification(player, "Erreur", "Veuillez nommer votre destination.", NotificationManager.Type.Error);
                
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetTeleportationCoordinates(Player player, PTeleportation pTeleportation)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Coordonnées de la destination");

            panel.text = "En confirmant, votre position actuelle correspondra aux coordonnées de destination.";

            panel.AddButton("Confirmer", (ui) =>
            {
                UIPanelManager.NextPanel(player, ui, () =>
                {
                    pTeleportation.SetPositionAxis(player.setup.transform.position);
                    pTeleportation.Save();
                    UIPanelManager.Notification(player, "Succès", "Les données de votre téléporteur ont bien été sauvegardées.", NotificationManager.Type.Success);
                });              
            });
            panel.AddButton("Retour", (ui) =>
            {
                UIPanelManager.NextPanel(player, ui, () => SetTeleportationSlug(player, pTeleportation));
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
