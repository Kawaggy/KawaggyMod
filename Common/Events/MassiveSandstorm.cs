using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Utilities;
using Terraria.ID;
using System.IO;
using KawaggyMod.Core.Helpers;
using Terraria.GameContent.Events;

namespace KawaggyMod.Common.Events
{
    public class MassiveSandstorm
    {
		public static bool Happening;
		public static float Severity;
		public static float IntendedSeverity;
		private static bool _effectsUp;

		public static void StartMassiveSandstorm(string key = "Mods.KawaggyMod.Common.MassiveSandstorm.Started")
		{
			if (Happening)
				return;

			if (Main.eclipse)
				return;

			if (Sandstorm.Happening)
				WorldHelper.StopSandstorm();

			Happening = true;
			ChangeSeverityIntentions();

			KawaggyHelper.NewText(text: key, color: TextureHelper.VanillaColors.Event);
			WorldHelper.SendWorldData();
		}

		public static void StopMassiveSandstorm(string key = "Mods.KawaggyMod.Common.MassiveSandstorm.Ended")
		{
			Happening = false;
			ChangeSeverityIntentions();

			KawaggyHelper.NewText(text: key, color: TextureHelper.VanillaColors.Event);
			WorldHelper.SendWorldData();
		}

        public static void WorldClear()
        {
            Happening = false;
        }

		public static void UpdateTime()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (Happening)
                {
					if (!Main.dayTime)
                    {
						StopMassiveSandstorm();
                    }
					
					if (Main.eclipse)
                    {
						StopMassiveSandstorm("Mods.KawaggyMod.Common.MassiveSandstorm.SuddenEnd");
                    }
                }

				if (Main.time == 1 && Main.rand.Next(15) == 0 && WorldHelper.CanMassiveSandstormStart && !Happening)
                {
					for (int i = 0; i < Main.maxPlayers; i++)
                    {
						if (Main.player[i].active && Main.player[i].statLifeMax2 >= 400)
                        {
							StartMassiveSandstorm();
							break;
						}
                    }
                }
			}

