using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Players
{
    internal class AfflictionPlayer: ModPlayer
    {
        //to be changed only placeholder damage type right now, 
        //assecorries to focus on each type of afflictions and how they interact with damage types
        //theme weapons based on type ideas plague(higher base damage but doesnt stack), corrosive(scaling dot stacks, single target focus), void/hex changes dot properties or adds some, 
        public float plagueDamage = 0f;




        public override void ResetEffects()
        {
            plagueDamage = 0.0f;
        }
    }
}
