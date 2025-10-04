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
        //ovveride previewBaseDamage as this is what each weapon scales off
        public virtual int BaseDamage => 1;
        public virtual bool SuppressVanillaDamage => true;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var player = Main.LocalPlayer;
            if (!player.TryGetModPlayer<AfflictionPlayer>(out var affPlayer))
                return;
            AfflictionModifiers mods = affPlayer.GetDamageModifiers(DamageType);
            tooltips.RemoveAll(line => line.Mod == "Terraria" && line.Name == "CritChance");
            base.ModifyTooltips(tooltips);

            var damageLine = tooltips.FirstOrDefault(x => x.Mod == "Terraria" && x.Name == "Damage");
            if (damageLine != null)
            {
                int total = (int)(mods.flatDamage + BaseDamage * mods.damagePercent);
                damageLine.Text = $"{total} {GetTypeName()} damage";
            }

          

          

            tooltips.Add(new TooltipLine(Mod, "AfflictionHeader", "[c/8AFF8A:— Affliction Stats —]"));
            tooltips.Add(new TooltipLine(Mod, "AfflictionFlat", $"[c/AAAAFF:+{mods.flatDamage} Flat Damage]"));
            tooltips.Add(new TooltipLine(Mod, "AfflictionMult", $"[c/AAAAFF:+{(mods.damagePercent * 100 - 100):F1}% Total Multiplier]"));
            tooltips.Add(new TooltipLine(Mod, "AfflictionCritFlag", $"[c/FFAAFF:Can Crit: {(mods.canCrit ? "Yes" : "No")}]"));

            if (mods.canCrit)
                tooltips.Add(new TooltipLine(Mod, "AfflictionCritChance", $"[c/FFAAFF:+{mods.critChance}% Crit Chance]"));

            tooltips.Add(new TooltipLine(Mod, "AfflictionPen", $"[c/FF8888:+{mods.pen} Penetration]"));
            tooltips.Add(new TooltipLine(Mod, "AfflictionDebuffAmp", $"[c/FFFF88:Debuff Amp: x{mods.debuffAmplifyPercent:F2}]"));
            tooltips.Add(new TooltipLine(Mod, "AfflictionTickSpeed", $"[c/FFFF88:Tick Speed: x{mods.dotTickRatePercent * mods.tickRateGlobalMultiplier:F2}]"));
        }


        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(player, target, hit, damageDone); // This allows derived classes like VoidSword to work
                                                            // or remove this override entirely if it's not needed
        }
        private string GetTypeName()
        {
            return DamageType == DamageTypeEnum.Plague ? "Plague" :
                 DamageType == DamageTypeEnum.Void ? "Void" :
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