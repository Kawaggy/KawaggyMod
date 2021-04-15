using KawaggyMod.Content.Buffs.Summoner;
using KawaggyMod.Core;
using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Content.Projectiles.KPlayer.Summoner
{
    public class Bouldy : KProjectile
    {
        public override string Texture => Assets.Projectiles.Player + "Summoner/Bouldy";

        public override void SetStaticDefaults()
        {
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 26;
            projectile.SetupMinion();
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Shatter, (int)projectile.position.X, (int)projectile.position.Y, 1, 0.25f);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Stone, projectile.velocity.X, projectile.velocity.Y).velocity *= 0.9f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDustDirect(target.position, target.width, target.height, DustID.Stone, projectile.velocity.X, projectile.velocity.Y).velocity *= 0.9f;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;
        public override bool MinionContactDamage() => true;
        public override bool? CanCutTiles() => false;

        public float fireIntensity;
        public float rotation;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor) //Draw fire
        {
            Texture2D fireTexture = ModContent.GetTexture(Assets.ExtraTextures + "FireTexture");
            Rectangle sourceRectangle = new Rectangle(0, 0, fireTexture.Width, fireTexture.Height);
            Vector2 origin = new Vector2(17, 21);
            Vector2 position = projectile.Center;
            position -= Main.screenPosition;
            Color color = new Color(153, 84, 0, 0);
            color *= fireIntensity;
            spriteBatch.Draw(fireTexture, position, sourceRectangle, color, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        public bool FirstTick = true;
        public int defDamage;
        public override bool PreAI()
        {
            if (FirstTick)
            {
                defDamage = projectile.damage;
                projectile.ai[1] = 1;
            }

            FirstTick = false;
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hops);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hops = reader.ReadInt32();
        }

        int hops = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            
            if (player.dead || !player.active)
                player.ClearBuff(ModContent.BuffType<BouldySummonBuff>());

            if (player.HasBuff(ModContent.BuffType<BouldySummonBuff>()))
                projectile.timeLeft = 2;
            
            Vector2 idlePosition = player.Center;
            idlePosition.X += (10 + projectile.minionPos * 40) * -player.direction;

            Vector2 vectorToIdlePosition = idlePosition - projectile.Center;
            float distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == projectile.owner && distanceToIdlePosition > 1000f)
            {
                projectile.scale -= 0.5f;
                Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Stone, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)).velocity *= 0.9f;

                if (projectile.scale <= 0.1f)
                {
                    Vector2 teleportationPosition = player.Center;
                    teleportationPosition.Y -= 16f;
                    projectile.position = teleportationPosition;
                    projectile.velocity *= 0.1f;
                    projectile.netUpdate = true;
                }
                return;
            }
            else
            {
                if (projectile.scale != 1)
                {
                    Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Stone, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)).velocity *= 0.9f;
                    projectile.scale += 0.05f;
                }
                if (projectile.scale > 1)
                    projectile.scale = 1;
            }

            if (projectile.IsInsideTile() && !player.IsInsideTile())
            {
                if (projectile.scale > 0)
                    projectile.scale -= 0.10f;

                if (projectile.scale < 0)
                    projectile.scale = 0;

                Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Stone, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)).velocity *= 0.9f;

                if (projectile.scale <= 0.10f)
                {
                    Vector2 position = player.Center;
                    position.Y -= 16f;
                    projectile.position = position;
                    projectile.velocity *= 0.1f;
                    projectile.netUpdate = true;
                }
                return;
            }

            projectile.velocity.Y += ProjectileHelper.gravity;
            projectile.CapSpeed(-16f, -16f, 16f, 80f);
            projectile.velocity.X *= 0.92f;

            float distanceFromTarget = 700f;
            Vector2 targetCenter = projectile.position;
            bool foundTarget = false;

            if (player.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[player.MinionAttackTargetNPC];
                float between = Vector2.DistanceSquared(npc.Center, projectile.Center);
                if (between < ((distanceFromTarget * 2) * (distanceFromTarget * 2)))
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                int myNPC = projectile.TargetClosestEnemy(true);
                if (myNPC != -1)
                {
                    NPC npc = Main.npc[myNPC];
                    float between = Vector2.DistanceSquared(npc.Center, projectile.Center);
                    if (between < distanceFromTarget * distanceFromTarget)
                    {
                        distanceFromTarget = between;
                        targetCenter = npc.Center;
                        foundTarget = true;
                    }
                }
            }

            float speed = 20f;
            float inertia = 60f;
            projectile.ai[0] = -5.5f + MathHelper.Lerp(-0.5f, 0.5f, Main.rand.NextFloat()); //jump height

            float volume = MathHelper.Lerp(0.25f, 0.50f, Main.rand.NextFloat());

            float pitch = MathHelper.Lerp(-0.25f, 0.25f, Main.rand.NextFloat());

            fireIntensity = (projectile.velocity.Length() - 6f) / 16f;
            rotation = projectile.velocity.ToRotation() + (float)MathHelper.PiOver2;
            fireIntensity = MathHelper.Clamp(fireIntensity, 0f, 1f);

            projectile.damage = (int)(defDamage * (1 + fireIntensity));
            projectile.tileCollide = true;

            int i = (int)(projectile.position.X + (projectile.width / 2)) / 16;
            int j = (int)(projectile.position.Y + (projectile.height / 2)) / 16;

            if (foundTarget)
            {
                Vector2 direction = targetCenter - projectile.Center;
                if ((direction.Y > 0) && (Main.tile[i, j + 1].type == TileID.Platforms))
                {
                    projectile.tileCollide = false;
                }
                else if (distanceFromTarget > 5f)
                {
                    if (WorldGen.SolidTile(i, j + 1) || Main.tile[i, j + 1].type == TileID.Platforms || Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() > 0)
                    {
                        if (-direction.Y / 16 > 4)
                        {
                            hops++;
                            switch(hops)
                            {
                                case 1:
                                    DoHop(projectile.ai[0]);
                                    PlaySound(volume, pitch);
                                    break;
                                case 2:
                                    DoHop(projectile.ai[0] * 1.25f);
                                    PlaySound(volume, pitch);
                                    break;
                                case 3:
                                    DoHop(projectile.ai[0] * 2.75f);
                                    PlaySound(volume, pitch, true);
                                    hops = 0;
                                    break;
                            }
                        }
                    }

                    direction.Normalize();
                    direction *= speed;
                    projectile.velocity.X = (projectile.velocity.X * (inertia - 1) + direction.X) / inertia;
                }
            }
            else
            {
                if (distanceToIdlePosition > 200f)
                {
                    speed = 40f;
                    inertia = 120f;
                }

                if (distanceToIdlePosition > 35f)
                {
                    if ((vectorToIdlePosition.Y > 0) && (Main.tile[i, j + 1].type == TileID.Platforms))
                    {
                        projectile.tileCollide = false;
                    }
                    else if(WorldGen.SolidTile(i, j + 1) || Main.tile[i, j + 1].type == TileID.Platforms || Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() > 0)
                    {
                        if (-vectorToIdlePosition.Y / 16 > 4)
                        {
                            hops++;
                            switch(hops)
                            {
                                case 1:
                                    DoHop(projectile.ai[0]);
                                    PlaySound(volume, pitch);
                                    break;
                                case 2:
                                    DoHop(projectile.ai[0] * 1.25f);
                                    PlaySound(volume, pitch);
                                    break;
                                case 3:
                                    DoHop(projectile.ai[0] * 2.75f);
                                    PlaySound(volume, pitch, true);
                                    hops = 0;
                                    break;
                            }
                        }
                        else
                        {
                            hops = 0;
                            DoHop(projectile.ai[0]);
                            PlaySound(volume, pitch);
                        }
                    }

                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    projectile.velocity.X = (projectile.velocity.X * (inertia - 1) + vectorToIdlePosition.X) / inertia;
                }
                else
                {
                    if (projectile.ai[1] > 0)
                    {
                        if (WorldGen.SolidTile(i, j + 1) || Main.tile[i, j + 1].type == TileID.Platforms || Main.tile[i, j + 1].halfBrick() || Main.tile[i, j + 1].slope() > 0)
                        {
                            PlaySound(volume, pitch);
                            projectile.ai[1] = 0;
                        }
                    }
                }
            }

            if (fireIntensity > 0.15f)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 127);
                Main.dust[dust].velocity *= 0.8f;
                Main.dust[dust].alpha = (int)(255 - (fireIntensity * 255));
            }

            projectile.rotation += projectile.velocity.X * 0.1f;
        }

        public void PlaySound(float volume, float pitch, bool forceExplosion = false)
        {
            if (projectile.IsInsideTile())
                return;

            if (fireIntensity < 0.25f && !forceExplosion)
            {
                Main.PlaySound(SoundID.Tink, (int)projectile.position.X, (int)projectile.position.Y, 1, volume, pitch);
                for (int i = 0; i < 3; i++)
                {
                    Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 1, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5)).velocity *= 0.9f;
                }
            }
            else
            {
                for (int a = 0; a < 3; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 16, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5));
                    Main.dust[dust].velocity *= 0.9f;
                    Main.dust[dust].scale = 1.25f;
                }
                for (int a = 0; a < 5; a++)
                {
                    int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 127);
                    Main.dust[dust].velocity *= 0.8f;
                    Main.dust[dust].alpha = (int)(255 - (fireIntensity * 255));
                }
                Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 62, volume, pitch);
            }
        }

        public void DoHop(float jumpHeight)
        {
            projectile.velocity.Y = jumpHeight;
            projectile.ai[1] = 1;
        }
    }
}
