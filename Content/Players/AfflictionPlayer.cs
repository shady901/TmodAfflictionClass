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

        // Override this method to set up your modifiers and initialize the dictionary
        public override void Initialize()
        {
            allModifiers = new Dictionary<DamageTypeEnum, AfflictionModifiers>
        {
            { DamageTypeEnum.Plague, plagueDamageModifiers },
            { DamageTypeEnum.Corrosive, corrosiveDamageModifiers }
        };

            // Add more modifiers as needed
            // allModifiers[DamageTypeEnum.Void] = new AfflictionModifiers() { damageType = DamageTypeEnum.Void };
        }
        public override void ResetEffects()
        {
            // Resetting modifiers for each damage type
            foreach (var modifier in allModifiers.Values)
            {
                modifier.SetDefault();
            }
        }

        // Get the correct damage modifiers based on the damage type
        public AfflictionModifiers GetDamageModifiers(DamageTypeEnum damageType)
        {
            // Return the associated modifier or a new default one if not found
            return allModifiers.ContainsKey(damageType) ? allModifiers[damageType] : new AfflictionModifiers();
        }
    }





    public class AfflictionModifiers
    {
        public DamageTypeEnum damageType;
        public float damagePercent = 1f;  // Multiplier for base damage
        public float flatDamage = 0f;     // Flat damage added
        public float critChance = 0f;     // Crit chance modifier

        public float dotScalingPercent = 1f;  // Multiplier for DoT damage
        public float dotTickRatePercent = 1f; // How fast the DoT ticks (1 = normal speed, >1 = faster)
        public float dotCritChance = 0f;      // Crit chance for DoT ticks
        public bool dotCanCrit = false;        // Should the DoT crit?


        // Resets all properties to their default values
        public void SetDefault()
        {
            damagePercent = 1f;
            flatDamage = 0f;
            critChance = 0f;
            dotScalingPercent = 1f;
            dotTickRatePercent = 1f;
            dotCritChance = 0f;
            dotCanCrit = false;
        }
    }
}
