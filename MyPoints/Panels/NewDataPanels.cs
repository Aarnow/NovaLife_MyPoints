

using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components;
using MyPoints.DTO;
using MyPoints.Interfaces;
using MyPoints.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using static PointActionManager;

namespace MyPoints.Panels
{
    abstract class NewDataPanels
    {
        public static void Action(Player player)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Sélectionner une action pour vos données");

            foreach ((KeyValuePair<PointActionKeys, IPointAction> pair, int index) in Actions.Select((pair, key) => (pair, key)))
                panel.AddTabLine($"{pair.Key}", (ui) => ui.selectedTab = index);

            panel.AddButton("Sélectionner", (ui) =>
            {
                UIPanelManager.NextPanel(player, ui, () =>
                {
                    if (Enum.IsDefined(typeof(PointActionKeys), ui.lines[ui.selectedTab].name))
                    {
                        PointActionKeys actionKey = (PointActionKeys)Enum.Parse(typeof(PointActionKeys), ui.lines[ui.selectedTab].name);
                        Actions[actionKey].CreateData(player);
                    }
                    else UIPanelManager.Notification(player, "Erreur", "Constructeur introuvable pour ce type de données", NotificationManager.Type.Error);
                });
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

       
    }
}
