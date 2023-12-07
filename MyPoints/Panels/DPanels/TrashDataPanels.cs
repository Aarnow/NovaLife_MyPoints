using Life.DB;
using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.TrashPoint;
using UIPanelManager;
using System.Linq;

namespace MyPoints.Panels.DPanels
{
    public class TrashDataPanels
    {
        public static void SetTrashAllowedBizs(Player player, PTrash pTrash)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Ajouter les sociétés qui peuvent vider la poubelle");

            foreach ((Bizs biz, int index) in Nova.biz.bizs.Select((value, index) => (value, index)))
                panel.AddTabLine(pTrash.AllowedBizs.Contains(biz.Id) ? $"<color={PanelManager.Colors[NotificationManager.Type.Success]}>{biz.BizName}</color>" : $"{biz.BizName}", (ui) => ui.selectedTab = index);

            panel.AddButton("Ajouter/Retirer", (ui) =>
            {
                PanelManager.NextPanel(player, ui, () =>
                {
                    if (pTrash.AllowedBizs.Contains(Nova.biz.bizs[ui.selectedTab].Id)) pTrash.AllowedBizs.Remove(Nova.biz.bizs[ui.selectedTab].Id);
                    else pTrash.AllowedBizs.Add(Nova.biz.bizs[ui.selectedTab].Id);
                    SetTrashAllowedBizs(player, pTrash);
                });
            });
            panel.AddButton("Confirmer", (ui) => PanelManager.NextPanel(player, ui, () => SetTrashSlug(player, pTrash)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void SetTrashSlug(Player player, PTrash pTrash)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Créer une poubelle");

            panel.inputPlaceholder = "Nommer votre poubelle";

            panel.AddButton("Sauvegarder", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pTrash.Slug = ui.inputText;
                pTrash.Save();
                PanelManager.Notification(player, "Succès", "Les données de votre poubelle ont bien été sauvegardées.", NotificationManager.Type.Success);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetTrashAllowedBizs(player, pTrash)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
