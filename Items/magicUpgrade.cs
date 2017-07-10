using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ARPGLoot.Items
{
    public class magicUpgrade : ModItem
    {
        public override bool CanRightClick()
        {
            if (Main.LocalPlayer.GetModPlayer<ARPGPlayer>(mod).canMagicUpgrade)
                return true;
            return false;
        }

        public override void RightClick(Player player)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/legendary").WithVolume(1f));
            player.GetModPlayer<ARPGPlayer>(mod).magicClarity = true;
            player.GetModPlayer<ARPGPlayer>(mod).canMagicUpgrade = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Orb of Alchemy");
            Tooltip.SetDefault("Right click to upgrade a single magical item to rare quality" + "\n" + "Selects top left items first" + "\n" + "Consumable");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(1332, 1);  //soul of night
            recipe.AddIngredient(521, 3);   //cursed flames
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(1332, 1); 
            recipe.AddIngredient(522, 3);   //ichor
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.value = 50000;
            item.rare = 6;
            item.maxStack = 999;
        }
    }
}
