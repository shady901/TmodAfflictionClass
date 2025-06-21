using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AfflictionClass.Content.NPCs;
using AfflictionClass.Content.DamageClasses;
using static Terraria.NPC;
using AfflictionClass.Content.Players;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Helper;
using AfflictionClass.Content.Buffs.Void;

namespace AfflictionClass.Content.Buffs.DebuffAgitated
{
    public class AgitatedDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;              // Marks it as a debuff (red)
            Main.buffNoSave[Type] = true;          // Doesn’t persist on save/load
            Main.buffNoTimeDisplay[Type] = false;  // Shows timer
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.HasBuff<VoidDebuff>())
            {
                VoidDebuff.TriggerVoidExplosion(npc);
            }
            // Optional: visual effect to show it’s applied
            if (Main.rand.NextBool(4))
            {

                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
                Main.dust[dust].velocity *= 0.3f;
                Main.dust[dust].scale = 1.1f;
                Main.dust[dust].noGravity = true;
            }
        }

      
    }
}