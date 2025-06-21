using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Players;
using Terraria;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Helper
{
    public static class AfflictionCritHelper
    {
        // Rolls a crit based on total chance from vanilla + affliction
        public static bool RollCrit(Player player, DamageTypeEnum type)
        {
            var afflictionPlayer = player.GetModPlayer<AfflictionPlayer>();
            var mod = afflictionPlayer.GetDamageModifiers(type);

            float baseCrit = player.GetCritChance(DamageClass.Generic); // fallback
            float totalCrit = baseCrit + mod.critChance;

            return Main.rand.NextFloat() < totalCrit * 0.01f;
        }

        // Returns the crit multiplier (custom if available)
        public static float GetCritMultiplier(Player player, DamageTypeEnum type)
        {
            var afflictionPlayer = player.GetModPlayer<AfflictionPlayer>();
            var mod = afflictionPlayer.GetDamageModifiers(type);
            return mod.critMulti;
        }

        // Optional: DoT version
        public static bool RollDotCrit(Player player, DamageTypeEnum type)
        {
            var afflictionPlayer = player.GetModPlayer<AfflictionPlayer>();
            var mod = afflictionPlayer.GetDamageModifiers(type);

            if (!mod.dotCanCrit)
                return false;

            return Main.rand.NextFloat() < mod.dotCritChance * 0.01f;
        }

        public static float GetDotCritMultiplier(Player player, DamageTypeEnum type)
        {
            var afflictionPlayer = player.GetModPlayer<AfflictionPlayer>();
            var mod = afflictionPlayer.GetDamageModifiers(type);
            return mod.dotCritMultiplier;
        }
    }
}
