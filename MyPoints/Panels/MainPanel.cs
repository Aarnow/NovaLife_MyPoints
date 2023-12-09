using Life;
using Life.UI;
using Life.Network;
using UIPanelManager;

namespace MyPoints.Panels
{
    abstract class MainPanel
    {
        public static void OpenMyPointsMenu(Player player)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"MyPoints (v1.0.0)");

            panel.AddTabLine("Ajouter un point", (ui) => ui.selectedTab = 0);
            panel.AddTabLine("Supprimer un point", (ui) => ui.selectedTab = 1);
            panel.AddTabLine("Créer un jeu de données", (ui) => ui.selectedTab = 2);

            panel.AddButton("Sélection", (ui) =>
            {
                if (ui.selectedTab == 0) PanelManager.NextPanel(player, ui, () => PointPanels.SetAction(player));
                else if (ui.selectedTab == 1) PanelManager.NextPanel(player, ui, () => PointPanels.PointList(player));
                else if (ui.selectedTab == 2) PanelManager.NextPanel(player, ui, () => DataPanels.Action(player));
                else PanelManager.Notification(player, "Erreur", "Vous devez sélectionner un choix", NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
