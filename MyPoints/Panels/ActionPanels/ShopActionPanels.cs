using Life;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyPoints.Components.ShopPoint;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.ActionPanels
{
    abstract class ShopActionPanels
    {
        public static void OpenShop(Player player, PShop pShop)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"{pShop.Slug}");

            foreach ((ShopItem shopItem, int index) in pShop.ShopItems.Select((shopItem, index) => (shopItem, index)))
            {
                panel.AddTabLine($"{shopItem.Item.itemName}", shopItem.Price.ToString("F2") + "€", shopItem.ItemIconId, (ui) => { ui.selectedTab = 0; });
            }

            panel.AddButton("Acheter", (ui) =>
            {
                if (pShop.ShopItems[ui.selectedTab].Buyable) Buy(player, pShop, pShop.ShopItems[ui.selectedTab]);
                else PanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être acheté.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Vendre", (ui) =>
            {
                if (pShop.ShopItems[ui.selectedTab].Resellable) Sell(player, pShop, pShop.ShopItems[ui.selectedTab]);
                else PanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être vendu.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void Buy(Player player, PShop pShop, ShopItem shopItem)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Acheter {shopItem.Item.itemName}");

            panel.inputPlaceholder = "Définir la quantité";

            panel.AddButton("Confirmer l'achat", (ui) =>
            {
                if (int.TryParse(ui.inputText, out int amount))
                {
                    if (amount > 0)
                    {
                        if (player.character.Money >= shopItem.Price * amount)
                        {
                            if (!player.setup.inventory.AddItem(shopItem.ItemId, amount, shopItem.Data))
                            {
                                PanelManager.Notification(player, "Erreur", "Vous n'avez pas suffisament de place dans votre inventaire.", NotificationManager.Type.Error);
                            }
                            else
                            {
                                PanelManager.NextPanel(player, ui, () =>
                                {
                                    PanelManager.Notification(player, "Achat", $"Vous venez d'acheter:\n{amount} {shopItem.Item.itemName} pour {amount * shopItem.Price}€", NotificationManager.Type.Success);
                                    player.AddMoney(-shopItem.Price * amount, "transaction");
                                    pShop.OnPlayerTrigger(player);
                                });
                            }
                        }
                        else PanelManager.Notification(player, "Erreur", "Vous n'avez pas les moyens pour acheter cette quantité.", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "La quantité minimale doit être d'au moins 1.", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Veuillez n'utiliser que des chiffres pour indiquer la quantité.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => pShop.OnPlayerTrigger(player)));

            player.ShowPanelUI(panel);
        }
        public static void Sell(Player player, PShop pShop, ShopItem shopItem)
        {
            int itemCount = 0;
            for (int i = 0; i < 12; i++) if (player.setup.inventory.items[i].itemId == shopItem.ItemId) itemCount += player.setup.inventory.items[i].number;
            int itemSelling = 0;

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Vendre {shopItem.Item.itemName}");

            panel.inputPlaceholder = "Définir la quantité";

            panel.AddButton("Confirmer la vente", (ui) =>
            {
                if (int.TryParse(ui.inputText, out int amount))
                {
                    if (amount > 0)
                    {
                        if (itemCount != 0)
                        {
                            foreach ((ItemInventory value, int index) in player.setup.inventory.items.Select((value, index) => (value, index)))
                            {
                                if (value.itemId == shopItem.ItemId)
                                {
                                    int quantityToRemove = ((amount - itemSelling) > value.number) ? value.number : (amount - itemSelling);
                                    itemCount -= quantityToRemove;
                                    player.setup.inventory.RemoveItem(shopItem.ItemId, quantityToRemove, false);
                                    itemSelling += quantityToRemove;
                                }
                                if (itemCount == 0 || amount == itemSelling) break;
                            }
                            PanelManager.NextPanel(player, ui, () =>
                            {
                                player.AddMoney(shopItem.Price * itemSelling, "transaction");
                                PanelManager.Notification(player, "Vente", $"Vous avez vendu:\n{itemSelling} {shopItem.Item.itemName} pour {itemSelling * shopItem.Price}€", NotificationManager.Type.Success);
                                pShop.OnPlayerTrigger(player);
                            });

                        }
                        else PanelManager.Notification(player, "Erreur", "Vous ne possédez pas cette quantité dans votre inventaire.", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "La quantité minimale doit être d'au moins 1.", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Veuillez n'utiliser que des chiffres pour indiquer la quantité.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => pShop.OnPlayerTrigger(player)));

            player.ShowPanelUI(panel);
        }
    }
}
