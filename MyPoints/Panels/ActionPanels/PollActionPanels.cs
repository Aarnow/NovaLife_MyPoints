using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.FuelPoint;
using MyPoints.Components.PollPoint;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class PollActionPanels
    {
        public static void ShowPoll(Player player, PPoll pPoll, List<int> choices)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Tab).SetTitle($"{pPoll.GetSlug()}");

            foreach ((string choice, int index) in pPoll.Choices.Select((choice, index) => (choice, index)))
            {
                panel.AddTabLine($"{(choices.Contains(index) ? $"<color={PanelManager.Colors[NotificationManager.Type.Success]}>{choice}</color>" : $"{choice}")}", ui => ui.selectedTab = index);
            }
            panel.AddButton("Voter", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    if (choices.Contains(ui.selectedTab)) choices.Remove(ui.selectedTab);
                    else if (pPoll.IsSingleVote)
                    {
                        choices.Clear();
                        choices.Add(ui.selectedTab);
                    }
                    else choices.Add(ui.selectedTab);
                    ShowPoll(player, pPoll, choices);
                });
            });
            panel.AddButton("Confirmer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    pPoll.UpdateProps();
                    pPoll.characterChoices.Add(player.character.Id, choices);
                    pPoll.UpdateData();
                    WaitPollResult(player, pPoll);
                });
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void WaitPollResult(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pPoll.GetSlug()}");

            panel.text = "Votre vote a bien été enregistré.\nEn attente des résultats.\nLes résultats seront disponibles sur ce point dès que son propriétaire aura clôturé les votes.";

            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void PollConfig(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pPoll.GetSlug()}");

            panel.text = "Config";

            if(!pPoll.IsInProgress && pPoll.characterChoices.Count == 0 || pPoll.IsInProgress)
            {
                panel.AddButton($"{(pPoll.IsInProgress ? "Clôturer" : "Activer")}", (ui) =>
                {
                    pPoll.UpdateProps();
                    pPoll.IsInProgress = !pPoll.IsInProgress;
                    pPoll.UpdateData();
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        if (pPoll.IsInProgress) PollConfig(player, pPoll);
                        else
                        {
                            ShowPollResult(player, pPoll);
                        }
                    });
                });
            }

            panel.AddButton($"{(pPoll.IsInProgress ? "Voter" : "Résultats")}", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                if(pPoll.IsInProgress && !pPoll.characterChoices.ContainsKey(player.character.Id)) ShowPoll(player, pPoll, new List<int>());
                else ShowPollResult(player, pPoll);
            }));           
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ShowPollResult(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Tab).SetTitle($"{pPoll.GetSlug()}");

            foreach ((string choice, int index) in pPoll.Choices.Select((choice, index) => (choice, index)))
            {
                panel.AddTabLine($"{choice} - ({pPoll.characterChoices.Count(vote => vote.Value.Contains(index))} votes)", ui => ui.selectedTab = index);
            }

            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ShowPollClosed(Player player, PPoll pPoll)
        {
            UIPanel panel = new UIPanel("MyPoints Panel", UIPanel.PanelType.Text).SetTitle($"{pPoll.GetSlug()}");

            panel.text = "Ce sondage n'est pas encore ouvert.";

            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
