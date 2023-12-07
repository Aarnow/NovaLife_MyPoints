using Life;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyPoints.Components.ShopPoint;
using MyPoints.Components.TrashPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class TrashActionPanels
    {
        public static void OpenTrash(Player player, PTrash pTrash)
        {
            Dictionary<int, int> playerInventory = new Dictionary<int, int>();

            for (int i = 0; i < 12; i++)
            {
                if (playerInventory.ContainsKey(player.setup.inventory.items[i].itemId))
                {
                    playerInventory[player.setup.inventory.items[i].itemId] += player.setup.inventory.items[i].number;                     
                }
                else if(player.setup.inventory.items[i].itemId != 0) playerInventory.Add(player.setup.inventory.items[i].itemId, player.setup.inventory.items[i].number);
            }

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"{pTrash.GetSlug()}");

            foreach ((var item, int index) in playerInventory.Select((item, index) => (item, index)))
            {
                Item currentItem = LifeManager.instance.item.GetItem(item.Key);
                panel.AddTabLine($"{currentItem.itemName}", $"Quantité: {item.Value}", ShopItem.getIconId(currentItem.id), ui => ui.selectedTab = index);
            }

            panel.AddButton("Sélectionner", (ui) =>
            {
                List<int> keysList = playerInventory.Keys.ToList();
                Item currentItem = LifeManager.instance.item.GetItem(keysList[ui.selectedTab]);
                
                if (currentItem != null)
                {
                    PanelManager.NextPanel(player, ui, () =>
                    {
                        int currentQty = playerInventory[currentItem.id];
                        Throw(player, pTrash, currentItem, currentQty);
                    }); 
                }
                else PanelManager.Notification(player, "Erreur", "Nous n'avons pas retrouvé l'objet indiqué.", NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void Throw(Player player, PTrash pTrash, Item item, int quantity)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"{pTrash.GetSlug()}");

            panel.inputPlaceholder = $"Définir la quantité à jeter (max: {quantity})";

            panel.AddButton("Jeter", (ui) =>
            {
                if (int.TryParse(ui.inputText, out int amount))
                {
                    if (amount > 0)
                    {
                        quantity = amount > quantity ? quantity : amount;
                        PanelManager.NextPanel(player, ui, () => ConfirmThrowing(player, pTrash, item, quantity));
                    }
                    else PanelManager.Notification(player, "Erreur", "La quantité minimale doit être d'au moins 1.", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Veuillez n'utiliser que des chiffres pour indiquer la quantité.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => OpenTrash(player, pTrash)));

            player.ShowPanelUI(panel);
        }

        public static void ConfirmThrowing(Player player, PTrash pTrash, Item item, int quantity)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"{pTrash.GetSlug()}");

            panel.text = $"Voulez-vous vraiment jeter {quantity} {item.itemName} ?";

            panel.AddButton("Confirmer", (ui) =>
            {
                int itemToThrow = quantity;

                foreach ((ItemInventory value, int index) in player.setup.inventory.items.Select((value, index) => (value, index)))
                {
                    if (value.itemId == item.id)
                    {
                        int quantityToRemove = itemToThrow >= value.number ? value.number : itemToThrow;
                        itemToThrow -= quantityToRemove;
                        player.setup.inventory.RemoveItem(item.id, quantityToRemove, false);
                    }
                    if (itemToThrow == 0) break;
                }

                PanelManager.NextPanel(player, ui, () => OpenTrash(player, pTrash));
                PanelManager.Notification(player, "Succès", $"Vous avez jeté {quantity} {item.itemName}", NotificationManager.Type.Success);
            });
            panel.AddButton("Annuler", (ui) => PanelManager.NextPanel(player, ui, () => OpenTrash(player, pTrash)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
