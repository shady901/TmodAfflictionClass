using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;


namespace AfflictionClass.Content.Helper
{
    public static class AfflictionHelper
    {

        public static int ApplyModifiersToDamage(int originalDamage, Player player, DamageTypeEnum damageType)
        {
            AfflictionModifiers modifiers = player.GetModPlayer<AfflictionPlayer>().GetDamageModifiers(damageType);
            
            return (int)((originalDamage*modifiers.damagePercent)+ modifiers.flatDamage);
        }

        public static AfflictionModifiers GetAfflictionModifiers(Player player, DamageTypeEnum damageType)
        { 
            return player.GetModPlayer<AfflictionPlayer>().GetDamageModifiers(damageType);

        }
    }
}
