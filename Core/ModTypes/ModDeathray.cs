using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace KawaggyMod.Core.ModTypes
{
    public abstract class ModDeathray : ModProjectile
    {
        public enum EntityContext // maybe should be in its own file? could be used for other things
        {
            Projectile,
            NPC,
            Player,
            /// <summary>
            /// You are on your own.
            /// </summary>
            Entity
        }

        public abstract EntityContext Context { get; }

        public Vector2 offset;
        public void AddOffset(Vector2 offset)
        {
            this.offset += offset;
        }

        public void SetOffset(Vector2 offset)
        {
            this.offset = offset;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(offset.X);
            writer.Write(offset.Y);
            writer.Write(topFrame);
            writer.Write(middleFrame);
            writer.Write(bottomFrame);
            writer.Write(topFrameCounter);
            writer.Write(middleFrameCounter);
            writer.Write(bottomFrameCounter);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            offset.X = reader.ReadSingle();
            offset.Y = reader.ReadSingle();
            topFrame = reader.ReadInt32();
            middleFrame = reader.ReadInt32();
            bottomFrame = reader.ReadInt32();
            topFrameCounter = reader.ReadInt32();
            middleFrameCounter = reader.ReadInt32();
            bottomFrameCounter = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }

        public override void SetDefaults()
        {
            projectile.hide = true;
            offset = Vector2.Zero;
            moreScale = 1f;
            base.SetDefaults();
        }

        /// <summary>
        /// Return <see langword="true"/> if you want to manually set the position. Returns <see langword="false"/> by default
        /// </summary>
        /// <returns></returns>
        public virtual bool SetPosition() { return false; }
        /// <summary>
        /// Use this to play a sound when the Deathray is spawned
        /// </summary>
        public virtual void PlaySound() { }

        /// <summary>
        /// The max distance this deathray is allowed to go
        /// </summary>
        public virtual float MaxDistance => 2400f;

        public virtual void ConstantDust(Vector2 position) { }

        public virtual void RandomDust(Vector2 position) { }

        public virtual bool HasLight => false;
        public virtual Color LightColor => Color.Transparent;

        public virtual int AnimationSpeed => 5;

        int maxTimeLeft;
        bool FirstTick = true;

        public int topFrame;
        public int middleFrame;
        public int bottomFrame;

        public int topFrameCounter;
        public int middleFrameCounter;
        public int bottomFrameCounter;

        public int entityOwner;
        public float moreScale;

        public virtual void ExtraMovement() { }

        public virtual void Animation() { }
        public override void AI()
        {
            if (FirstTick)
            {
                maxTimeLeft = projectile.timeLeft;
                FirstTick = false;
                topFrame = 0;
                middleFrame = 0;
                bottomFrame = 0;
                topFrameCounter = 0;
                middleFrameCounter = 0;
                bottomFrameCounter = 0;
            }

            Vector2? throwaway = null;

            projectile.timeLeft = 2;

            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            switch(Context)
            {
                case EntityContext.NPC:
                    if (!SetPosition())
                    {
                        projectile.Center = Main.npc[entityOwner].Center + offset - new Vector2(projectile.width, projectile.height) / 2f;
                    }
                    break;

                case EntityContext.Player:
                    if (!SetPosition())
                    {
                        projectile.Center = Main.player[projectile.owner].Center + offset - new Vector2(projectile.width, projectile.height) / 2f;
                    }
                    break;

                case EntityContext.Projectile:
                    if (!SetPosition())
                    {
                        projectile.Center = Main.projectile[entityOwner].Center + offset - new Vector2(projectile.width, projectile.height) / 2f;
                    }
                    break;

                case EntityContext.Entity:
                    SetPosition();
                    break;
            }

            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
                projectile.velocity = -Vector2.UnitY;

            if (projectile.localAI[0] == 0f)
                PlaySound();

            projectile.localAI[0]++;

            if (projectile.localAI[0] >= maxTimeLeft)
            {
                projectile.Kill();
                return;
            }

            projectile.scale = (float)Math.Sin(projectile.localAI[0] * ((float)Math.PI / maxTimeLeft)) * moreScale;

            if (projectile.scale > moreScale)
                projectile.scale = moreScale;

            float rotation = projectile.velocity.ToRotation();
            rotation += projectile.ai[0];

            projectile.rotation = rotation - MathHelper.PiOver2;
            projectile.velocity = rotation.ToRotationVector2();

            float bigSize = projectile.width > projectile.height ? projectile.width : projectile.height;

            Vector2 samplingPoint = projectile.Center;

            if (throwaway.HasValue)
                samplingPoint = throwaway.Value;

            float[] array = new float[3];
            Collision.LaserScan(samplingPoint, projectile.velocity, bigSize * projectile.scale, MaxDistance, array);

            float num0 = 0f;
            for (int i = 0; i < array.Length; i++)
            {
                num0 += projectile.tileCollide ? array[i] : MaxDistance;
            }

            num0 /= array.Length;

            float amount = 0.5f;

            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num0, amount);

            Vector2 position = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);

            ConstantDust(position);
            RandomDust(position);
            ExtraMovement();

            Animation();

            if (!HasLight)
            {
                DelegateMethods.v3_1 = new Vector3(LightColor.R, LightColor.G, LightColor.B);

                Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)bigSize * projectile.scale, DelegateMethods.CastLight);
            }
        }

        public override void PostAI()
        {
            projectile.hide = false;
            base.PostAI();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (projectile.width > projectile.height ? projectile.width : projectile.height), ref collisionPoint))
                return true;
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void CutTiles()
        {
            base.CutTiles();

            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)(projectile.width > projectile.height ? projectile.width : projectile.height), new Utils.PerLinePoint(DelegateMethods.CutTiles));
        }

        public bool Draw(SpriteBatch spriteBatch, Color color)
        {
            if (projectile.velocity == Vector2.Zero)
                return false;

            Texture2D topTexture = ModContent.GetTexture(Texture + "_Top_" + topFrame);
            Texture2D middleTexture = ModContent.GetTexture(Texture + "_Middle_" + middleFrame);
            Texture2D endTexture = ModContent.GetTexture(Texture + "_Bottom_" + bottomFrame);

            float num0 = projectile.localAI[1];

            spriteBatch.Draw(topTexture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, topTexture.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);

            num0 -= (topTexture.Height / 2 + endTexture.Height) * projectile.scale;

            Vector2 newCenter = projectile.Center;
            newCenter += projectile.velocity * projectile.scale * topTexture.Height / 2f;
            if (num0 > 0f)
            {
                float num1 = 0f;
                Rectangle middleTextureFrame = middleTexture.Frame();
                while (num1 + 1f < num0)
                {
                    if (num0 - num1 < middleTextureFrame.Height)
                    {
                        middleTextureFrame.Height = (int)(num0 - num1);
                    }

                    spriteBatch.Draw(middleTexture, newCenter - Main.screenPosition, new Rectangle?(middleTextureFrame), color, projectile.rotation, new Vector2((middleTextureFrame.Width / 2f), 0f), projectile.scale, SpriteEffects.None, 0f);

                    num1 += middleTextureFrame.Height * projectile.scale;
                    newCenter += projectile.velocity * middleTextureFrame.Height * projectile.scale;

                    middleTextureFrame.Y += (projectile.width > projectile.height ? projectile.width : projectile.height);
                    if (middleTextureFrame.Y + middleTextureFrame.Height > middleTexture.Height)
                    {
                        middleTextureFrame.Y = 0;
                    }
                }
            }

            spriteBatch.Draw(endTexture, newCenter - Main.screenPosition, null, color, projectile.rotation, endTexture.Frame().Top(), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return Draw(spriteBatch, new Color(255, 255, 255, 0) * 0.9f);
        }
    }
}
