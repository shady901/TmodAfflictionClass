using AfflictionClass.Content.Buffs.RottingFleshDebuff;
using Terraria;
using Terraria.ModLoader;

namespace AfflictionClass.Content.NPCs
{
    public class AfflictionGlobalNPC : GlobalNPC
    {
        public int rottingFleshDebuffDamage = 0;
        public int rottingFleshDebuffTickTimer = 0; // Tracks DoT damage ticks
        public int rottingFleshStack = 0;
        public override bool InstancePerEntity => true;

        public override void ResetEffects(NPC npc)
        {
            // Only reset the plagueDotDamage when the debuff is NOT active
            if (!npc.HasBuff(ModContent.BuffType<RottingFleshDebuff>()))
            {
                rottingFleshDebuffDamage = 0;
                rottingFleshDebuffTickTimer = 0; // Reset DoT timer
                rottingFleshStack = 0;
            }
        }
       
    }
}
