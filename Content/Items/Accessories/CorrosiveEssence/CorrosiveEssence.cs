using AfflictionClass.Content.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AfflictionClass.Content.Enums;

namespace AfflictionClass.Content.Items.Accessories.CorrosiveEssence
{
    public class CorrosiveEssence : ModItem
    {
        
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue; // Green rarity
            Item.value = Item.buyPrice(silver: 50); // Item price
            Item.accessory = true; // Makes it an accessory item
        }

        // Apply the plague damage bonus when equipped
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Add 10% to the player's plague damage
            player.GetModPlayer<AfflictionPlayer>().corrosiveDamageModifiers.damagePercent += 0.1f; // 10% increase in corrosive damage
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DirtBlock, 1);  // Example ingredient
            recipe.AddTile(TileID.WorkBenches);  // Crafting station
            recipe.Register();
        }
    }
}
