using AfflictionClass.Content.DamageClasses;
using AfflictionClass.Content.Items.Types;
using AfflictionClass.Content.Projectiles.FlaskAttemptProjectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AfflictionClass.Content.Items.flaskattempt
{
    public class CorrosiveVial:AfflictionWeapon
    {
        public override bool IsPlague => true;

        public override void SetDefaults()
        {
            Item.damage = 10;
            Item.DamageType = ModContent.GetInstance<AfflictionDamageClass>();
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ModContent.ProjectileType<CorrosiveVialProjectile>();
            Item.UseSound = SoundID.Item106;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing; // 1
            Item.useTime = 40;        // Attacks every 30 ticks
            Item.useAnimation = 40;   // Fast arm motion
            Item.reuseDelay = Item.useTime - Item.useAnimation;      // Optional buffer
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
