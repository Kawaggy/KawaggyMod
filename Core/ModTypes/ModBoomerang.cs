using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace KawaggyMod.Core.ModTypes
{
    public abstract class ModBoomerang : ModProjectile
    {
        /// <summary>
        /// Makes it so that when this hits an entity, it stops
        /// </summary>
        public virtual bool StopOnHit => true;
        /// <summary>
        /// How long it should fly outwards before coming back
        /// </summary>
        public virtual int FlyingTime => 30;
        /// <summary>
        /// How far it needs to be to get instantly killed
        /// </summary>
        public virtual float DistanceToKill => 3000f;

        /// <summary>
        /// The usual defaults for a boomerang
        /// </summary>
        /// <param name="size"></param>
        public void Defaults(int size)
        {
            projectile.width = size;
            projectile.height = size;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 12;
            projectile.penetrate = -1;
        }
        
        /// <summary>
        /// Allows you to modify the flying sound of the boomerang. Return <see langword="false"/> to stop the sound from playing. Returns <see langword="true"/> by default
        /// </summary>
        /// <param name="type">The sound type</param>
        /// <param name="style">The sound style</param>
        /// <param name="position">The position to play the sound at</param>
        /// <param name="delay">The delay between sounds</param>
        /// <returns></returns>
        public virtual bool ModifyFlyingSound(ref int type, ref int style, ref Vector2 position, ref int delay)
        {
            return true;
        }

        /// <summary>
        /// Allows you to modify the tile hit sound of the boomerang. Return <see langword="false"/> to stop the sound from playing. Returns <see langword="true"/> by default
        /// </summary>
        /// <param name="type">The sound type</param>
        /// <param name="style">The sound style</param>
        /// <param name="position">The position to play the sound at</param>
        /// <returns></returns>
        public virtual bool ModifyTileHitSound(ref int type, ref int style, ref Vector2 position)
        {
            return true;
        }

        /// <summary>
        /// Allows you to modify the rotation to add during the flying of this boomerang. Return <see langword="false"/> to stop the rotation to be added. Returns <see langword="true"/> by default
        /// </summary>
        /// <param name="rotationToAdd">The rotation (in radians) to add to the boomerang</param>
        /// <returns></returns>
        public virtual bool ModifyFlyingRotation(ref float rotationToAdd)
        {
            return true;
        }

        /// <summary>
        /// Allows you to add dust to the boomerang
        /// </summary>
        /// <param name="state">The current state</param>
        public virtual void AddFlyingDust(float state)
        {
        }

        /// <summary>
        /// Allows you to add custom behaviour to the boomerang
        /// </summary>
        /// <param name="state">The state of the boomerang</param>
        /// <param name="player">The Player (owner) of this boomerang</param>
        public virtual void CustomBehaviour(ref float state, Player player)
        {
        }

        /// <summary>
        /// Allows you to modify what happens when the boomerang hits an entity. Return <see langword="false"/> so that nothing happens. Returns <see langword="true"/> by default
        /// </summary>
        /// <returns></returns>
        public virtual bool ModifyOnHit()
        {
            return true;
        }

        /// <summary>
        /// Allows you to modify the usual return of a boomerang. Note that the higher velocity is, the much higher detraction has to be
        /// </summary>
        /// <param name="detractionToPlayer">How demagnetic the boomerang is to you</param>
        /// <param name="velocityToAdd">The velocity to add to the boomerang</param>
        public virtual void ModifyBoomerangReturn(ref float detractionToPlayer, ref float velocityToAdd)
        {

        }

        public const int GoingOutwards = 0;
        public const int GoingToPlayer = 1;

        public override void AI()
        {
            CustomBehaviour(ref projectile.ai[0], Main.player[projectile.owner]);

            DoFlyingSound();
            AddFlyingDust(projectile.ai[0]);

            switch ((int)projectile.ai[0])
            {
                case GoingOutwards:

                    projectile.ai[1]++;
                    if (projectile.ai[1] >= FlyingTime)
                    {
                        projectile.ai[0] = GoingToPlayer;
                        projectile.ai[1] = 0f;
                    }

                    break;

                case GoingToPlayer:

                    projectile.tileCollide = false;
                    float detraction = 9f;
                    float velocityToAdd = 0.4f;

                    ModifyBoomerangReturn(ref detraction, ref velocityToAdd);

                    Vector2 vectorToPlayer = Main.player[projectile.owner].Center - projectile.Center;
                    float distanceToPlayer = vectorToPlayer.Length();

                    if (distanceToPlayer > DistanceToKill)
                        projectile.Kill();

                    distanceToPlayer = detraction / distanceToPlayer;
                    vectorToPlayer *= distanceToPlayer;

                    if (projectile.velocity.X < vectorToPlayer.X)
                    {
                        projectile.velocity.X += velocityToAdd;
                        if (projectile.velocity.X < 0f && vectorToPlayer.X > 0f)
                            projectile.velocity.X += velocityToAdd;
                    }
                    else if (projectile.velocity.X > vectorToPlayer.X)
                    {
                        projectile.velocity.X -= velocityToAdd;
                        if (projectile.velocity.X > 0f && vectorToPlayer.X < 0f)
                            projectile.velocity.X -= velocityToAdd;
                    }

                    if (projectile.velocity.Y < vectorToPlayer.Y)
                    {
                        projectile.velocity.Y += velocityToAdd;
                        if (projectile.velocity.Y < 0f && vectorToPlayer.Y > 0f)
                            projectile.velocity.Y += velocityToAdd;
                    }
                    else if (projectile.velocity.Y > vectorToPlayer.Y)
                    {
                        projectile.velocity.Y -= velocityToAdd;
                        if (projectile.velocity.Y > 0f && vectorToPlayer.Y < 0f)
                            projectile.velocity.Y -= velocityToAdd;
                    }

                    if (Main.myPlayer == projectile.owner)
                    {
                        Rectangle projectileRectangle = projectile.getRect();
                        Rectangle playerRectangle = Main.player[projectile.owner].getRect();

                        if (projectileRectangle.Intersects(playerRectangle))
                            projectile.Kill();
                    }

                    break;
            }

            DoFlyingRotation();
        }

        private void DoFlyingRotation()
        {
            float rotationToAdd = 0.4f * projectile.direction;
            if (ModifyFlyingRotation(ref rotationToAdd))
            {
                projectile.rotation += rotationToAdd;
            }
        }

        private void DoFlyingSound()
        {
            int soundType = SoundID.Item7.SoundId;
            int soundStyle = SoundID.Item7.Style;
            Vector2 soundPosition = projectile.Center;
            int delay = 8;
            if (ModifyFlyingSound(ref soundType, ref soundStyle, ref soundPosition, ref delay))
            {
                if (projectile.soundDelay == 0)
                {
                    Main.PlaySound(soundType, (int)soundPosition.X, (int)soundPosition.Y, soundStyle);
                    projectile.soundDelay = delay;
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.tileCollide)
            {
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                projectile.ai[0] = GoingToPlayer;
                projectile.velocity *= -1f;

                projectile.netUpdate = true;

                int type = SoundID.Dig;
                int style = 1;
                Vector2 position = projectile.Center;

                if (ModifyTileHitSound(ref type, ref style, ref position))
                {
                    Main.PlaySound(type, (int)position.X, (int)position.Y, style);
                }
            }
            return false;
        }

        private void DoHit()
        {
            if (ModifyOnHit())
            {
                if (StopOnHit)
                {
                    if (projectile.ai[0] == GoingOutwards)
                    {
                        projectile.velocity *= -1;
                        projectile.netUpdate = true;
                    }

                    projectile.ai[0] = GoingToPlayer;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            DoHit();
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            DoHit();
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (projectile.hostile)
            {
                DoHit();
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture(Texture);
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, texture.Size() / 2f, projectile.scale, (SpriteEffects)projectile.spriteDirection, 0f);
            return false;
        }
    }
}
