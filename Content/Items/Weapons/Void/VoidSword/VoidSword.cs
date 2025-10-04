using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AfflictionClass.Content.Enums;
using AfflictionClass.Content.Buffs.Void;
using AfflictionClass.Content.Helper;
using AfflictionClass.Content.Config;
using AfflictionClass.Content.Players;
using AfflictionClass.Content.Items.Types;
using Microsoft.Xna.Framework;

namespace AfflictionClass.Content.Items.Weapons.Void.VoidSword
{
    public class VoidSword : AfflictionWeapon
    {
        public override DamageTypeEnum DamageType { get; set; } = DamageTypeEnum.Void;
        public override int BaseDamage => 10;

        public override void SetDefaults()
        {
            Item.damage = 1; //not used but must be 1 for weapon to effect anything
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.knockBack = 5;
            Item.value = 1000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;      
             
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
           
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage.Base = 0;           
            modifiers.FinalDamage.Flat = -9999;
            modifiers.DisableCrit();
              modifiers.HideCombatText(); // Now truly suppresses number
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {           
            if (Main.myPlayer != player.whoAmI)
                return;    
            if (target is null)
                return;




            if (SuppressVanillaDamage&&damageDone>0)
            {
                target.life += damageDone;
            }
            // Calculate amplify and crit (custom system)
            float amp = AfflictionHelper.GetDebuffAmplifyMultiplier(target, player, DamageTypeEnum.Void);
            bool crit = AfflictionCritHelper.RollCrit(player, DamageTypeEnum.Void);
          
            // Get player modifiers
            var affPlayer = player.GetModPlayer<AfflictionPlayer>();
            var voidMod = affPlayer.GetDamageModifiers(DamageTypeEnum.Void);
            //modified base weapon damage
            int baseVoidDamage = (int)(BaseDamage * voidMod.damagePercent + voidMod.flatDamage);
           
            // Try applying void stack and checking explosion
            AfflictionHelper.AddVoidStackAndCheckExplosion(target, player, baseVoidDamage, crit, amp);
           

            // Add debuff for visual display
            target.AddBuff(ModContent.BuffType<VoidDebuff>(), AfflictionConstants.VoidStackInactivityTimeout);
           
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CopperShortsword)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
