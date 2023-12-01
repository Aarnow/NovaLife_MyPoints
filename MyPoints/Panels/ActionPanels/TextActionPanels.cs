using Life.Network;
using Life.UI;
using MyPoints.Components.TextPoint;
using MyPoints.Managers;

namespace MyPoints.Panels.ActionPanels
{
    abstract class TextActionPanels
    {
        public static void ShowTextPanel(Player player, PText pText)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pText.Slug}");

            panel.text = $"{pText.Content}";

            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
