using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ARPGLoot
{
    public class ARPGItem : GlobalItem
    {
        private int seedPlus;
        private Player player;

        private Random rand;
        private int randNum;

        public string itemType;
        public string rarity;
        public int rarityValue;
        public int[] modifiers;
        public int[] modifierValues;

        private int baseDamage;
        private int baseCrit;
        private int baseDefense;
        private int baseMana;

        public ARPGItem()
        {
            seedPlus = -1;
            player = Main.LocalPlayer;
            rand = new Random();
            randNum = -1;
            rarity = "";
            itemType = "";
            rarityValue = -1;
            modifiers = null;
            modifierValues = null;
        }

        public override void SetDefaults(Item item)
        {
            if (InstancePerEntity)
                Assign(item);
            baseDamage = item.damage;
            baseCrit = item.crit;
            baseDefense = item.defense;
            baseMana = item.mana;
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            ARPGItem myClone = (ARPGItem)base.Clone(item, itemClone);
            myClone.itemType = itemType;
            myClone.rarity = rarity;
            myClone.rarityValue = rarityValue;
            myClone.modifiers = modifiers;
            myClone.modifierValues = modifierValues;

            myClone.baseDamage = baseDamage;
            myClone.baseCrit = baseCrit;
            myClone.baseDefense = baseDefense;
            myClone.baseMana = baseMana;

            return myClone;
        }

        public void Roll(Item item)
        {
            if (rarityValue > 0)
                return;
            Assign(item);
            if (itemType.Length > 0)
            {
                String soundPath = "";
                randNum = rand.Next(0, 101);
                if (randNum % 100 == 0)
                {
                    rarity = "Primal";
                    if (itemType.Equals("accessory"))
                        rarityValue = 2;
                    else
                        rarityValue = 3;
                    soundPath = "Sounds/primal";
                }
                else if (randNum % 10 == 0)
                {
                    rarity = "Legendary";
                    if (itemType.Equals("accessory"))
                        rarityValue = 2;
                    else
                        rarityValue = 3;
                    soundPath = "Sounds/legendary";
                }
                else if (randNum % 4 == 0)
                {
                    rarity = "Rare";
                    if (itemType.Equals("accessory"))
                        rarityValue = 1;
                    else
                        rarityValue = 2;
                }
                else
                {
                    rarity = "Magical";
                    rarityValue = 1;
                }
                if (soundPath.Length > 0)
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, soundPath).WithVolume(1f));
            }
            else
            {
                return;
            }
            modifiers = new int[rarityValue];
            modifierValues = new int[rarityValue];
            int[] cases = new int[rarityValue];
            if (itemType.Equals("weapon"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    while (true)
                    {
                        Boolean pass = true;

                        randNum = rand.Next(0, 9) + 1;

                        for (int j = 0; j < modifiers.Length; j++)
                        {
                            if (randNum == cases[j])
                                pass = false;
                        }
                        if (pass)
                        {
                            cases[i] = randNum;
                            break;
                        }
                    }
                    int tempValue;
                    if (!rarity.Equals("Primal"))//not a primal
                    {
                        switch (randNum)
                        {
                            case (1)://damage when standing still
                                {
                                    tempValue = rand.Next(4, 10) + 1;
                                    modifiers[i] = 1;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (2)://knockback when running
                                {
                                    tempValue = rand.Next(5 - 1, 25) + 1;
                                    modifiers[i] = 2;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (3):
                                {
                                    if (rand.Next(0, 2) == 0)//damage when below 50% hp
                                    {
                                        tempValue = rand.Next(5 - 1, 15) + 1;
                                        modifiers[i] = 3;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //damage when full hp
                                    {
                                        tempValue = rand.Next(5 - 1, 15) + 1;
                                        modifiers[i] = 4;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (4)://damage when below 50% mp
                                {
                                    tempValue = rand.Next(5 - 1, 10) + 1;
                                    modifiers[i] = 5;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (5)://damage when below certain defense
                                {
                                    if (!Main.hardMode)  //prehardmode
                                    {
                                        tempValue = rand.Next(5 - 1, 15) + 1;
                                        modifiers[i] = 6;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //hardmode
                                    {
                                        tempValue = rand.Next(5 - 1, 15) + 1;
                                        modifiers[i] = 6;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (6)://damage in biome
                                {
                                    int biome = -1;
                                    if (Main.hardMode)
                                        biome = rand.Next(0, 4);
                                    else
                                        biome = rand.Next(0, 3);
                                    if (biome == 0)//jungle
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 7;
                                        modifierValues[i] = tempValue;
                                    }
                                    else if (biome == 1) //evil
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 8;
                                        modifierValues[i] = tempValue;
                                    }
                                    else if (biome == 2) //water
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 9;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //hallow
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 10;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (7)://attack speed during day/night
                                {
                                    if (rand.Next(0, 2) == 0)  //day
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 11;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //night
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 12;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (8)://crit when invulnerable
                                {
                                    tempValue = rand.Next(24, 50) + 1;
                                    modifiers[i] = 13;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (9):
                                {
                                    if (item.magic) //reduced mana cost
                                    {
                                        tempValue = rand.Next(0, 2) + 1;
                                        modifiers[i] = 14;
                                        modifierValues[i] = tempValue;
                                        break;
                                    }
                                    int temp;
                                    if (!Main.hardMode)  //prehardmode
                                        temp = rand.Next(0, 2);
                                    else
                                        temp = rand.Next(0, 4);
                                    if (temp == 0) //chance to deal on fire
                                    {
                                        tempValue = rand.Next(9, 100) + 1;
                                        modifiers[i] = 15;
                                        modifierValues[i] = tempValue;
                                    }
                                    else if (temp == 1) //chance to deal frostburn
                                    {
                                        tempValue = rand.Next(10 - 1, 50) + 1;
                                        modifiers[i] = 16;
                                        modifierValues[i] = tempValue;
                                    }
                                    else if (temp == 2)//chance to deal cursed inferno
                                    {
                                        tempValue = rand.Next(5 - 1, 25) + 1;
                                        modifiers[i] = 17;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //chance to life leech
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 18;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                        }
                    }
                    else//primal
                    {
                        switch (randNum)
                        {
                            case (1)://damage when standing still
                                {
                                    modifiers[i] = 1;
                                    modifierValues[i] = 10;
                                    break;
                                }
                            case (2)://knockback when running
                                {
                                    modifiers[i] = 2;
                                    modifierValues[i] = 25;
                                    break;
                                }
                            case (3):
                                {
                                    if (rand.Next(0, 2) == 0)//damage when below 50% hp
                                    {
                                        modifiers[i] = 3;
                                        modifierValues[i] = 15;
                                    }
                                    else //damage when full hp
                                    {
                                        modifiers[i] = 4;
                                        modifierValues[i] = 15;
                                    }
                                    break;
                                }
                            case (4)://damage when below 50% mp
                                {
                                    modifiers[i] = 5;
                                    modifierValues[i] = 10;
                                    break;
                                }
                            case (5)://damage when below certain defense
                                {
                                    if (!Main.hardMode)  //prehardmode
                                    {
                                        modifiers[i] = 6;
                                        modifierValues[i] = 15;
                                    }
                                    else //hardmode
                                    {
                                        tempValue = rand.Next(5, 15) + 1;
                                        modifiers[i] = 6;
                                        modifierValues[i] = 15;
                                    }
                                    break;
                                }
                            case (6)://damage in biome
                                {
                                    int biome = rand.Next(0, 5);
                                    if (biome == 0)//jungle
                                    {
                                        modifiers[i] = 7;
                                        modifierValues[i] = 10;
                                    }
                                    else if (biome == 1) //evil
                                    {
                                        modifiers[i] = 8;
                                        modifierValues[i] = 10;
                                    }
                                    else if (biome == 2) //hallow
                                    {
                                        modifiers[i] = 9;
                                        modifierValues[i] = 10;
                                    }
                                    else //water
                                    {
                                        modifiers[i] = 10;
                                        modifierValues[i] = 10;
                                    }
                                    break;
                                }
                            case (7)://attack speed during day/night
                                {
                                    if (rand.Next(0, 2) == 0)  //day
                                    {
                                        modifiers[i] = 11;
                                        modifierValues[i] = 10;
                                    }
                                    else //night
                                    {
                                        modifiers[i] = 12;
                                        modifierValues[i] = 10;
                                    }
                                    break;
                                }
                            case (8)://crit when invulnerable
                                {
                                    modifiers[i] = 13;
                                    modifierValues[i] = 50;
                                    break;
                                }
                            case (9):
                                {
                                    if (item.magic) //reduced mana cost
                                    {
                                        modifiers[i] = 14;
                                        modifierValues[i] = 2;
                                        break;
                                    }
                                    int temp;
                                    if (!Main.hardMode)  //prehardmode
                                        temp = rand.Next(0, 2);
                                    else
                                        temp = rand.Next(0, 4);
                                    if (temp == 0) //chance to deal on fire
                                    {
                                        modifiers[i] = 15;
                                        modifierValues[i] = 100;
                                    }
                                    else if (temp == 1) //chance to deal frostburn
                                    {
                                        modifiers[i] = 16;
                                        modifierValues[i] = 50;
                                    }
                                    else if (temp == 2)//chance to deal cursed inferno
                                    {
                                        modifiers[i] = 17;
                                        modifierValues[i] = 25;
                                    }
                                    else //chance to life leech
                                    {
                                        modifiers[i] = 18;
                                        modifierValues[i] = 10;
                                    }
                                    break;
                                }
                        }
                    }
                }
                Load(item, Save(item));
            }
            else if (itemType.Equals("armor"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    while (true)
                    {
                        Boolean pass = true;
                        randNum = rand.Next(0, 6) + 1;
                        for (int j = 0; j < modifiers.Length; j++)
                        {
                            if (randNum == cases[j])
                                pass = false;
                        }
                        if (pass)
                        {
                            cases[i] = randNum;
                            break;
                        }
                    }
                    int tempValue;
                    if (!rarity.Equals("Primal"))//not a primal
                    {
                        switch (randNum)
                        {
                            case (1):
                                {
                                    int temp = rand.Next(0, 3);
                                    if (temp == 0)//defense
                                    {
                                        tempValue = rand.Next(9, 15) + 1;
                                        modifiers[i] = 1;
                                        modifierValues[i] = tempValue;
                                    }
                                    else if (temp == 1)//defense while standing still
                                    {
                                        tempValue = rand.Next(19, 25) + 1;
                                        modifiers[i] = 2;
                                        modifierValues[i] = tempValue;
                                    }
                                    else//defense while under 50% hp
                                    {
                                        tempValue = rand.Next(19, 25) + 1;
                                        modifiers[i] = 3;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (2):
                                {
                                    if (rand.Next(0, 2) == 0)//max hp
                                    {
                                        tempValue = rand.Next(9, 20) + 1;
                                        modifiers[i] = 4;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //max mp
                                    {
                                        tempValue = rand.Next(9, 20) + 1;
                                        modifiers[i] = 5;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (3):
                                {
                                    if (rand.Next(0, 2) == 0) //move speed
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 6;
                                        modifierValues[i] = tempValue;
                                    }
                                    else//jump speed
                                    {
                                        tempValue = rand.Next(9, 15) + 1;
                                        modifiers[i] = 7;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (4):
                                {
                                    tempValue = rand.Next(4, 10) + 1;
                                    modifiers[i] = 8;   //mining speed
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (5):
                                {
                                    tempValue = rand.Next(4, 10) + 1;
                                    modifiers[i] = 9;   //ignores damage taken if under
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (6):
                                {
                                    tempValue = rand.Next(149, 200) + 1;
                                    modifiers[i] = 10;   //damage taken can not exceed
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                        }
                    }
                    else //primal
                    {
                        switch (randNum)
                        {
                            case (1):
                                {
                                    int temp = rand.Next(0, 3);
                                    if (temp == 0)//defense
                                    {
                                        modifiers[i] = 1;
                                        modifierValues[i] = 15;
                                    }
                                    else if (temp == 1)//defense while standing still
                                    {
                                        modifiers[i] = 2;
                                        modifierValues[i] = 25;
                                    }
                                    else//defense while under 50% hp
                                    {
                                        modifiers[i] = 3;
                                        modifierValues[i] = 25;
                                    }
                                    break;
                                }
                            case (2):
                                {
                                    if (rand.Next(0, 2) == 0)//max hp
                                    {
                                        modifiers[i] = 4;
                                        modifierValues[i] = 20;
                                    }
                                    else //max mp
                                    {
                                        modifiers[i] = 5;
                                        modifierValues[i] = 20;
                                    }
                                    break;
                                }
                            case (3):
                                {
                                    if (rand.Next(0, 2) == 0)//move speed
                                    {
                                        modifiers[i] = 6;
                                        modifierValues[i] = 10;
                                    }
                                    else//jump speed
                                    {
                                        modifiers[i] = 7;
                                        modifierValues[i] = 15;
                                    }
                                    break;
                                }
                            case (4):
                                {
                                    modifiers[i] = 8;   //mining speed
                                    modifierValues[i] = 10;
                                    break;
                                }
                            case (5):
                                {
                                    modifiers[i] = 9;   //ignores damage taken if under
                                    modifierValues[i] = 10;
                                    break;
                                }
                            case (6):
                                {
                                    modifiers[i] = 10;   //damage taken can not exceed
                                    modifierValues[i] = 100;
                                    break;
                                }
                        }
                    }
                }
                Load(item, Save(item));
            }
            else if (itemType.Equals("accessory"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    while (true)
                    {
                        Boolean pass = true;
                        randNum = rand.Next(0, 8) + 1;
                        for (int j = 0; j < modifiers.Length; j++)
                        {
                            if (randNum == cases[j])
                                pass = false;
                        }
                        if (pass)
                        {
                            cases[i] = randNum;
                            break;
                        }
                    }
                    int tempValue;
                    if (!rarity.Equals("Primal"))//not a primal
                    {
                        switch (randNum)
                        {
                            case (1):   //potion cooldown
                                {
                                    tempValue = rand.Next(2, 6) + 1;
                                    modifiers[i] = 1;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (2):   //armor pen
                                {
                                    tempValue = rand.Next(1, 5) + 1;
                                    modifiers[i] = 2;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (3):   //regen
                                {
                                    tempValue = rand.Next(0, 2) + 1;
                                    modifiers[i] = 3;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (4):   //thorns
                                {
                                    tempValue = rand.Next(9, 20) + 1;
                                    modifiers[i] = 4;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (5):   //endurance
                                {
                                    tempValue = rand.Next(1, 3) + 1;
                                    modifiers[i] = 5;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (6):
                                {
                                    if (rand.Next(0, 2) == 0)   //max hp
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 6;
                                        modifierValues[i] = tempValue;
                                    }
                                    else //max mp
                                    {
                                        tempValue = rand.Next(4, 10) + 1;
                                        modifiers[i] = 7;
                                        modifierValues[i] = tempValue;
                                    }
                                    break;
                                }
                            case (7):   //mana usage
                                {
                                    tempValue = rand.Next(4, 7) + 1;
                                    modifiers[i] = 8;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                            case (8):   //iframes
                                {
                                    tempValue = rand.Next(9, 20) + 1;
                                    modifiers[i] = 9;
                                    modifierValues[i] = tempValue;
                                    break;
                                }
                        }
                    }
                    else //primal
                    {
                        switch (randNum)
                        {
                            case (1):
                                {
                                    modifiers[i] = 1;
                                    modifierValues[i] = 6;
                                    break;
                                }
                            case (2):
                                {
                                    modifiers[i] = 2;
                                    modifierValues[i] = 5;
                                    break;
                                }
                            case (3):
                                {
                                    modifiers[i] = 3;
                                    modifierValues[i] = 2;
                                    break;
                                }
                            case (4):
                                {
                                    modifiers[i] = 4;
                                    modifierValues[i] = 20;
                                    break;
                                }
                            case (5):
                                {
                                    modifiers[i] = 3;
                                    modifierValues[i] = 5;
                                    break;
                                }
                            case (6):
                                {
                                    if (rand.Next(0, 2) == 0)
                                    {
                                        modifiers[i] = 6;
                                        modifierValues[i] = 10;
                                    }
                                    else
                                    {
                                        modifiers[i] = 7;
                                        modifierValues[i] = 10;
                                    }
                                    break;
                                }
                            case (7):
                                {
                                    modifiers[i] = 8;
                                    modifierValues[i] = 7;
                                    break;
                                }
                            case (8):
                                {
                                    modifiers[i] = 9;
                                    modifierValues[i] = 20;
                                    break;
                                }
                        }
                    }
                }
                Load(item, Save(item));
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "Rarity", "" + rarity + "");
            if (rarity.Equals("Magical"))
            {
                line.overrideColor = new Color(130, 135, 240);
            }
            else if (rarity.Equals("Rare"))
            {
                line.overrideColor = new Color(249, 249, 9);
            }
            else if (rarity.Equals("Legendary"))
            {
                line.overrideColor = new Color(241, 165, 0);
            }
            else if (rarity.Equals("Primal"))
            {
                line.overrideColor = new Color(225, 0, 49);
            }
            else
            {
                line.overrideColor = new Color(102, 204, 255);
            }
            tooltips.Insert(1, line);
            if (itemType.Equals("weapon"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    String modifierName = "";
                    Boolean sign = true;
                    Boolean percent = true;
                    if (modifiers[i] == 1)
                    {
                        modifierName = "damage while standing still";
                    }
                    else if (modifiers[i] == 2)
                    {
                        modifierName = "knockback while moving";
                    }
                    else if (modifiers[i] == 3)
                    {
                        modifierName = "damage when below half health";
                    }
                    else if (modifiers[i] == 4)
                    {
                        modifierName = "damage when full health";
                    }
                    else if (modifiers[i] == 5)
                    {
                        modifierName = "damage when below half mana";
                    }
                    else if (modifiers[i] == 6)
                    {
                        if (Main.hardMode)
                            modifierName = "damage when below 50 defense";
                        else
                            modifierName = "damage when below 20 defense";
                    }
                    else if (modifiers[i] == 7)
                    {
                        modifierName = "damage while in the jungle";
                    }
                    else if (modifiers[i] == 8)
                    {
                        if (WorldGen.totalEvil > 0)
                            modifierName = "damage while in corruption";
                        else
                            modifierName = "damage while in crimson";
                    }
                    else if (modifiers[i] == 9)
                    {
                        modifierName = "damage while near water";
                    }
                    else if (modifiers[i] == 10)
                    {
                        modifierName = "damage while in hallow";
                    }
                    else if (modifiers[i] == 11)
                    {
                        modifierName = "attack speed during day";
                    }
                    else if (modifiers[i] == 12)
                    {
                        modifierName = "attack speed during night";
                    }
                    else if (modifiers[i] == 13)
                    {
                        modifierName = "critical strike chance while invulnerable";
                    }
                    else if (modifiers[i] == 14)
                    {
                        modifierName = "mana cost";
                        sign = false;
                        percent = false;
                    }
                    else if (modifiers[i] == 15)
                    {
                        modifierName = "chance to inflict fire";
                    }
                    else if (modifiers[i] == 16)
                    {
                        modifierName = "chance to inflict frostburn";
                    }
                    else if (modifiers[i] == 17)
                    {
                        modifierName = "chance to inflict cursed inferno";
                    }
                    else if (modifiers[i] == 18)
                    {
                        modifierName = "chance to heal on hit";
                    }

                    String signString = "+";
                    if (!sign)
                        signString = "-";
                    String percentString = "%";
                    if (!percent)
                        percentString = "";

                    TooltipLine line2;
                    line2 = new TooltipLine(mod, "Modifiers", signString + modifierValues[i] + percentString + " " + modifierName);
                    line2.overrideColor = Color.LightGreen;
                    tooltips.Add(line2);
                }
            }
            else if (itemType.Equals("armor"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    String modifierName = "";
                    int modAmount = modifierValues[i];
                    bool sign = true;
                    bool percent = true;
                    bool form = true;
                    if (modifiers[i] == 1)
                    {
                        modifierName = "defense";
                    }
                    else if (modifiers[i] == 2)
                    {
                        modifierName = "defense while still";
                    }
                    else if (modifiers[i] == 3)
                    {
                        modifierName = "defense while under half health";
                    }
                    else if (modifiers[i] == 4)
                    {
                        percent = false;
                        modifierName = "maximum health";
                    }
                    else if (modifiers[i] == 5)
                    {
                        percent = false;
                        modifierName = "maximum mana";
                    }
                    else if (modifiers[i] == 6)
                    {
                        modifierName = "movement speed";
                    }
                    else if (modifiers[i] == 7)
                    {
                        modifierName = "jump speed";
                    }
                    else if (modifiers[i] == 8)
                    {
                        modifierName = "mining speed";
                    }
                    else if (modifiers[i] == 9)
                    {
                        form = false;
                        sign = false;
                        percent = false;
                        modifierName = "Ignores damage taken if under";
                    }
                    else if (modifiers[i] == 10)
                    {
                        form = false;
                        sign = false;
                        percent = false;
                        if (Main.hardMode)
                            modAmount += 100;
                        modifierName = "Damage taken cannot exceed";
                    }

                    String signString = "+";
                    if (!sign)
                        signString = "-";

                    String percentString = "%";
                    if (!percent)
                        percentString = "";

                    TooltipLine line2;

                    if (form)
                        line2 = new TooltipLine(mod, "Modifiers", signString + modAmount + percentString + " " + modifierName);
                    else if (!Main.hardMode)
                        line2 = new TooltipLine(mod, "Modifiers", modifierName + " " + modAmount);
                    else
                        line2 = new TooltipLine(mod, "Modifiers", modifierName + " " + modAmount);
                    line2.overrideColor = Color.LightGreen;
                    tooltips.Add(line2);
                }
            }
            else if (itemType.Equals("accessory"))
            {
                for (int i = 0; i < rarityValue; i++)
                {
                    String modifierName = "";
                    int modAmount = modifierValues[i];
                    bool sign = true;
                    bool percent = true;
                    bool form = true;
                    if (modifiers[i] == 1)
                    {
                        modifierName = "Reduces potion cooldown by";
                        form = false;
                        percent = false;
                        sign = false;
                    }
                    else if (modifiers[i] == 2)
                    {
                        form = false;
                        percent = false;
                        sign = false;
                        modifierName = "Increases armor penetration by";
                    }
                    else if (modifiers[i] == 3)
                    {
                        percent = false;
                        modifierName = "health regeneration";
                    }
                    else if (modifiers[i] == 4)
                    {
                        modifierName = "thorns";
                    }
                    else if (modifiers[i] == 5)
                    {
                        sign = false;
                        modifierName = "damage taken";
                    }
                    else if (modifiers[i] == 6)
                    {
                        percent = false;
                        modifierName = "maximum health";
                    }
                    else if (modifiers[i] == 7)
                    {
                        percent = false;
                        modifierName = "maximum mana";
                    }
                    else if (modifiers[i] == 8)
                    {
                        sign = false;
                        modifierName = "mana cost";
                    }
                    else if (modifiers[i] == 9)
                    {
                        modifierName = "invulnerability time";
                    }

                    String signString = "+";
                    if (!sign)
                        signString = "-";

                    String percentString = "%";
                    if (!percent)
                        percentString = "";

                    TooltipLine line2;

                    if (form)
                        line2 = new TooltipLine(mod, "Modifiers", signString + modAmount + percentString + " " + modifierName);
                    else if (!Main.hardMode)
                        line2 = new TooltipLine(mod, "Modifiers", modifierName + " " + modAmount);
                    else
                        line2 = new TooltipLine(mod, "Modifiers", modifierName + " " + modAmount);
                    line2.overrideColor = Color.LightGreen;
                    tooltips.Add(line2);
                }
            }
        }

        public override void GetWeaponDamage(Item item, Player player, ref int damage)
        {
            bool moving = player.velocity.Length() > 0;   //returns true if player is moving
            if (itemType.Equals("weapon"))
            {
                if (HasMod(1) > -1 && !moving)  //not moving
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(1)] / 100.0)) * damage));
                if (HasMod(3) > -1 && player.statLife < (int)(player.statLifeMax2 / 2))  //less than half HP
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(3)] / 100.0)) * damage));
                if (HasMod(4) > -1 && player.statLife == player.statLifeMax2)  //full hp
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(4)] / 100.0)) * damage));
                if (HasMod(5) > -1 && player.statMana < (int)(player.statManaMax2 / 2))  //less than half MP
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(5)] / 100.0)) * damage));
                if (HasMod(6) > -1 && player.statDefense < 20 && !Main.hardMode)  //less than 20 def in prehardmode
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(6)] / 100.0)) * damage));
                else if (HasMod(6) > -1 && player.statDefense < 50 && Main.hardMode)  //less than 50 def in hardmode
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(6)] / 100.0)) * damage));
                if (HasMod(7) > -1 && player.ZoneJungle)    //player in jungle
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(8)] / 100.0)) * damage));
                if (HasMod(8) > -1 && (player.ZoneCorrupt || player.ZoneCrimson))    //player in evil
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(8)] / 100.0)) * damage));
                if (HasMod(9) > -1 && (player.adjWater || player.oldAdjWater))    //player in water
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(9)] / 100.0)) * damage));
                if (HasMod(10) > -1 && player.ZoneHoly)    //player in holy
                    damage = (int)Math.Round(((1 + (modifierValues[HasMod(10)] / 100.0)) * damage));

                if (HasMod(13) > -1 && player.immune)
                    item.crit = baseCrit + modifierValues[HasMod(13)];
                else if (HasMod(13) > -1 && !player.immune)
                    item.crit = baseCrit;

                if (HasMod(15) > -1)  //fire
                {
                    player.GetModPlayer<ARPGPlayer>(mod).onHit = "Fire";
                    player.GetModPlayer<ARPGPlayer>(mod).onHitChance = modifierValues[HasMod(15)];
                }
                else if (HasMod(16) > -1)  //frostburn
                {
                    player.GetModPlayer<ARPGPlayer>(mod).onHit = "Frostburn";
                    player.GetModPlayer<ARPGPlayer>(mod).onHitChance = modifierValues[HasMod(16)];
                }
                else if (HasMod(17) > -1)  //cursed inferno
                {
                    player.GetModPlayer<ARPGPlayer>(mod).onHit = "Curse";
                    player.GetModPlayer<ARPGPlayer>(mod).onHitChance = modifierValues[HasMod(17)];
                }
                else if (HasMod(18) > -1)  //leech
                {
                    player.GetModPlayer<ARPGPlayer>(mod).onHit = "Leech";
                    player.GetModPlayer<ARPGPlayer>(mod).onHitChance = modifierValues[HasMod(18)];
                }
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (itemType.Equals("weapon"))
            {
                if (HasMod(11) > -1 && Main.dayTime)  //day time 
                    return 1 + (modifierValues[HasMod(11)] / 100.0f);
                if (HasMod(12) > -1 && !Main.dayTime)  //night time 
                    return 1 + (modifierValues[HasMod(12)] / 100.0f);
            }
            return 1f;
        }

        public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
        {
            if (itemType.Equals("weapon"))
            {
                bool moving = player.velocity.Length() > 0;
                if (HasMod(2) > -1 && moving)  //player moving
                    knockback = (1 + (modifierValues[HasMod(2)] / 100.0f)) * knockback;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (itemType.Length <= 0)
            {
                int seed = 0;
                for (int i = 0; i < item.Name.Length; i++)
                {
                    seed += (int)item.Name[i];
                }
                rand = new Random(seed);
                Assign(item);
            }
            if (itemType.Equals("armor") && modifiers.Length > 0)
            {
                bool moving = player.velocity.Length() > 0;   //returns true if player is moving
                if (HasMod(1) > -1)
                {
                    item.defense = (int)Math.Round(((1 + (modifierValues[HasMod(1)] / 100.0)) * baseDefense));
                }
                else if (HasMod(2) > -1 && !moving)
                {
                    item.defense = (int)Math.Round(((1 + (modifierValues[HasMod(2)] / 100.0)) * baseDefense));
                }
                else if (HasMod(3) > -1 && player.statLife < (int)(player.statLifeMax2 / 2))
                {
                    item.defense = (int)Math.Round(((1 + (modifierValues[HasMod(3)] / 100.0)) * baseDefense));
                }
                else
                {
                    item.defense = baseDefense;
                }
                if (HasMod(4) > -1)
                {
                    player.statLifeMax2 += modifierValues[HasMod(4)];
                }
                else if (HasMod(5) > -1)
                {
                    player.statManaMax2 += modifierValues[HasMod(5)];
                }
                if (HasMod(6) > -1)
                {
                    player.maxRunSpeed = 1f + (modifierValues[HasMod(6)] / 100.0f) * player.maxRunSpeed;
                }
                else if (HasMod(7) > -1)
                {
                    player.jumpSpeedBoost = 1f + (modifierValues[HasMod(7)] / 100.0f) * player.jumpSpeedBoost;
                }
                if (HasMod(8) > -1)
                {
                    player.pickSpeed = 1f + (modifierValues[HasMod(8)] / 100.0f) * player.pickSpeed;
                }
                if (HasMod(9) > -1)
                {
                    if (item.headSlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage1 = modifierValues[HasMod(9)];
                    else if (item.bodySlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage2 = modifierValues[HasMod(9)];
                    else
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage3 = modifierValues[HasMod(9)];
                }
                else
                {
                    if (item.headSlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage1 = 0;
                    else if (item.bodySlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage2 = 0;
                    else
                        player.GetModPlayer<ARPGPlayer>(mod).immuneDamage3 = 0;
                }
                if (HasMod(10) > -1)
                {
                    if (Main.hardMode)
                    {
                        if (item.headSlot > 0)
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken1 = modifierValues[HasMod(10)] + 100;
                        else if (item.bodySlot > 0)
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken2 = modifierValues[HasMod(10)] + 100;
                        else
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken3 = modifierValues[HasMod(10)] + 100;
                    }
                    else
                    {
                        if (item.headSlot > 0)
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken1 = modifierValues[HasMod(10)];
                        else if (item.bodySlot > 0)
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken2 = modifierValues[HasMod(10)];
                        else
                            player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken3 = modifierValues[HasMod(10)];
                    }
                }
                else
                {
                    if (item.headSlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken1 = int.MaxValue;
                    else if (item.bodySlot > 0)
                        player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken2 = int.MaxValue;
                    else
                        player.GetModPlayer<ARPGPlayer>(mod).maxDamageTaken3 = int.MaxValue;
                }
            }
            if (itemType.Equals("accessory") && modifiers.Length > 0)
            {
                if (HasMod(1) > -1)
                {
                    player.GetModPlayer<ARPGPlayer>(mod).potionReduce = modifierValues[HasMod(1)];
                }
                if (HasMod(2) > -1)
                {
                    player.armorPenetration += modifierValues[HasMod(2)];
                }
                if (HasMod(3) > -1)
                {
                    player.lifeRegen += modifierValues[HasMod(3)];
                }
                if (HasMod(4) > -1)
                {
                    player.thorns += modifierValues[HasMod(4)] / 100.0f;
                }
                if (HasMod(5) > -1)
                {
                    player.endurance += modifierValues[HasMod(5)] / 100.0f;
                }
                if (HasMod(6) > -1)
                {
                    player.statLifeMax2 += modifierValues[HasMod(6)];
                }
                if (HasMod(7) > -1)
                {
                    player.statManaMax2 += modifierValues[HasMod(7)];
                }
                if (HasMod(8) > -1)
                {
                    player.manaCost -= modifierValues[HasMod(8)] / 100.0f;
                }
                if (HasMod(9) > -1)
                {
                    player.GetModPlayer<ARPGPlayer>(mod).iFrames += modifierValues[HasMod(9)];
                }
            }
        }

        public override bool NeedsSaving(Item item)
        {
            return rarity.Length > 0;
        }

        public override TagCompound Save(Item item)
        {
            return new TagCompound {
                {"itemType",itemType},{"rarity", rarity},{"rarityValue", rarityValue},{"modifiers", modifiers},{"modifierValues", modifierValues},{"baseDamage",baseDamage},{"baseCrit",baseCrit},{"baseDefense",baseDefense},{"baseMana",baseMana}
            };
        }

        public override void Load(Item item, TagCompound tag)
        {
            itemType = tag.GetString("itemType");
            rarity = tag.GetString("rarity");
            rarityValue = tag.GetInt("rarityValue");
            modifiers = tag.GetIntArray("modifiers");
            modifierValues = tag.GetIntArray("modifierValues");

            baseDamage = tag.GetInt("baseDamage");
            baseCrit = tag.GetInt("baseCrit");
            baseDefense = tag.GetInt("baseDefense");
            baseMana = tag.GetInt("baseMana");

            if (itemType.Equals("weapon"))
            {
                if (item.magic && HasMod(14) > -1)
                {
                    item.mana = baseMana - modifierValues[HasMod(14)];
                }
            }
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (seedPlus < 0)
            {
                player.GetModPlayer<ARPGPlayer>(mod).seedPlus = player.GetModPlayer<ARPGPlayer>(mod).seedPlus + 1;
                this.seedPlus = player.GetModPlayer<ARPGPlayer>(mod).seedPlus;
            }
            this.player = player;
            if (rarity.Equals("Unidentified"))
            {
                player.GetModPlayer<ARPGPlayer>(mod).canUse = true;
            }
            if (player.GetModPlayer<ARPGPlayer>(mod).clarity2 && rarity.Equals("Unidentified"))
            {
                Roll(item);
            }
            if ((player.GetModPlayer<ARPGPlayer>(mod).clarity && rarity.Equals("Unidentified")))
            {
                Roll(item);
                player.GetModPlayer<ARPGPlayer>(mod).clarity = false;
                player.GetModPlayer<ARPGPlayer>(mod).canUse = false;
            }
        }

        public override void OnCraft(Item item, Recipe recipe)
        {
            if (itemType.Length > 0)
                Roll(item);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (itemType.Length <= 0)
                Assign(item);
            return true;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (itemType.Length <= 0)
                Assign(item);
        }

        public override void PreReforge(Item item)
        {
            player.GetModPlayer<ARPGPlayer>(mod).ir = (ARPGItem)Clone(item, new Item());
        }

        public override void PostReforge(Item item)
        {
            if (player.GetModPlayer<ARPGPlayer>(mod).ir != null)
                Unpack(player.GetModPlayer<ARPGPlayer>(mod).ir);
        }

        public void Unpack(ARPGItem item)
        {
            itemType = item.itemType;
            rarity = item.rarity;
            rarityValue = item.rarityValue;
            modifiers = item.modifiers;
            modifierValues = item.modifierValues;
            baseDamage = item.baseDamage;
            baseCrit = item.baseCrit;
            baseDefense = item.baseDefense;
            baseMana = item.baseMana;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(itemType);
            writer.Write(rarity);
            writer.Write(rarityValue);
            for (int i = 0; i < modifiers.Length; i++)
            {
                writer.Write(modifiers[i]);
            }
            for (int i = 0; i < modifierValues.Length; i++)
            {
                writer.Write(modifierValues[i]);
            }
            writer.Write(baseDamage);
            writer.Write(baseCrit);
            writer.Write(baseDefense);
            writer.Write(baseMana);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            itemType = reader.ReadString();
            rarity = reader.ReadString();
            rarityValue = reader.ReadInt32();
            for (int i = 0; i < rarityValue; i++)
            {
                modifiers[i] = reader.ReadInt32();
            }
            for (int i = 0; i < rarityValue; i++)
            {
                modifierValues[i] = reader.ReadInt32();
            }
            baseDamage = reader.ReadInt32();
            baseCrit = reader.ReadInt32();
            baseDefense = reader.ReadInt32();
            baseMana = reader.ReadInt32();
        }

        private void Assign(Item item)
        {
            int seed = 0;   //used to pseudo-randomize items, especially when spawning for the first time
            for (int i = 0; i < item.Name.Length; i++)
            {
                seed += (int)item.Name[i];
            }
            seed += seedPlus;
            seed += (int)DateTime.UtcNow.Ticks;
            rand = new Random(seed);

            if (item.maxStack == 1 && item.accessory)
            {
                itemType = "accessory";
                rarity = "Unidentified";
            }
            else if (item.maxStack == 1 && item.damage > 0)
            {
                itemType = "weapon";
                rarity = "Unidentified";
            }
            else if (item.maxStack == 1 && item.defense > 0)
            {
                itemType = "armor";
                rarity = "Unidentified";
            }

        }

        private int HasMod(int mod)
        {
            if (modifiers != null)
            {
                for (int i = 0; i < modifiers.Length; i++)
                {
                    if (modifiers[i] == mod)
                        return i;
                }
            }
            return -1;
        }
    }
}
