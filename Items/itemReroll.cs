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
    public class itemReroll : ModItem
    {
        public override bool CanRightClick()
        {
            if (Main.LocalPlayer.GetModPlayer<ARPGPlayer>(mod).canReroll)
                return true;
            return false;
        }

        public override void RightClick(Player player)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/identify").WithVolume(1f));
            player.GetModPlayer<ARPGPlayer>(mod).reroll = true;
            player.GetModPlayer<ARPGPlayer>(mod).canReroll = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Essence of Favorable Fate");
            Tooltip.SetDefault("Right click to reroll a single item" + "\n" + "Selects top left items first" + "\n" + "Consumable");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);

            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.value = 100000;
            item.rare = 8;
            item.maxStack = 999;
        }
    }
}
