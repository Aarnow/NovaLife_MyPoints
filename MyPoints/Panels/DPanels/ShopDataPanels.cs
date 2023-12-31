﻿using Life;
using Life.Network;
using Life.UI;
using MyPoints.Components.ShopPoint;
using System;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.DPanels
{
    abstract class ShopDataPanels
    {
        public static void ShopCreationInstructions(Player player, PShop pShop)
        {           
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Création d'une boutique");

            panel.text = $"\nRendez-vous sur le wiki officiel pour obtenir les ID des objets que vous souhaitez intégrer à votre boutique.\n\n<color={PanelManager.Colors[NotificationManager.Type.Warning]}>https://sites.google.com/view/nova-life-wiki/aides/</color>";

            panel.AddButton("Commencer", (ui) => PanelManager.NextPanel(player, ui, () => SetItemList(player, pShop)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetItemList(Player player, PShop pShop)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.TabPrice).SetTitle($"Aperçu de votre boutique");

            foreach((ShopItem shopItem, int index) in pShop.ShopItems.Select((shopItem, index)=>(shopItem, index)))
            {
                panel.AddTabLine($"{shopItem.Item.itemName}",shopItem.Price.ToString("F2") + "€",shopItem.ItemIconId, (ui) =>{ui.selectedTab = index;});
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () => AddItem(player, pShop)));
            panel.AddButton("Supprimer", (ui) => PanelManager.NextPanel(player, ui, ()=>
            {
                pShop.ShopItems.RemoveAt(ui.selectedTab);
                SetItemList(player, pShop);
            }));
            panel.AddButton("Valider", (ui) => PanelManager.NextPanel(player, ui, () => SetSlug(player, pShop)));
            panel.AddButton("Quitter", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void AddItem(Player player, PShop pShop, bool buyable = true, bool resellable = true)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Ajouter un objet");

            panel.inputPlaceholder = "ID de l'objet [ESPACE] Prix de l'objet";

            panel.AddButton($"Achetable: {(buyable ? $"<color={PanelManager.Colors[NotificationManager.Type.Success]}>OUI</color>" : $"<color={PanelManager.Colors[NotificationManager.Type.Error]}>NON</color>")}", (ui) => PanelManager.NextPanel(player, ui, () => AddItem(player, pShop, !buyable, resellable)));
            panel.AddButton($"Vendable: {(resellable ? $"<color={PanelManager.Colors[NotificationManager.Type.Success]}>OUI</color>" : $"<color={PanelManager.Colors[NotificationManager.Type.Error]}>NON</color>")}", (ui) => PanelManager.NextPanel(player, ui, () => AddItem(player, pShop, buyable, !resellable)));
            panel.AddButton("Confirmer", (ui) =>
            {
                string[] inputText = ui.inputText.Split(' ');
                if (inputText.Length == 2)
                {
                    if (int.TryParse(inputText[0], out int itemId))
                    {
                        if (LifeManager.instance.item.GetItem(itemId) != null)
                        {
                            inputText[1] = inputText[1].Replace(',', '.');
                            if (double.TryParse(inputText[1], out double itemPrice) && itemPrice >= 0)
                            {
                                PanelManager.NextPanel(player, ui, () =>
                                {
                                    itemPrice = Math.Ceiling(itemPrice * 100) / 100;
                                    ShopItem shopItem = new ShopItem(itemPrice, itemId, buyable, resellable);
                                    pShop.ShopItems.Add(shopItem);
                                    SetItemList(player, pShop);
                                });
                            }
                            else PanelManager.Notification(player, "Erreur", "Vous devez définir un prix au bon format et positif.", NotificationManager.Type.Error);
                        }
                        else PanelManager.Notification(player, "Erreur", $"Nous n'avons aucun objet correspondant à l'ID: {itemId}", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Vous devez indiquer l'identifiant de l'objet.\nID [ESPACE] PRIX", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez respecter le format et ne fournir que l'ID et le prix.\nID [ESPACE] PRIX", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetItemList(player, pShop)));

            player.ShowPanelUI(panel);
        }

        public static void SetSlug(Player player, PShop pShop)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Confirmer votre boutique");

            panel.inputPlaceholder = "Quel est le nom de votre boutique ?";

            panel.AddButton("Confirmer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pShop.Slug = ui.inputText;
                pShop.Save();
                PanelManager.Notification(player, "Succès", "Les données de votre boutique ont bien été sauvegardées.", NotificationManager.Type.Success);
            }));
            panel.AddButton("Quitter", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
