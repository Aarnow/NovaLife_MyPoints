using Life.Network;
using MyPoints.Common;
using MyPoints.Panels.ActionPanels;
using MyPoints.Panels.DPanels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static PointActionManager;

namespace MyPoints.Components.OutfitPoint
{
    public class POutfit : PointAction
    {
        public override PointActionKeys ActionKeys { get; set; }
        public override string Slug { get; set; }

        public List<Outfit> Outfits = new List<Outfit>();

        public POutfit():base(PointActionKeys.Outfit, "default_job")
        {
        }

        public override void OnPlayerTrigger(Player player)
        {
            OutfitActionPanels.OpenOutfitList(player, this);
        }

        public override void CreateData(Player player)
        {
            POutfit pOutfit = new POutfit();
            OutfitDataPanels.SetOutfitList(player, pOutfit);
        }

        public override void UpdateProps()
        {
            JsonConvert.PopulateObject(File.ReadAllText(Main.dataPath + "/" + ActionKeys + "/" + $"{ActionKeys}_{Slug}.json"), this);
        }

        public override object Clone()
        {
            return new POutfit();
        }

        public void EquipOutfit(Player player, Outfit outfit)
        {
            if (outfit.Hat != null) player.setup.characterSkinData.Hat = outfit.Hat.ClothId;
            if (outfit.Accessory != null) player.setup.characterSkinData.Accessory = outfit.Accessory.ClothId;
            if (outfit.Shirt != null) player.setup.characterSkinData.TShirt = outfit.Shirt.ClothId;
            if (outfit.Pants != null) player.setup.characterSkinData.Pants = outfit.Pants.ClothId;
            if (outfit.Shoes != null) player.setup.characterSkinData.Shoes = outfit.Shoes.ClothId;

            player.setup.RpcSkinChange(player.setup.characterSkinData);
        }

        public void UnequipOutfit(Player player, string Skin)
        {
            CharacterCustomizationSetup skin = JsonConvert.DeserializeObject<CharacterCustomizationSetup>(player.character.Skin);

            player.setup.characterSkinData.Hat = skin.Hat;
            player.setup.characterSkinData.Accessory = skin.Accessory;
            player.setup.characterSkinData.TShirt = skin.TShirt;
            player.setup.characterSkinData.Pants = skin.Pants;
            player.setup.characterSkinData.Shoes = skin.Shoes;

            player.setup.RpcSkinChange(player.setup.characterSkinData);
        }
    }
}
