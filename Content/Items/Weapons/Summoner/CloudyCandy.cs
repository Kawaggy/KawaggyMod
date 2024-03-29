﻿using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Interfaces;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Summoner
{
    public class CloudyCandy : ModItem, ICustomizable
    {
        public int Count => CustomizationManager.cloudSummon.cache.Count;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;

            DisplayName.SetDefault("Cloudy Candy");
            Tooltip.SetDefault("Summons a small cute cloud to protect you");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.damage = 9;
            item.knockBack = 3f;
            item.mana = 10;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(silver: 8);
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<CloudSummonBuff>();
            item.shoot = ModContent.ProjectileType<CloudSummon>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 15);
            recipe.AddIngredient(ItemID.FallenStar);
            recipe.AddRecipeGroup(RecipeManager.AnyWood, 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;

            (Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI)].modProjectile as CloudSummon).faceFrame = Main.rand.Next(0, CustomizationManager.cloudSummon.FrameCount);
            return false;
        }
    }
}
