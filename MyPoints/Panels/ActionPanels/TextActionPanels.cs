using Life.Network;
using Life.UI;
using MyPoints.Components.TextPoint;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class TextActionPanels
    {
        public static void ShowTextPanel(Player player, PText pText)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pText.GetSlug()}");

            panel.text = $"{pText.Content}";

            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
