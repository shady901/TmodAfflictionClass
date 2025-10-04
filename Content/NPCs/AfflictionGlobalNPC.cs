using AfflictionClass.Content.Buffs.CorrosiveDebuff;
using AfflictionClass.Content.Config;
using AfflictionClass.Content.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AfflictionClass.Content.NPCs
{
    public class AfflictionGlobalNPC : GlobalNPC
    {
       
        public override bool InstancePerEntity => true;

        public CorrosiveNPCData CorrosiveNPCData = new CorrosiveNPCData();
        public VoidNPCData voidNPCData = new VoidNPCData();

        public override void ResetEffects(NPC npc)
        {
            var corrosiveData = CorrosiveNPCData;
            var voidData = voidNPCData;

            // Remove expired corrosive stacks
            foreach (var kv in corrosiveData.CorrosiveDOTs.ToList())
            {
                if (kv.Value.TimeLeft <= 0)
                    corrosiveData.CorrosiveDOTs.Remove(kv.Key);
            }

            // Remove expired void stacks
            foreach (var kv in voidData.VoidDOTs.ToList())
            {
                var dot = kv.Value;
                if (Main.GameUpdateCount - dot.LastHitTime > AfflictionConstants.VoidStackInactivityTimeout)
                {
                    voidData.VoidDOTs.Remove(kv.Key);

                    if (AfflictionConstants.DebugShowCombatText)
                    {
                        CombatText.NewText(npc.Hitbox, Color.Gray, "Void expired", dramatic: true);
                    }

                    // 💨 Purple poof effect
                    for (int i = 0; i < 6; i++)
                    {
                        int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame);
                        Main.dust[dust].velocity *= 0.5f;
                        Main.dust[dust].scale = 1.1f;
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
        }


    }
    public class CorrosiveDOTInstance
    {
        public int OwnerPlayerID;
        public int BaseDamage;
        public int TickTimer;
        public int Stack;
        public int TimeLeft;

        public CorrosiveDOTInstance(int playerID, int baseDamage)
        {
            OwnerPlayerID = playerID;
            BaseDamage = baseDamage;
            TickTimer = 0;
            Stack = 1;
            TimeLeft = 300; // 5 seconds = 300 ticks
        }
    }

    public class CorrosiveNPCData
    {
        public Dictionary<int, CorrosiveDOTInstance> CorrosiveDOTs = new();

        public void SetDefault()
        {
            CorrosiveDOTs.Clear();
        }
    }

    public class VoidNPCData
    {
        public Dictionary<int, VoidDOTInstance> VoidDOTs = new();

        public void SetDefault()
        {
            VoidDOTs.Clear();
        }
    }
    public class VoidDOTInstance
    {
        public int PlayerID;
        public ulong LastHitTime;
        public List<VoidStackEntry> Stacks = new();

        public VoidDOTInstance(int playerID, int baseDamage, bool crit, float amp,  int npcDefence, int voidPen)
        {
            PlayerID = playerID;
            AddStack(baseDamage, crit, amp,  npcDefence, voidPen);
        }

        public int AddStack(int baseDamage, bool crit, float amp,  int npcDefence, int voidPen)
        {
            var result = new VoidStackEntry(baseDamage, crit, amp, npcDefence,voidPen);
            Stacks.Add(result);
            LastHitTime = Main.GameUpdateCount;
            return result.FinalDamage;
        }

        public int GetTotalDamage()
        {
            int total = 0;
            foreach (var stack in Stacks)
                total += stack.FinalDamage;
            return total;
        }
    }

    public class VoidStackEntry
    {
        public int FinalDamage;

        public VoidStackEntry(int baseDamage, bool crit, float amp, int npcDefence, int voidPen)
        {
            float dmg = baseDamage * amp;
            if (crit)
                dmg *= 2;
           dmg = AfflictionHelper.ApplyArmorToValue(npcDefence, voidPen, (int)dmg);
          

            FinalDamage = Math.Max(1, (int)dmg);
        }
    }

}

