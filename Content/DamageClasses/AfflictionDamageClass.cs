using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;


namespace AfflictionClass.Content.DamageClasses
{

    public class AfflictionDamageClass : DamageClass
    {
        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            return damageClass == DamageClass.Generic
                ? StatInheritanceData.Full
                : StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return false; // No vanilla class effects
        }

        public override bool UseStandardCritCalcs => false; // We'll define custom crit behavior
    }
}

