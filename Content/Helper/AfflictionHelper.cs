using AfflictionClass.Content.Buffs.DebuffAgitated;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Players;
using Terraria.ModLoader;
using Terraria;
using AfflictionClass.Content.Buffs.Amp;
using AfflictionClass.Content.Buffs.CorrosiveDebuff;
using AfflictionClass.Content.NPCs;
using Terraria.ID;
using AfflictionClass.Content.Buffs.Void;
using AfflictionClass.Content.Config;
using System;
using Terraria.GameContent;
using System.Linq;


namespace AfflictionClass.Content.Helper
{
    public static class AfflictionHelper
    {
        public static int ApplyModifiersToDamage(int originalDamage, Player player, DamageTypeEnum damageType)
        {
            AfflictionModifiers modifiers = GetAfflictionModifiers(player, damageType);
            return (int)((originalDamage * modifiers.damagePercent) + modifiers.flatDamage);
        }

        public static AfflictionModifiers GetAfflictionModifiers(Player player, DamageTypeEnum damageType)
        {
            return player.GetModPlayer<AfflictionPlayer>().GetDamageModifiers(damageType);
        }

        public static float GetDebuffAmplifyMultiplier(NPC npc, Player player, DamageTypeEnum damageType)
        {
            float amplify = GetAfflictionModifiers(player, damageType).debuffAmplifyPercent;

            if (npc.HasBuff(ModContent.BuffType<AmpDebuff>()))
                amplify += (AfflictionConstants.BaseDebuffAmpPercent-1f);

            return amplify;
        }

        public static float GetTickRateMultiplier(NPC npc, Player player, DamageTypeEnum damageType)
        {
            var modifiers = GetAfflictionModifiers(player, damageType);

            float tickRate = modifiers.dotTickRatePercent * modifiers.tickRateGlobalMultiplier;

            if (npc.HasBuff(ModContent.BuffType<AgitatedDebuff>()))
                tickRate += (AfflictionConstants.AgitatedTickBoostMultiplier-1f);

            return tickRate;
        }
        public static int ApplyArmorToValue(int npcDefence, int pen,int totalDamage)
        {
            float effectiveDef = Math.Max(0, npcDefence - pen);
           return totalDamage -= (int)(effectiveDef * 0.5f);

        }
        public static void AddVoidStackAndCheckExplosion(NPC npc, Player player, int baseDamage, bool forceCrit = false, float ampOverride = -1f)
        {
            var aff = npc.GetGlobalNPC<AfflictionGlobalNPC>();
            var dotMap = aff.voidNPCData.VoidDOTs;
            int stackvalue = 0;
            // Determine amp and crit
            float amp = ampOverride > 0 ? ampOverride : AfflictionHelper.GetDebuffAmplifyMultiplier(npc, player, DamageTypeEnum.Void);
            bool crit = forceCrit || AfflictionCritHelper.RollCrit(player, DamageTypeEnum.Void);
            int voidpen = AfflictionHelper.GetPen(player, DamageTypeEnum.Void);

            if (!dotMap.TryGetValue(player.whoAmI, out var dot))
            {
                dot = new VoidDOTInstance(player.whoAmI, baseDamage, crit, amp,  npc.defense, voidpen);
                dotMap[player.whoAmI] = dot;
                stackvalue = dot.Stacks.Last().FinalDamage; // Grab the damage just added
            }
            else
            {
                stackvalue = dot.AddStack(baseDamage, crit, amp,  npc.defense, voidpen);
            }

            int totalDamage = dot.GetTotalDamage();
            if (totalDamage >= npc.life)
            {
                // 💥 Instant kill
                VoidDebuff.DoVoidDotDamage(npc, totalDamage, player.whoAmI);

                for (int i = 0; i < 10; i++)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame);
                dotMap.Remove(player.whoAmI);
            }
            else
            {              
                    CombatText.NewText(npc.Hitbox, new Microsoft.Xna.Framework.Color(255, 50, 255), stackvalue, dramatic: crit);                       
            }
            
        }

        private static int GetPen(Player player, DamageTypeEnum damageType)
        {
            var modifiers = GetAfflictionModifiers(player, damageType);
            return modifiers.pen;
        }
    }
}