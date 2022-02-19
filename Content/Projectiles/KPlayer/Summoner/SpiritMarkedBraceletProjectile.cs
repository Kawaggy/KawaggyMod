using KawaggyMod.Core.DataTypes;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class SpiritMarkedBraceletProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 5;
            ProjectileID.Sets.MinionShot[projectile.type] = true;

            DisplayName.SetDefault("Spirit");
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.aiStyle = -1;
            projectile.tileCollide = false;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            projectile.timeLeft = 5 * 60;

            randomSize = Main.rand.NextFloat(-0.1f, 0.25f);
        }

        public float randomSize;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            projectile.SimpleAnimation(animationSpeed: 15);
            NPCData npcData = projectile.FindClosest<NPC>(delegate (NPC npc) { return npc.lifeMax > 5 && !npc.friendly; }, true);

            if (player.HasMinionAttackTargetNPC)
            {
                if (Main.npc[player.MinionAttackTargetNPC].active)
                {
                    if (projectile.DistanceSQ(Main.npc[player.MinionAttackTargetNPC].Center) < 2000f * 2000f)
                    {
                        npcData.npc = Main.npc[player.MinionAttackTargetNPC];
                        npcData.distance = projectile.Distance(Main.npc[player.MinionAttackTargetNPC].Center);
                        npcData.hasLineOfSight = Collision.CanHitLine(projectile.Center, projectile.width, projectile.height, npcData.npc.Center, npcData.npc.width, npcData.npc.height);
                    }
                }
            }

            (int myCount, int _) = projectile.CountSameAsSelf(checkOwner: true);

            if (npcData.npc != null)
                projectile.Move(desiredPosition: npcData.npc.Center, minDistance: 20f + (5f * myCount), speed: 6f, inertia: 20f);
            else
                projectile.Move(desiredPosition: player.Center, minDistance: 60f + (5f * myCount), speed: 6f, inertia: 20f);
        }

        public override void Kill(int timeLeft)
        {
            int dustType = 20;

            for (int i = 0; i < 3; i++)
            {
                Dust.NewDustDirect(projectile.Center, projectile.width, projectile.height, dustType, Main.rand.Next(-8, 9), Main.rand.Next(-8, 9), Scale: 1f + Main.rand.NextFloat(0.5f, 1f)).noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            Rectangle frame = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
            Vector2 origin = new Vector2(0, 12);
            SpriteEffects spriteEffect = projectile.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            spriteBatch.Draw(texture, projectile.position - Main.screenPosition, frame, lightColor, projectile.rotation, origin, 1f + randomSize, spriteEffect, 0);
            return false;
        }
    }
}
