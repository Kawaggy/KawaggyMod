using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Content.Projectiles.KPlayer.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Items.Weapons.Summoner
{
    public class CloudyCandy : ModItem, ICustomizable
    {
        public int Count => 0;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
            ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.damage = 24;
            item.knockBack = 3f;
            item.mana = 10;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.buyPrice(silver: 40);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item44;
            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<CloudSummonBuff>();
            item.shoot = ModContent.ProjectileType<CloudSummon>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBlade);
            recipe.AddIngredient(ItemID.Shiverthorn, 5);
            recipe.AddIngredient(ItemID.Bone, 5);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            position = Main.MouseWorld;

            //Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI)].frame = Main.rand.Next(0, CustomizationManager.iceSwords.FrameCount);
            (Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI)].modProjectile as CloudSummon).faceFrame = Main.rand.Next(0, 10);
            return false;
        }
    }
}
