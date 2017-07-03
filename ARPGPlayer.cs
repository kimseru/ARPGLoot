using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;

namespace ARPGLoot
{
    public class ARPGPlayer : ModPlayer
    {
        public int immuneDamage1 = 0;
        public int immuneDamage2 = 0;
        public int immuneDamage3 = 0;

        public int maxDamageTaken1 = int.MaxValue;
        public int maxDamageTaken2 = int.MaxValue;
        public int maxDamageTaken3 = int.MaxValue;

        public string onHit = "";
        public int onHitChance = 0;

        public int potionReduce = 0;
        public int iFrames = 0;

        public ARPGItem ir = null;

        public bool clarity = false;
        public bool clarity2 = false;
        public bool canUse = false;

        public int seedPlus = 0;

        public override void PreUpdate()
        {
            canUse = false;
            if (onHit.Length > 0)
            {
                onHit = "";
            }
            if (player.pStone && potionReduce != 0)
            {
                player.potionDelayTime = (45 * 60) - (potionReduce * 60);
            }
            else if (potionReduce != 0)
            {
                player.potionDelayTime = (60 * 60) - (potionReduce * 60);
            }
            potionReduce = 0;
        }

        public override void PostUpdate()
        {
            clarity2 = false;
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Random rand = new Random();
            if (rand.Next(0, 100) + 1 <= onHitChance)
            {
                if (onHit.Equals("Fire"))
                    target.AddBuff(BuffID.OnFire, 5 * 60);
                if (onHit.Equals("Frostburn"))
                    target.AddBuff(BuffID.Frostburn, 5 * 60);
                if (onHit.Equals("Curse"))
                    target.AddBuff(BuffID.CursedInferno, 5 * 60);
                if (onHit.Equals("Leech") && target.type != 488 && player.statLife <= player.statLifeMax2)
                {
                    player.HealEffect(3, true);
                    player.statLife += 3;
                }
            }
            onHit = "";
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            Random rand = new Random();
            if (rand.Next(0, 100) + 1 <= onHitChance)
            {
                if (onHit.Equals("Fire"))
                    target.AddBuff(BuffID.OnFire, 5 * 60);
                if (onHit.Equals("Frostburn"))
                    target.AddBuff(BuffID.Frostburn, 5 * 60);
                if (onHit.Equals("Curse"))
                    target.AddBuff(BuffID.CursedInferno, 5 * 60);
                if (onHit.Equals("Leech") && target.type != 488 && player.statLife <= player.statLifeMax2)
                {
                    player.HealEffect(10, true);
                    player.statLife += 10;
                }
            }
            onHit = "";
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (player.longInvince && player.immuneTime >= 60)
                player.immuneTime += iFrames;
            else if (player.immuneTime >= 30)
                player.immuneTime += iFrames;
            iFrames = 0;

            int maxDamageTaken;
            if (maxDamageTaken1 < maxDamageTaken2 && maxDamageTaken1 < maxDamageTaken3)
            {
                maxDamageTaken = maxDamageTaken1;
            }
            else if (maxDamageTaken2 < maxDamageTaken1 && maxDamageTaken2 < maxDamageTaken3)
            {
                maxDamageTaken = maxDamageTaken2;
            }
            else
            {
                maxDamageTaken = maxDamageTaken3;
            }
            maxDamageTaken1 = int.MaxValue;
            maxDamageTaken2 = int.MaxValue;
            maxDamageTaken3 = int.MaxValue;

            double defMod = 0.5;
            if (Main.expertMode)
                defMod = 0.75;

            double finalDamage = (damage - (player.statDefense * defMod)) * (1 + player.endurance);
            if (finalDamage > maxDamageTaken)
            {
                damage = maxDamageTaken;
            }

            int immuneDamage = immuneDamage1 + immuneDamage2 + immuneDamage3;
            if (immuneDamage > finalDamage)
            {
                damage = 0;
                immuneDamage1 = 0;
                immuneDamage2 = 0;
                immuneDamage3 = 0;
                return false;
            }
            return true;
        }
    }
}