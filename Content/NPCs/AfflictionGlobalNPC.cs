using AfflictionClass.Content.Buffs.CorrosiveDebuff;
using Terraria;
using Terraria.ModLoader;

namespace AfflictionClass.Content.NPCs
{
    public class AfflictionGlobalNPC : GlobalNPC
    {
        
        public override bool InstancePerEntity => true;
        public CorrosiveNPCData CorrosiveNPCData = new CorrosiveNPCData();
        public override void ResetEffects(NPC npc)
        {
            // Only reset the corrosive when the debuff is NOT active
            if (!npc.HasBuff(ModContent.BuffType<CorrosiveDebuff>()))
            {
                CorrosiveNPCData.SetDefault();
            }
        }
       
    }
    public class CorrosiveNPCData
    {
        public int corrosiveDebuffDamage = 0;
        public int corrosiveDebuffTickTimer = 0; // Tracks DoT damage ticks
        public int corrosiveStack = 0;

        public void SetDefault()
        {
            corrosiveDebuffDamage = 0;
            corrosiveDebuffTickTimer = 0; // Reset DoT timer
            corrosiveStack = 0;

        }
    }
}
