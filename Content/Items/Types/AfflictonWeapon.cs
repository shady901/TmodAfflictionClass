using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using AfflictionClass.Content.Buffs;
using AfflictionClass.Content.Players;
using Microsoft.Xna.Framework;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Helper;

namespace AfflictionClass.Content.Items.Types
{
    public abstract class AfflictionWeapon : ModItem
    {
        // Subtype flags — override these in each weapon
        public virtual DamageTypeEnum DamageType { get; set; } = DamageTypeEnum.Generic;

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var line = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (line != null)
            {
                string[] split = line.Text.Split(' ');
               

                line.Text = $"{split.First()} {GetTypeName()} {split.Last()}";
            }
        }

        private string GetTypeName()
        {
            return DamageType == DamageTypeEnum.Plague ? "Plague" :
                   DamageType == DamageTypeEnum.Corrosive ? "Corrosive" :
                   "Generic";
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
       

            // Apply the modifiers using your custom method
            damage = AfflictionHelper.ApplyModifiersToDamage(damage, player, DamageType);

        } 
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
         
            // Apply the damage modifiers using your custom helper method
            
            AfflictionModifiers afflictionModifiers = AfflictionHelper.GetAfflictionModifiers(player, DamageType);

            damage *= afflictionModifiers.damagePercent;
            damage.Flat += afflictionModifiers.flatDamage;
         }
        
    }
}