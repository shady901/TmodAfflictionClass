using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AfflictionClass.Content.NPCs;
using AfflictionClass.Content.DamageClasses;
using static Terraria.NPC;
using AfflictionClass.Content.Players;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Buffs;
using AfflictionClass.Content.Buffs.DebuffAgitated;
using static System.Net.Mime.MediaTypeNames;
using AfflictionClass.Content.Buffs.Amp;
using AfflictionClass.Content.Helper;
using AfflictionClass.Content.Config;


namespace AfflictionClass.Content.Buffs.CorrosiveDebuff
{
    public class CorrosiveDebuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            var aff = npc.GetGlobalNPC<AfflictionGlobalNPC>();

            // Only do damage server-side
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                HandleDotDamage(npc, aff);
            }

            HandleVisualEffects(npc);
            HandleSplatterEffect(npc, buffIndex);
        }
        private void HandleDotDamage(NPC npc, AfflictionGlobalNPC aff)
        {
            UpdateCorrosiveDotTimers(npc, aff);
        }
        private void UpdateCorrosiveDotTimers(NPC npc, AfflictionGlobalNPC aff)
        {
            foreach (var kv in aff.CorrosiveNPCData.CorrosiveDOTs)
            {
                int playerID = kv.Key;
                var dot = kv.Value;
                var player = Main.player[playerID];

                if (dot.TimeLeft <= 0 || !player.active)
                    continue;

                dot.TickTimer++;
                dot.TimeLeft--;

                TryTriggerCorrosiveDot(npc, dot, playerID);
            }
        }

        private void TryTriggerCorrosiveDot(NPC npc, CorrosiveDOTInstance dot, int playerID)
        {
            var player = Main.player[playerID];

            float tickRateMultiplier = AfflictionHelper.GetTickRateMultiplier(npc, player, DamageTypeEnum.Corrosive);
            int ticksNeeded = Math.Max(1, (int)(AfflictionConstants.CorrosiveBaseTickRate / tickRateMultiplier));

            if (dot.TickTimer >= ticksNeeded)
            {
                dot.TickTimer = 0;

                float amplify = AfflictionHelper.GetDebuffAmplifyMultiplier(npc, player, DamageTypeEnum.Corrosive);
                int damage = (int)(dot.BaseDamage * HandleCorrosiveStack(dot.Stack) * amplify);

                DoDotDamage(npc, damage, playerID);
            }
        }
        public static void DoDotDamage(NPC npc, int damage, int playerID)
        {
            Player player = Main.player[playerID];

            if (!player.active || player.dead)
                return;

            NPC.HitInfo hitInfo = npc.CalculateHitInfo(damage, 0, false, 0f);

            hitInfo.HideCombatText = false;

            // THIS is what actually registers DPS meter damage
            player.ApplyDamageToNPC(npc, hitInfo.Damage, hitInfo.Knockback, hitInfo.HitDirection, hitInfo.Crit);
            //registers damage dealt
            Main.player[playerID].GetModPlayer<AfflictionPlayer>().CorrosiveDps.Register(damage);
        }


        private float HandleCorrosiveStack(int stack)
        {
            int fullStacks = stack / AfflictionConstants.CorrosiveStackDamageRatio;
            return 1f + fullStacks * AfflictionConstants.CorrosiveStackMultiplier;
        }

        private void HandleVisualEffects(NPC npc)
        {
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.DungeonGreen);
                dust.velocity *= 0.1f;
                dust.scale = Main.rand.NextFloat(0.9f, 1.4f);
                dust.noGravity = true;
            }

            Lighting.AddLight(npc.Center, 0.1f, 0.4f, 0.1f);
        }

        private void HandleSplatterEffect(NPC npc, int buffIndex)
        {
            if (npc.buffTime[buffIndex] % 60 == 0 && Main.rand.NextBool(2))
            {
                Gore.NewGore(npc.GetSource_FromThis(), npc.Center, npc.velocity * 0.2f, GoreID.Smoke1);
            }
        }
    }
}
