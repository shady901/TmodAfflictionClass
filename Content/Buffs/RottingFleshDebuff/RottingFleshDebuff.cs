using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AfflictionClass.Content.NPCs;

namespace AfflictionClass.Content.Buffs.RottingFleshDebuff
{
    public class RottingFleshDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // It’s a debuff
            Main.buffNoSave[Type] = true; // Doesn’t persist on save
            Main.buffNoTimeDisplay[Type] = false; // Show the timer
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            var aff = npc.GetGlobalNPC<AfflictionGlobalNPC>();
            
            // Handle the DoT damage ticking
            HandleDotDamage(npc, aff);

            // Handle the visual effects (dust and lighting)
            HandleVisualEffects(npc);

            // Optional: Handle splatter effect on tick
            HandleSplatterEffect(npc, buffIndex);

            // Increment the DoT tick timer
            aff.rottingFleshDebuffTickTimer++;
        }

        // Method to handle DoT damage application
        private void HandleDotDamage(NPC npc, AfflictionGlobalNPC aff)
        {
            if (aff.rottingFleshDebuffTickTimer >= 60)
            {
                if (aff.rottingFleshDebuffDamage > 0)
                {

                    npc.SimpleStrikeNPC((int)(aff.rottingFleshDebuffDamage * HandleRottingFleshStack(aff)), 0, false, default); // Apply DoT damage
                }
                aff.rottingFleshDebuffTickTimer = 0; // Reset the tick timer after damage is applied
            }
        }
        //handles the stack and returns a multiplier, every 3 will return a 20% damage increase 
        private float HandleRottingFleshStack(AfflictionGlobalNPC aff)
        {
            // Calculate the number of full stacks (every 3) 
            int fullStacks = aff.rottingFleshStack / 3;

            // Calculate the multiplier (20% per stack of 3)
            float multiplier = 1f + fullStacks * 0.2f;

            // Return the multiplier (up to a reasonable limit, say 1.6 for 3 full stacks)
            return multiplier;
        }

        // Method to handle visual effects (dust and lighting)
        private void HandleVisualEffects(NPC npc)
        {
            // 🌿 Visual: Green plague mist and spores
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.DungeonGreen);
                dust.velocity *= 0.1f;
                dust.scale = Main.rand.NextFloat(0.9f, 1.4f);
                dust.noGravity = true;
            }

            // 🌟 Light green glow pulse
            Lighting.AddLight(npc.Center, 0.1f, 0.4f, 0.1f); // Subtle sickly glow
        }

        // Method to handle optional splatter effect
        private void HandleSplatterEffect(NPC npc, int buffIndex)
        {
            if (npc.buffTime[buffIndex] % 60 == 0 && Main.rand.NextBool(2))
            {
                Gore.NewGore(npc.GetSource_FromThis(), npc.Center, npc.velocity * 0.2f, GoreID.Smoke1);
            }
        }

    }
}
