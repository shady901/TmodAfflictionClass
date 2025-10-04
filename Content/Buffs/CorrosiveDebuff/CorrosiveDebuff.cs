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
using Microsoft.Xna.Framework;


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
            if (!player.active || player.dead) return;
            var affPlayer = player.GetModPlayer<AfflictionPlayer>();
            var corrosiveMod = affPlayer.GetDamageModifiers(DamageTypeEnum.Corrosive);

            // Roll crit using your custom helper (still gives you control over when it crits)
            bool crit = corrosiveMod.canCrit && AfflictionCritHelper.RollCrit(player, DamageTypeEnum.Corrosive);

            // Let Terraria calculate final damage with armor + apply crit internally
            NPC.HitInfo hitInfo = npc.CalculateHitInfo(damage, 0, crit, 0f);
            hitInfo.HideCombatText = true;

            // Apply damage
            npc.StrikeNPC(hitInfo);
            // Register actual post-armor damage to your DPS tracker
            player.GetModPlayer<AfflictionPlayer>().CorrosiveDps.Register(hitInfo.Damage);

            // Show custom colored combat text
            CombatText.NewText(npc.Hitbox, hitInfo.Crit ? Color.Green : Color.DarkGreen, hitInfo.Damage, dramatic: hitInfo.Crit);
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
