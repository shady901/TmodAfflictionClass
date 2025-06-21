using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Buffs.Amp
{
    public class AmpDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            
            Main.debuff[Type] = true; // Marks it as a debuff
            Main.buffNoSave[Type] = true; // Doesn’t persist on save
            Main.buffNoTimeDisplay[Type] = false; // Shows a timer
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            Lighting.AddLight(npc.Center, new Vector3(0.6f, 0.2f, 0.8f)); // deeper arcane violet

            if (Main.rand.NextBool(3))
            {
                int dustType = Main.rand.NextBool() ? DustID.MagicMirror : DustID.PortalBoltTrail;
                var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, dustType);
                dust.velocity *= 0.25f;
                dust.scale = Main.rand.NextFloat(1f, 1.5f);
                dust.noGravity = true;
                dust.color = new Color(200, 80, 255); // amplified purple hue
            }
        }
    }
}
