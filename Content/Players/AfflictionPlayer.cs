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
        public float plagueDamage = 0f;




        public override void ResetEffects()
        {
            plagueDamage = 0.0f;
        }
    }
}
