using Life;
using Life.UI;
using Life.Network;
using MyPoints.Managers;

namespace MyPoints.Panels
{
    abstract class MainPanel
    {
        public static void OpenMyPointsMenu(Player player)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"MyPoints Menu");

            panel.AddTabLine("Ajouter un point", (ui) => ui.selectedTab = 0);
            panel.AddTabLine("Supprimer un point", (ui) => ui.selectedTab = 1);
            panel.AddTabLine("Créer un jeu de données", (ui) => ui.selectedTab = 2);

            panel.AddButton("Sélection", (ui) =>
            {
                if (ui.selectedTab == 0) UIPanelManager.NextPanel(player, ui, () => PointPanels.SetAction(player));
                else if (ui.selectedTab == 1) UIPanelManager.NextPanel(player, ui, () => PointPanels.PointList(player));
                else if (ui.selectedTab == 2) UIPanelManager.NextPanel(player, ui, () => DataPanels.Action(player));
                else UIPanelManager.Notification(player, "Erreur", "Vous devez sélectionner un choix", NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
