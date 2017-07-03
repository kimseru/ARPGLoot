using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ARPGLoot.Items
{
    public class Reroll : ModItem
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
            player.GetModPlayer<ARPGPlayer>(mod).clarity = true;
            player.GetModPlayer<ARPGPlayer>(mod).canUse = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of Identify");
            Tooltip.SetDefault("Right click to identify a single item" + "\n" + "Selects top then left items first" + "\n" + "Consumable");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.value = 5000;
            item.rare = 3;
            item.maxStack = 999;
        }
    }
}
