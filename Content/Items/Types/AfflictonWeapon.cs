using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using AfflictionClass.Content.Buffs;
using AfflictionClass.Content.Players;
using Microsoft.Xna.Framework;

namespace AfflictionClass.Content.Items.Types
{
    public abstract class AfflictionWeapon : ModItem
    {
        // Subtype flags — override these in each weapon
        public virtual bool IsPlague => false;
        public virtual bool IsCorrosion => false;

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
            return IsPlague ? "Plague" :
                   IsCorrosion ? "Corrosion" :
                   "Affliction";
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            var modPlayer = player.GetModPlayer<AfflictionPlayer>();

            if (IsPlague)
                damage = (int)(damage * (1f + modPlayer.plagueDamage));

            // You can add more types later:
            // if (IsCorrosion)
            //     damage = (int)(damage * (1f + modPlayer.corrosionDamage));
        } 
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            var modPlayer = player.GetModPlayer<AfflictionPlayer>();
            if (IsPlague)
                damage += modPlayer.plagueDamage;
            //if (IsCorrosion)
            //    damage += modPlayer.corrosionDamage;
        }
    }
}