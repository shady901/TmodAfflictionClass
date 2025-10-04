using AfflictionClass.Content.Config;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Helper;
using AfflictionClass.Content.NPCs;
using AfflictionClass.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Buffs.Void
{
    public class VoidDebuff:ModBuff
    {
        public override void SetStaticDefaults()
        {

            Main.debuff[Type] = true; // Marks it as a debuff
            Main.buffNoSave[Type] = true; // Doesn’t persist on save
            Main.buffNoTimeDisplay[Type] = false; // Shows a timer
        }




        public static void TriggerVoidExplosion(NPC npc)
        {
            var aff = npc.GetGlobalNPC<AfflictionGlobalNPC>();

            foreach (var kv in aff.voidNPCData.VoidDOTs.ToList()) // clone to safely modify
            {
                int playerID = kv.Key;
                var dot = kv.Value;

                // ✅ Skip explosion if stack has expired due to inactivity
                if (Main.GameUpdateCount - dot.LastHitTime > AfflictionConstants.VoidStackInactivityTimeout)
                {
                    aff.voidNPCData.VoidDOTs.Remove(playerID);
                    continue;
                }

                var player = Main.player[playerID];

                int damage = dot.GetTotalDamage();
                float amp = AfflictionHelper.GetDebuffAmplifyMultiplier(npc, player, DamageTypeEnum.Void);
                damage = (int)(damage * amp);

                // Apply damage 
                DoVoidDotDamage(npc, damage, playerID);

                // Visual effects for feedback
                for (int i = 0; i < 10; i++)
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Shadowflame);

                // Clear stacks after explosion
                aff.voidNPCData.VoidDOTs.Remove(playerID);
            }
        }
        public static void DoVoidDotDamage(NPC npc, int damage, int playerID)
        {
            Player player = Main.player[playerID];
            if (!player.active || player.dead)
                return;

            NPC.HitInfo hitInfo = new NPC.HitInfo
            {
                Damage = damage, // YOUR pre-processed value (already includes armor, amp, crit, etc)
                Knockback = 0f,
                HitDirection = npc.Center.X > player.Center.X ? 1 : -1,
                Crit = false,
                HideCombatText = true
            };

            npc.StrikeNPC(hitInfo);

            // 💜 Purple custom damage text
            CombatText.NewText(npc.Hitbox, new Microsoft.Xna.Framework.Color(255, 50, 255), "Void Burst: " + hitInfo.Damage.ToString(), dramatic: true);
            SoundEngine.PlaySound(SoundID.Item74, npc.position); // spooky explosion sound

        }
    }

}
