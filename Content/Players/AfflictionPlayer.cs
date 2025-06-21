using AfflictionClass.Content.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AfflictionClass.Content.Players
{
    public class AfflictionPlayer : ModPlayer
    {
        //to be changed only placeholder damage type right now, 
        //assecorries to focus on each type of afflictions and how they interact with damage types
        //theme weapons based on type ideas plague(higher base damage but doesnt stack), corrosive(scaling dot stacks, single target focus), void/hex changes dot properties or adds some, 
        private Dictionary<DamageTypeEnum, AfflictionModifiers> allModifiers;

        public AfflictionModifiers plagueDamageModifiers = new AfflictionModifiers() { damageType = DamageTypeEnum.Plague };
        public AfflictionModifiers corrosiveDamageModifiers = new AfflictionModifiers() { damageType = DamageTypeEnum.Corrosive };
        public AfflictionModifiers voidDamageModifiers = new AfflictionModifiers() { damageType = DamageTypeEnum.Void };
        public DpsTracker CorrosiveDps = new();
        public DpsTracker PlagueDps = new();
        // Override this method to set up your modifiers and initialize the dictionary
        public override void Initialize()
        {
            allModifiers = new Dictionary<DamageTypeEnum, AfflictionModifiers>
            {
                { DamageTypeEnum.Plague, plagueDamageModifiers },
                { DamageTypeEnum.Corrosive, corrosiveDamageModifiers },
                { DamageTypeEnum.Void, voidDamageModifiers } 
            };
        }
        public override void ResetEffects()
        {
            // Resetting modifiers for each damage type
            foreach (var modifier in allModifiers.Values)
            {
                modifier.SetDefault();
            }
            CorrosiveDps.Tick();
            PlagueDps.Tick();
        }

        // Get the correct damage modifiers based on the damage type
        public AfflictionModifiers GetDamageModifiers(DamageTypeEnum damageType)
        {
            // Return the associated modifier or a new default one if not found
            if (allModifiers.TryGetValue(damageType, out var modifier))
                return modifier;

            throw new Exception($"AfflictionModifier missing for damage type: {damageType}");
        }
    }


    public class DpsTracker
    {
        public int RollingTotal = 0;
        public int LastSecond = 0;
        private int timer = 60;

        public void Register(int amount)
        {
            RollingTotal += amount;
        }

        public void Tick()
        {
            timer--;
            if (timer <= 0)
            {
                LastSecond = RollingTotal;
                RollingTotal = 0;
                timer = 60;
            }
        }
    }


    public class AfflictionModifiers
    {
        public DamageTypeEnum damageType;

        // Base damage
        public float damagePercent = 1f;    // Multiplier for base weapon damage
        public float flatDamage = 0f;       // Flat added damage

        // Crit system
        public float critChance = 0f;       // Custom crit chance (added on top of vanilla)
        public float critMulti = 2f;        // Custom crit multiplier (default 2x like vanilla)
        public bool useCustomCrit = false;  // If true, overrides vanilla crit system (optional)

        // DoT properties
        public float dotScalingPercent = 1f;      // DoT total scaling
        public float dotTickRatePercent = 1f;     // Speed of dot ticks
        public float dotCritChance = 0f;          // Crit chance for DoT
        public bool dotCanCrit = false;           // Can DoT crit?
        public float dotCritMultiplier = 1.5f;     // Multiplier for DoT crits

        // Debuff synergy
        public float debuffAmplifyPercent = 1f;   // Amplifies debuff damage
        public float tickRateGlobalMultiplier = 1f; // Global tick speed modifier
        //Pen
        public int pen = 0;

        // Reset all fields
        public void SetDefault()
        {
            damagePercent = 1f;
            flatDamage = 0f;
            critChance = 0f;
            critMulti = 2f;
            useCustomCrit = false;

            dotScalingPercent = 1f;
            dotTickRatePercent = 1f;
            dotCritChance = 0f;
            dotCanCrit = false;
            dotCritMultiplier = 1.5f;

            debuffAmplifyPercent = 1f;
            tickRateGlobalMultiplier = 1f;

            pen = 0;
        }
    }
}
