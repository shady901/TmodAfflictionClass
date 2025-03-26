using AfflictionClass.Content.DamageClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AfflictionClass.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using AfflictionClass.Content.Players;
using System.Security.AccessControl;
using AfflictionClass.Content.Buffs.RottingFleshDebuff;

namespace AfflictionClass.Content.Projectiles.FlaskAttemptProjectile
{
    public class CorrosiveVialProjectile :ModProjectile
    {
        
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = 2; // Like a thrown flask/grenade
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.DamageType = ModContent.GetInstance<AfflictionDamageClass>();
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = 1;
          //  Main.NewText($"[Debug] Spawned {Projectile.Name} with forced damage = {Projectile.damage}");

        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Flat = -99999f; // Nullify contact damage
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        //    Main.NewText("Hit registered!", Color.LightGreen);

        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.1f, 0.25f, 0.1f); // Soft toxic green
            if (Main.rand.NextBool(2)) // Emit every other frame
            {
                Dust trail = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DungeonGreen);
                trail.scale = Main.rand.NextFloat(1f, 1.4f);
                trail.velocity = Projectile.velocity * -0.2f; // Trails slightly behind
                trail.noGravity = true;
                trail.fadeIn = 1.2f;
            }
        }

        //on kill as in the projectile ends
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            float plagueBonus = player.GetModPlayer<AfflictionPlayer>().plagueDamage;

            int scaledDotDamage = (int)(Projectile.originalDamage * (1f + plagueBonus));
            float aoeRadius = 100f;


            //actual application
            ApplyAOEDebuff(scaledDotDamage, aoeRadius);



            //visuals
            SpawnImpactVisuals();
            SpawnPlagueCloud(aoeRadius);
        }
        private void ApplyAOEDebuff(int dotDamage, float radius)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
                if (target.active && !target.friendly && !target.dontTakeDamage)
                {
                    float dist = Vector2.Distance(target.Center, Projectile.Center);
                    if (dist <= radius)
                    {
                        if (target.HasBuff<RottingFleshDebuff>())
                        {
                            if (target.GetGlobalNPC<AfflictionGlobalNPC>().rottingFleshStack < 10)
                            {
                                target.GetGlobalNPC<AfflictionGlobalNPC>().rottingFleshStack += 1;
                            }
                        } 
                        
                        target.GetGlobalNPC<AfflictionGlobalNPC>().rottingFleshDebuffDamage = dotDamage;
                        target.AddBuff(ModContent.BuffType<RottingFleshDebuff>(), 300);
                        
                        target.SimpleStrikeNPC(1, 0, false, default);
                    }
                }
            }
        }
       
        private void SpawnImpactVisuals()
        {
            for (int i = 0; i < 12; i++)
            {
                Dust.NewDust(Projectile.Center, 10, 10, DustID.Poisoned);
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
        private void SpawnPlagueCloud(float radius)
        {
            int plagueDustAmount = 60;

            for (int i = 0; i < plagueDustAmount; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(radius, radius);
                Vector2 pos = Projectile.Center + offset;

                Dust dust = Dust.NewDustPerfect(pos, DustID.DungeonGreen);
                dust.noGravity = true;
                dust.velocity = -offset.SafeNormalize(Vector2.Zero) * 1.5f;
                dust.scale = Main.rand.NextFloat(1.2f, 1.6f);
                dust.fadeIn = 1.1f;
            }
        }

    }
}
