using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.TextPoint;
using MyPoints.Managers;

namespace MyPoints.Panels.PanelsData
{
    abstract class TextDataPanels
    {
        public static void SetTextSlug(Player player, PText pText)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Titre de votre panel");

            panel.inputPlaceholder = "Donner un titre à votre panel";

            panel.AddButton("Confirmer", (ui) =>
            {
                UIPanelManager.NextPanel(player, ui, () =>
                {
                    if (ui.inputText.Length > 0)
                    {
                        pText.Slug = ui.inputText;
                        SetTextContent(player, pText);
                    }
                    else UIPanelManager.Notification(player, "Erreur", "Veuillez donner un titre à votre panel", NotificationManager.Type.Error);
                });
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetTextContent(Player player, PText pText)
        {
            int max = 320;
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Texte de votre panel");

            panel.inputPlaceholder = $"Ecrivez votre texte ({max} caractères maximum)";

            panel.AddButton("Confirmer", (ui) =>
            {
                if (ui.inputText.Length > 0 && ui.inputText.Length <= max)
                {
                    UIPanelManager.NextPanel(player, ui, () =>
                    {
                        pText.Content = ui.inputText;
                        pText.Save();
                        UIPanelManager.Notification(player, "Succès", "Les données de votre panel textuel ont bien été sauvegardées.", NotificationManager.Type.Success);
                    });
                }
                else UIPanelManager.Notification(player, "Erreur", $"Votre texte ne respecte pas la limite de caractères. ({ui.inputText.Length}/{max})", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => UIPanelManager.NextPanel(player, ui, ()=> SetTextSlug(player, pText)));
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
