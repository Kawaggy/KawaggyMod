using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace KawaggyMod.Common.ModPlayers
{
    //Add different ways to make the camera move, like one that always follows the desired position instead of sticking to it
    public class CameraPlayer : ModPlayer
    {
        public Vector2 screenShakeStrengthX;
        public Vector2 screenShakeStrengthY;
        public int maxScreenMoveTime;
        /// <summary>
        /// Keep in mind that 60 ticks are reserved for going towards and back.
        /// </summary>
        public int screenMoveTimer;
        public Vector2 desiredScreenPosition;

        public override void PostUpdate()
        {
            if (maxScreenMoveTime > 0 && desiredScreenPosition != Vector2.Zero)
            {
                if (screenMoveTimer >= maxScreenMoveTime)
                {
                    screenMoveTimer = 0;
                    maxScreenMoveTime = 0;
                    desiredScreenPosition = Vector2.Zero;
                }

                if (screenMoveTimer < maxScreenMoveTime)
                    screenMoveTimer++;
            }
        }

        public void SetCamera(Vector2 position, int time)
        {
            SetCameraPosition(position);
            SetCameraTime(time);
        }

        public void SetCameraTime(int time)
        {
            maxScreenMoveTime = time + (30 * 2);
        }

        public void SetCameraPosition(Vector2 position)
        {
            desiredScreenPosition = position;
        }

        public void SetScreenShakeStrength(Vector2 XStrength, Vector2 YStrength)
        {
            screenShakeStrengthX = XStrength;
            screenShakeStrengthY = YStrength;
        }

        public void SetScreenShakeStrength(Vector2 strength)
        {
            screenShakeStrengthX = strength;
            screenShakeStrengthY = strength;
        }

        public void SetScreenShakeStrength(float strength)
        {
            screenShakeStrengthX = new Vector2(strength);
            screenShakeStrengthY = new Vector2(strength);
        }

        public void SetScreenShakeSource(Vector2 position, float strength)
        {
            Vector2 vectorToPosition = position - player.Center;
            vectorToPosition.Normalize();
            vectorToPosition *= strength;

            Vector2 shakeX = Vector2.Zero;
            if (vectorToPosition.X > 0)
            {
                shakeX.Y = vectorToPosition.X;
                shakeX.X = vectorToPosition.X / 2f;
            }
            else
            {
                shakeX.X = vectorToPosition.X;
                shakeX.Y = vectorToPosition.X / 2f;
            }

            Vector2 shakeY = Vector2.Zero;
            if (vectorToPosition.Y > 0)
            {
                shakeY.Y = vectorToPosition.Y;
                shakeY.X = vectorToPosition.Y / 2f;
            }
            else
            {
                shakeY.X = vectorToPosition.Y;
                shakeY.Y = vectorToPosition.Y / 2f;
            }

            SetScreenShakeStrength(shakeX, shakeY);
        }

        public override void ModifyScreenPosition()
        {
            if (Main.myPlayer != player.whoAmI)
                return;

            if (maxScreenMoveTime > 0 && desiredScreenPosition != Vector2.Zero)
            {
                Vector2 offset = new Vector2(Main.screenWidth, Main.screenHeight) / -2f;

                if (screenMoveTimer < 30)
                {
                    Main.screenPosition = Vector2.SmoothStep(Main.LocalPlayer.Center + offset, desiredScreenPosition + offset, screenMoveTimer / 30f);
                }
                else if (screenMoveTimer > maxScreenMoveTime - 30)
                {
                    Main.screenPosition = Vector2.SmoothStep(desiredScreenPosition + offset, Main.LocalPlayer.Center + offset, (screenMoveTimer - (maxScreenMoveTime - 30)) / 30f);
                }
                else
                {
                    Main.screenPosition = desiredScreenPosition + offset;
                }
            }
            
            Main.screenPosition.X += (int)Main.rand.NextFloat(screenShakeStrengthX.X, screenShakeStrengthX.Y);
            Main.screenPosition.Y += (int)Main.rand.NextFloat(screenShakeStrengthY.X, screenShakeStrengthY.Y);

            screenShakeStrengthX *= 0.94f;
            screenShakeStrengthY *= 0.94f;

            if ((float)Math.Abs(screenShakeStrengthX.X) < 0.05f)
                screenShakeStrengthX.X = 0f;

            if ((float)Math.Abs(screenShakeStrengthX.Y) < 0.05f)
                screenShakeStrengthX.Y = 0f;

            if ((float)Math.Abs(screenShakeStrengthY.X) < 0.05f)
                screenShakeStrengthY.X = 0f;

            if ((float)Math.Abs(screenShakeStrengthY.Y) < 0.05f)
                screenShakeStrengthY.Y = 0f;
        }

        public override void OnEnterWorld(Player player)
        {
            screenShakeStrengthX = Vector2.Zero;
            screenShakeStrengthY = Vector2.Zero;
            maxScreenMoveTime = 0;
            screenMoveTimer = 0;
            desiredScreenPosition = Vector2.Zero;
        }
    }
}
