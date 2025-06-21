using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfflictionClass.Content.Config
{
    public static class AfflictionConstants
    {
        // === CORROSIVE DOT ===
        public const int CorrosiveBaseTickRate = 60;              // 1 second per tick
        public const int CorrosiveBaseDuration = 300;             // 5 seconds
        public const int CorrosiveMaxStacks = 10;
        public const float CorrosiveStackMultiplier = 0.2f;       // +20% damage per 3 stacks
        public const int CorrosiveStackDamageRatio = 3;
        // === Void Stacks ===
        public const int VoidStackInactivityTimeout = 300;
        // === AGITATED DEBUFF ===
        public const float AgitatedTickBoostMultiplier = 1.25f;   // 25% faster ticks

        // === AMP DEBUFF & GEAR ===
        public const float BaseDebuffAmpPercent = 1.2f;       // 1.0 = normal, >1 = amplified

        // === SPREAD DEBUFF ON DEATH ===
        public const float DebuffSpreadStackMultiplier = 0.5f;    // Spread 50% of current stacks

        // === GENERAL DOT SETTINGS ===
        public const float BaseDotScalingPercent = 1.0f;
        public const float BaseTickRateModifier = 1.0f;           // Used in gear, accessories
        public const float BaseDurationModifier = 1.0f;           // For future gear stat

        // === ON-HIT STACKING DEBUFF ===
        public const int OnHitMaxStacks = 10;
        public const int OnHitDuration = 180;                     // 3 seconds duration
        public const int OnHitRefreshTime = 60;                   // Refresh timer per hit
        public const float OnHitBaseEffectPower = 1.0f;

        // === DEBUG / TUNING HELPERS ===
        public const bool DebugShowCombatText = false;
    }
}