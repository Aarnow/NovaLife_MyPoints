using Life.InventorySystem;
using Life.Network;
using Life.UI;
using MyCloths.Components;
using MyPoints.Components.OutfitPoint;
using System.Collections.Generic;
using System.Linq;
using UIPanelManager;

namespace MyPoints.Panels.DPanels
{
    abstract class OutfitDataPanels
    {
        public static void SetOutfitList(Player player,  POutfit pOutfit)
        {
            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Création d'un vestiaire");

            foreach((Outfit outfit, int index) in pOutfit.Outfits.Select((outfit, index) => (outfit, index)))
            {
                panel.AddTabLine($"{outfit.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitSexId(player, pOutfit)));
            panel.AddButton("Sauvegarder", (ui) => PanelManager.NextPanel(player, ui, () => SaveOutfitList(player, pOutfit)));
            panel.AddButton("Supprimer", (ui) => PanelManager.Quit(ui, player));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitSexId(Player player, POutfit pOutfit)
        {
            Outfit outfit = new Outfit();

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Text).SetTitle($"Choisissez un genre");

            panel.text = "Pour quel genre est destiné votre tenue ?";

            panel.AddButton("Homme", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.SexId = 0;
                SetOutfitHat(player, pOutfit, outfit);
            }));
            panel.AddButton("Femme", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.SexId = 1;
                SetOutfitHat(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitList(player, pOutfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitHat(Player player, POutfit pOutfit, Outfit outfit)
        {
            List<PCloth> pClothList = MyCloths.Main.clothList.FilterClothListByTypeAndSex(outfit.SexId, ClothType.Hat);    

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Choisissez un chapeau");

            foreach ((PCloth pCloth, int index) in pClothList.Select((pCloth, index) => (pCloth, index)))
            {
                string nameTag;
                if (pCloth.SexId == outfit.SexId) nameTag = $"{(outfit.SexId == 0 ? $"<color={MyCloths.Main.boyColor}>[H]</color>" : $"<color={MyCloths.Main.girlColor}>[F]</color>")}";
                else nameTag = "";
                panel.AddTabLine($"{nameTag} {pCloth.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Hat = pClothList[ui.selectedTab];
                SetOutfitAccessory(player, pOutfit, outfit);
            }));
            panel.AddButton("Passer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Hat = null;
                SetOutfitAccessory(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitSexId(player, pOutfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitAccessory(Player player, POutfit pOutfit, Outfit outfit)
        {
            List<PCloth> pClothList = MyCloths.Main.clothList.FilterClothListByTypeAndSex(outfit.SexId, ClothType.Accessory);

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Choisissez un accessoire");

            foreach ((PCloth pCloth, int index) in pClothList.Select((pCloth, index) => (pCloth, index)))
            {
                string nameTag;
                if (pCloth.SexId == outfit.SexId) nameTag = $"{(outfit.SexId == 0 ? $"<color={MyCloths.Main.boyColor}>[H]</color>" : $"<color={MyCloths.Main.girlColor}>[F]</color>")}";
                else nameTag = "";
                panel.AddTabLine($"{nameTag} {pCloth.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Accessory = pClothList[ui.selectedTab];
                SetOutfitShirt(player, pOutfit, outfit);
            }));
            panel.AddButton("Passer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Accessory = null;
                SetOutfitShirt(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitHat(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitShirt(Player player, POutfit pOutfit, Outfit outfit)
        {
            List<PCloth> pClothList = MyCloths.Main.clothList.FilterClothListByTypeAndSex(outfit.SexId, ClothType.Shirt);

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Choisissez un haut");

            foreach ((PCloth pCloth, int index) in pClothList.Select((pCloth, index) => (pCloth, index)))
            {
                string nameTag;
                if (pCloth.SexId == outfit.SexId) nameTag = $"{(outfit.SexId == 0 ? $"<color={MyCloths.Main.boyColor}>[H]</color>" : $"<color={MyCloths.Main.girlColor}>[F]</color>")}";
                else nameTag = "";
                panel.AddTabLine($"{nameTag} {pCloth.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Shirt = pClothList[ui.selectedTab];
                SetOutfitPants(player, pOutfit, outfit);
            }));
            panel.AddButton("Passer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Shirt = null;
                SetOutfitPants(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitAccessory(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitPants(Player player, POutfit pOutfit, Outfit outfit)
        {
            List<PCloth> pClothList = MyCloths.Main.clothList.FilterClothListByTypeAndSex(outfit.SexId, ClothType.Pants);

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Choisissez un bas");

            foreach ((PCloth pCloth, int index) in pClothList.Select((pCloth, index) => (pCloth, index)))
            {
                string nameTag;
                if (pCloth.SexId == outfit.SexId) nameTag = $"{(outfit.SexId == 0 ? $"<color={MyCloths.Main.boyColor}>[H]</color>" : $"<color={MyCloths.Main.girlColor}>[F]</color>")}";
                else nameTag = "";
                panel.AddTabLine($"{nameTag} {pCloth.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Pants = pClothList[ui.selectedTab];
                SetOutfitShoes(player, pOutfit, outfit);
            }));
            panel.AddButton("Passer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Pants = null;
                SetOutfitShoes(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitShirt(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitShoes(Player player, POutfit pOutfit, Outfit outfit)
        {
            List<PCloth> pClothList = MyCloths.Main.clothList.FilterClothListByTypeAndSex(outfit.SexId, ClothType.Shoes);

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Choisissez des chaussures");

            foreach ((PCloth pCloth, int index) in pClothList.Select((pCloth, index) => (pCloth, index)))
            {
                string nameTag;
                if (pCloth.SexId == outfit.SexId) nameTag = $"{(outfit.SexId == 0 ? $"<color={MyCloths.Main.boyColor}>[H]</color>" : $"<color={MyCloths.Main.girlColor}>[F]</color>")}";
                else nameTag = "";
                panel.AddTabLine($"{nameTag} {pCloth.Name}", ui => ui.selectedTab = index);
            }

            panel.AddButton("Ajouter", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Shoes = pClothList[ui.selectedTab];
                ShowOutfit(player, pOutfit, outfit);
            }));
            panel.AddButton("Passer", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Shoes = null;
                ShowOutfit(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitPants(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void ShowOutfit(Player player, POutfit pOutfit, Outfit outfit)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Tab).SetTitle($"Valider votre tenue");

            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Genre:</color> {(outfit.SexId == 0 ? "Homme" : "Femme")}", ui => ui.selectedTab = 0);
            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Chapeau:</color> {(outfit.Hat != null ? outfit.Hat.Name : "AUCUN")}", ui => ui.selectedTab = 1);
            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Accessoire:</color> {(outfit.Accessory != null ? outfit.Accessory.Name : "AUCUN")}", ui => ui.selectedTab = 2);
            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Haut:</color> {(outfit.Shirt != null ? outfit.Shirt.Name : "AUCUN")}", ui => ui.selectedTab = 3);
            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Bas:</color> {(outfit.Pants != null ? outfit.Pants.Name : "AUCUN")}", ui => ui.selectedTab = 4);
            panel.AddTabLine($"<color={PanelManager.Colors[Life.NotificationManager.Type.Warning]}>Chaussures:</color> {(outfit.Shoes != null ? outfit.Shoes.Name : "AUCUN")}", ui => ui.selectedTab = 5);

            panel.AddButton("Valider", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                SetOutfitName(player, pOutfit, outfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitShoes(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetOutfitName(Player player, POutfit pOutfit, Outfit outfit)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Nommer votre tenue");

            panel.inputPlaceholder = "Donner un nom à votre tenue";

            panel.AddButton("Valider", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                outfit.Name = ui.inputText;
                pOutfit.Outfits.Add(outfit);
                SetOutfitList(player, pOutfit);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => ShowOutfit(player, pOutfit, outfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SaveOutfitList(Player player, POutfit pOutfit)
        {

            UIPanel panel = new UIPanel("MyPoints Menu", UIPanel.PanelType.Input).SetTitle($"Nommer votre collection de tenues");

            panel.inputPlaceholder = "Quel est le nom de votre collection de tenues";

            panel.AddButton("Sauvegarder", (ui) => PanelManager.NextPanel(player, ui, () =>
            {
                pOutfit.Slug = ui.inputText;
                pOutfit.Save();
                PanelManager.Notification(player, "Succès", "Les données de votre vestiaire ont bien été sauvegardées.", Life.NotificationManager.Type.Success);
            }));
            panel.AddButton("Retour", (ui) => PanelManager.NextPanel(player, ui, () => SetOutfitList(player, pOutfit)));
            panel.AddButton("Fermer", (ui) => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