			UpdateSeverity();
			EmitDust();
		}

		private static void ChangeSeverityIntentions()
		{
			if (Happening)
            {
				IntendedSeverity = 0.4f + Main.rand.NextFloat();
			}
			else if (Main.rand.Next(3) == 0)
            {
				IntendedSeverity = 0f;
			}
			else
            {
				IntendedSeverity = Main.rand.NextFloat() * 0.3f;
			}

			WorldHelper.SendWorldData();
		}

		public static void SendData(BinaryWriter writer)
        {
			writer.Write(IntendedSeverity);
        }

		public static void ReceiveData(BinaryReader reader)
        {
			IntendedSeverity = reader.ReadSingle();
        }

		public static void UpdateSeverity()
		{
			int currentSeverity = Math.Sign(IntendedSeverity - Severity);

			Severity = MathHelper.Clamp(Severity + 0.003f * currentSeverity, 0f, 1f);

			int newCurrentSeverity = Math.Sign(IntendedSeverity - Severity);

			if (currentSeverity != newCurrentSeverity)
				Severity = IntendedSeverity;
		}

		public static void HandleEffectAndSky(bool toState)
		{
			if (toState != _effectsUp)
			{
				_effectsUp = toState;

				Vector2 center = Main.LocalPlayer.Center;

				if (_effectsUp)
				{
					SkyManager.Instance.Activate("Sandstorm", center);
					Filters.Scene.Activate("Sandstorm", center);
					Overlays.Scene.Activate("Sandstorm", center);
				}
				else
				{
					SkyManager.Instance.Deactivate("Sandstorm");
					Filters.Scene.Deactivate("Sandstorm");
					Overlays.Scene.Deactivate("Sandstorm");
				}
			}
		}

		public static void EmitDust(int randDustEmit = 1, float minWind = 0.1f)
		{
			if (Main.gamePaused)
				return;

			int sandTiles = Main.sandTiles + 1;
			Player player = Main.LocalPlayer;

			HandleEffectAndSky(Happening && Main.UseStormEffects);
			if (player.position.Y > Main.worldSurface * 16.0)
				return;

			if (!Happening || Main.rand.Next(randDustEmit) != 0)
				return;

			int windDirection = Math.Sign(Main.windSpeed);
			float absWindSpeed = Math.Abs(Main.windSpeed);

			if (absWindSpeed < minWind)
				return;

			float windSpeed = windDirection * MathHelper.Lerp(0.9f, 1f, absWindSpeed);

			float expectedCount = 2000f / sandTiles;

			float minValue = 3f / expectedCount;

			minValue = MathHelper.Clamp(minValue, 0.77f, 1f);

			int dustRandom = (int)expectedCount;

			float screenWidth = (float)Main.screenWidth / (float)Main.maxScreenW;
			int totalWidth = (int)(1000f * screenWidth);

			float totalLoopTimes = 20f * Severity;

			float canDustSpawn = totalWidth * (Main.gfxQuality * 0.5f + 0.5f) + totalWidth * 0.1f - Dust.SandStormCount;

			if (canDustSpawn <= 0f)
				return;

			float maxWidth = Main.screenWidth + 1000f;
			float maxHeight = Main.screenHeight;

			Vector2 screenPosition = Main.screenPosition + player.velocity;

			WeightedRandom<Color> randomColors = new WeightedRandom<Color>();

			randomColors.Add(new Color(200, 160, 20, 180), WorldHelper.Tiles.PureSandBlocks);
			randomColors.Add(new Color(103, 98, 122, 180), WorldHelper.Tiles.CorruptSandBlocks);
			randomColors.Add(new Color(135, 43, 34, 180), WorldHelper.Tiles.CrimsonSandBlocks);
			randomColors.Add(new Color(213, 196, 197, 180), WorldHelper.Tiles.HallowSandBlocks);

			float addedVelocity = MathHelper.Lerp(0.2f, 0.35f, Severity);
			float fade = MathHelper.Lerp(0.5f, 0.7f, Severity);

			float lerpValue = (minValue - 0.77f) / 0.23000002f;

			int anotherTileRand = (int)MathHelper.Lerp(1f, 10f, lerpValue);

			for (int i = 0; i < totalLoopTimes; i++)
			{
				if (Main.rand.Next(dustRandom / 4) != 0)
					continue;

				Vector2 spawnPosition = new Vector2(Main.rand.NextFloat() * maxWidth - 500f, Main.rand.NextFloat() * -50f);

				if (Main.rand.Next(3) == 0 && windDirection == 1)
                {
					spawnPosition.X = Main.rand.Next(500) - 500;
				}
				else if (Main.rand.Next(3) == 0 && windDirection == -1)
                {
					spawnPosition.X = Main.rand.Next(500) + Main.screenWidth;
				}

				if (spawnPosition.X < 0f || spawnPosition.X > Main.screenWidth)
					spawnPosition.Y += Main.rand.NextFloat() * maxHeight * 0.9f;

				spawnPosition += screenPosition;

				int spawnX = (int)spawnPosition.X / 16;
				int spawnY = (int)spawnPosition.Y / 16;

				if (Main.tile[spawnX, spawnY] == null || Main.tile[spawnX, spawnY].wall != 0)
					continue;

				for (int j = 0; j < 1; j++)
				{
					Dust dust = Main.dust[Dust.NewDust(spawnPosition, 10, 10, DustID.Sandstorm)];

					dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
					dust.velocity.Y *= dust.scale;
					dust.velocity.Y *= 0.35f;

					dust.velocity.X = windSpeed * 5f + Main.rand.NextFloat() * 1f;
					dust.velocity.X += windSpeed * fade * 20f;

					dust.fadeIn += fade * 0.2f;

					dust.velocity *= 1f + addedVelocity * 0.5f;

					dust.color = randomColors;

					dust.velocity *= 1f + addedVelocity;
					dust.velocity *= minValue;

					dust.scale = 0.9f;

					canDustSpawn -= 1f;

					if (canDustSpawn <= 0f)
						break;

					if (Main.rand.Next(anotherTileRand) != 0)
					{
						j--;
						spawnPosition += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
					}
				}

				if (canDustSpawn <= 0f)
					break;
			}
		}
	}
}
