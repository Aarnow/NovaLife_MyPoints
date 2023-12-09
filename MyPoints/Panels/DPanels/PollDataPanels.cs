using Life;
using Life.DB;
using Life.Network;
using Life.UI;
using MyPoints.Components.PollPoint;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.DPanels
{
    abstract class PollDataPanels
    {
        public static void SetChoicesList(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Liste des choix");

            foreach ((string choice, int index) in pPoll.Choices.Select((choice, index) => (choice, index)))
            {
                panel.AddTabLine($"{choice}", ui => ui.selectedTab = index);
            }
        
            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player,ui, ()=> AddChoice(player, pPoll)));
            panel.AddButton("Supprimer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pPoll.Choices.RemoveAt(ui.selectedTab); 
                SetChoicesList(player, pPoll);
            }));
            panel.AddButton("Confirmer", (ui) => PanelManager.NextPanel(player, ui, ()=> SetIsSingleVote(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void AddChoice(Player player, PPoll pPoll)
        {
            int max = 210;
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Ajouter un choix");

            panel.inputPlaceholder = "Inscrivez un choix (caractères 210 cractères)";

            panel.AddButton("Confirmer", (ui) =>
            {
                if(ui.inputText.Length > 0)
                {
                    if (ui.inputText.Length <= max)
                    {
                        PanelManager.NextPanel(player, ui, () =>
                        {
                            pPoll.Choices.Add(ui.inputText);
                            SetChoicesList(player, pPoll);
                        });
                    } else PanelManager.Notification(player, "Erreur", $"Votre texte est trop long ({max} caractères maximum)", NotificationManager.Type.Error);
                } else PanelManager.Notification(player, "Erreur", "Vous devez inscrire un choix", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetChoicesList(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetIsSingleVote(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Type de sondage");

            panel.text = "Est ce un sondage à choix unique ou multiple ?";

            panel.AddButton("Unique", (ui) => PanelManager.NextPanel(player, ui, ()=>
            {
                pPoll.IsSingleVote = true;
                SetIsInProgress(player, pPoll);
            }));
            panel.AddButton("Multiple", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pPoll.IsSingleVote = false;
                SetIsInProgress(player, pPoll);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetChoicesList(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetIsInProgress(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Activer le sondage");

            panel.text = "Voulez-vous que votre sondage soit actif dès la création du point ?";

            panel.AddButton("Oui", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pPoll.IsInProgress = true;
                SetPollSlug(player, pPoll);
            }));
            panel.AddButton("Non", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pPoll.IsInProgress = false;
                SetBizOwner(player, pPoll);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetIsSingleVote(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetBizOwner(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Définir la société propriétaire");

            foreach ((Bizs biz, int index) in Nova.biz.bizs.Select((value, index) => (value, index)))
                panel.AddTabLine($"{biz.BizName}", (ui) => ui.selectedTab = index);

            panel.AddButton("Confirmer", (ui) =>
            {
                if (Nova.biz.bizs[ui.selectedTab].Id != 0)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        pPoll.BizOwner = Nova.biz.bizs[ui.selectedTab].Id;
                        SetPollSlug(player, pPoll);
                    });
                } else PanelManager.Notification(player, "Erreur", "Vous devez sélectionner une société", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetIsInProgress(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetPollSlug(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Nommer votre sondage");

            panel.inputPlaceholder = "Donner un nom à votre sondage";

            panel.AddButton("Sauvegarder", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pPoll.Slug = ui.inputText;
                    pPoll.Save();
                    PanelManager.Notification(player, "Succès", "Les données de votre sondage ont bien été sauvegardées.", NotificationManager.Type.Success);
                });
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetBizOwner(player, pPoll)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
