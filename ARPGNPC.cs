using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ARPGLoot
{
    public class ARPGNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void NPCLoot(NPC npc)
        {
            Random rand = new Random();
            if (npc.lifeMax > 5 && npc.value > 0f)
            {
                if (rand.Next(0, 20) == 0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Reroll"));
                
            }
            if (Main.hardMode && npc.lifeMax > 200 && npc.value > 0f)
            {
                if (rand.Next(0, 50) == 0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("magicUpgrade"));
            }
            if (NPC.downedPlantBoss && npc.lifeMax > 500 && npc.value > 0f)
            {
                if (rand.Next(0, 100) == 0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("itemReroll"));
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Merchant)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType<Items.Reroll>());
                shop.item[nextSlot].shopCustomPrice = new int?(5000);
                nextSlot++;
            }
        }
    }
}
