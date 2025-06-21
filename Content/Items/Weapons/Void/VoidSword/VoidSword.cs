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
    public class VoidSword : ModItem
    {

        public override void SetDefaults()
        {
            Item.damage = 1;
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

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0;
            modifiers.DisableCrit();
              modifiers.HideCombatText(); // Now truly suppresses number
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
          
            if (Main.myPlayer != player.whoAmI)
                return;
           

            if (target is null)
                return;
           
            // Calculate amplify and crit (custom system)
            float amp = AfflictionHelper.GetDebuffAmplifyMultiplier(target, player, DamageTypeEnum.Void);
            bool crit = AfflictionCritHelper.RollCrit(player, DamageTypeEnum.Void);
          
            // Get player modifiers
            var affPlayer = player.GetModPlayer<AfflictionPlayer>();
            var voidMod = affPlayer.GetDamageModifiers(DamageTypeEnum.Void);

            int baseVoidDamage = (int)(10 * voidMod.damagePercent + voidMod.flatDamage);
           
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
