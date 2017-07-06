using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ARPGLoot
{
    class ARPGTile : GlobalTile
    {
        bool placeableItem;
        public override void RightClick(int i,int j,int type)
        {
            if (type == TileID.ItemFrame || type == TileID.WeaponsRack)
                placeableItem = true;
        }
        //???
    }
}