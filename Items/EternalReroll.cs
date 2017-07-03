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
    public class EternalReroll : ModItem
    {
        public override bool CanRightClick()
        {
            if (Main.LocalPlayer.GetModPlayer<ARPGPlayer>(mod).canUse)
                return true;
            return false;
        }

        public override void RightClick(Player player)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/identify").WithVolume(1f));
            player.GetModPlayer<ARPGPlayer>(mod).clarity2 = true;
            item.stack = 2;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tome of Identify");
            Tooltip.SetDefault("Right click to identify an item" + "\n" + "Selects top then left items first" + "\n" + "Not consumable");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = 5000;
            item.rare = 5;
            item.maxStack = 1;
            item.consumable = false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Reroll", 100);
            recipe.AddIngredient(531);
            recipe.AddTile(101);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
