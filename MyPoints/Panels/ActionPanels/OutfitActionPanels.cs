using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyPoints.Components.OutfitPoint;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class OutfitActionPanels
    {
        public static void OpenOutfitList(Player player, POutfit pOutfit)
        {
            List<Outfit> outfits = pOutfit.Outfits.Where(o => o.SexId == player.character.SexId).ToList();

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"{pOutfit.Slug}");

            foreach((Outfit outfit, int index) in outfits.Select((outfit, index) => (outfit, index)))
            {
                panel.AddTabLine($"{outfit.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Equiper", (ui) =>
            {   
                if(outfits[ui.selectedTab] != null)
                {
                    pOutfit.EquipOutfit(player, outfits[ui.selectedTab]);
                    PanelManager.Notification(player, $"Succès", $"Vous venez d'équiper une tenue de {outfits[ui.selectedTab].Name}", Life.NotificationManager.Type.Success);
                } else PanelManager.Notification(player, $"Erreur", $"Il n'y a aucune tenue à équiper", Life.NotificationManager.Type.Success);
            });
            panel.AddButton("Retirer", (ui) =>
            {
                CharacterCustomizationSetup skin = JsonConvert.DeserializeObject<CharacterCustomizationSetup>(player.character.Skin);
                player.setup.RpcSkinChange(skin);
                PanelManager.Notification(player, $"Succès", $"Vous venez de retrouver vos vêtements d'origine", Life.NotificationManager.Type.Success);
            });
            panel.AddButton("Service", (ui) =>
            {
                player.serviceMetier = !player.serviceMetier;
                PanelManager.Notification(player, $"{(player.serviceMetier ? "Service activé" : "Service désactivé")}", $"Vous venez de {(player.serviceMetier ? "débuter" : "terminer")} votre service", Life.NotificationManager.Type.Success);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
