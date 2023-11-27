using Life;
using Life.UI;
using Life.Network;
using MyPoints.Interfaces;
using MyPoints.Panels.PanelsData;
using Newtonsoft.Json;
using System.IO;
using static PointActionManager;
using System.Collections.Generic;
using System.Linq;
using MyPoints.Managers;
using Life.InventorySystem;
using Microsoft.SqlServer.Server;

namespace MyPoints.Components
{
    public class PShop : IPointAction
    {
        public PointActionKeys ActionKeys { get; set; }
        public string Slug { get; set; }
        public List<ShopItem> shopItems { get; set; }
        public PShop()
        {
            ActionKeys = PointActionKeys.Shop;
            shopItems = new List<ShopItem>();
        }

        public void OnPlayerTrigger(Player player)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"{Slug}");

            foreach ((ShopItem shopItem, int index) in shopItems.Select((shopItem, index) => (shopItem, index)))
            {
                panel.AddTabLine($"{shopItem.Item.itemName}", shopItem.Price.ToString("F2") + "€", shopItem.ItemIconId, (ui) => { ui.selectedTab = 0; });
            }

            panel.AddButton("Acheter", (ui) =>
            {
                if (shopItems[ui.selectedTab].Buyable) Buy(player, shopItems[ui.selectedTab]);
                else UIPanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être acheté.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Vendre", (ui) =>
            {
                if (shopItems[ui.selectedTab].Resellable) Sell(player, shopItems[ui.selectedTab]);
                else UIPanelManager.Notification(player, "Indisponible", "Cette objet ne peut pas être acheté.", NotificationManager.Type.Warning);
            });
            panel.AddButton("Fermer", (ui) => UIPanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public void Buy(Player player, ShopItem shopItem)
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
                            if (!player.setup.inventory.AddItem(shopItem.ItemId, amount, "")) //TO DO: Data prop
                            {
                                UIPanelManager.Notification(player, "Erreur", "Vous n'avez pas suffisament de place dans votre inventaire.", NotificationManager.Type.Error);
                            }    
                            else
                            {
                                UIPanelManager.NextPanel(player, ui, () =>
                                {
                                    UIPanelManager.Notification(player, "Achat", $"Vous venez d'acheter:\n{amount} {shopItem.Item.itemName} pour {amount * shopItem.Price}€", NotificationManager.Type.Success);
                                    player.AddMoney(-shopItem.Price * amount, "transaction");
                                    OnPlayerTrigger(player);
                                });
                            }
                        }
                        else UIPanelManager.Notification(player, "Erreur", "Vous n'avez pas les moyens pour acheter cette quantité.", NotificationManager.Type.Error);
                    }
                    else UIPanelManager.Notification(player, "Erreur", "La quantité minimale doit être d'au moins 1.", NotificationManager.Type.Error);
                }
                else UIPanelManager.Notification(player, "Erreur", "Veuillez n'utiliser que des chiffres pour indiquer la quantité.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => UIPanelManager.NextPanel(player, ui, () => OnPlayerTrigger(player)));

            player.ShowPanelUI(panel);
        }

        public void Sell(Player player, ShopItem shopItem)
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
                            UIPanelManager.NextPanel(player, ui, () =>
                            {
                                player.AddMoney(shopItem.Price * itemSelling, "transaction");
                                UIPanelManager.Notification(player, "Vente", $"Vous avez vendu:\n{itemSelling} {shopItem.Item.itemName} pour {itemSelling * shopItem.Price}€", NotificationManager.Type.Success);
                                OnPlayerTrigger(player);
                            });
                           
                        }
                        else UIPanelManager.Notification(player, "Erreur", "Vous ne possédez pas cette quantité dans votre inventaire.", NotificationManager.Type.Error);
                    }
                    else UIPanelManager.Notification(player, "Erreur", "La quantité minimale doit être d'au moins 1.", NotificationManager.Type.Error);
                }
                else UIPanelManager.Notification(player, "Erreur", "Veuillez n'utiliser que des chiffres pour indiquer la quantité.", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => UIPanelManager.NextPanel(player, ui, () => OnPlayerTrigger(player)));

            player.ShowPanelUI(panel);
        }

        public void CreateData(Player player)
        {
            PShop pShop = new PShop();
            ShopDataPanels.ShopCreationInstructions(player, pShop);
        }
        public void UpdateProps(string json)
        {
            JsonConvert.PopulateObject(json, this);
        }

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                filePath = Path.Combine(Main.dataPath + "/" + PointActionKeys.Shop, $"shop_{Slug}_{number}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public object Clone()
        {
            return new PShop();
        }
    }
}
