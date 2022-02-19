using KawaggyMod.Core.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod.Core.ModTypes
{
    public abstract class ModGrenade : ModProjectile
    {
		/// <summary>
		/// Makes the projectile sticky
		/// </summary>
		public virtual bool Sticky => false;

		/// <summary>
		/// Makes the projectile have a more intense explosion sound, more dust and more gore
		/// </summary>
		public virtual bool BigExplosion => false;

		/// <summary>
		/// The size of the projectile once it explodes
		/// </summary>
		public virtual Vector2 ExplosionSize => new Vector2(128, 128);

		/// <summary>
		/// If the projectile is currently sticking
		/// </summary>
		public bool sticking;

		/// <summary>
		/// The npc whoAmI that is being stuck onto
		/// </summary>
		public int npcSticking;

		/// <summary>
		/// Allows you to modify the explosion sound. Return <see langword="false"/> to stop the sound from playing. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="soundType">The sound type</param>
		/// <param name="soundStyle">The sound style</param>
		/// <param name="soundPosition">The sound position</param>
		/// <returns></returns>
		public virtual bool ModifyExplosionSound(ref int soundType, ref int soundStyle, ref Vector2 soundPosition)
        {
			return true;
        }

		/// <summary>
		/// Allows you to modify the fire dust. Return <see langword="false"/> to stop the dust from spawning. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="type">The dust type</param>
		/// <returns></returns>
		public virtual bool ModifyFireDust(ref int type)
        {
			return true;
        }

		/// <summary>
		/// Allows you to modify the smoke dust. Return <see langword="false"/> to stop the dust from spawning. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="type">The dust type</param>
		/// <returns></returns>
		public virtual bool ModifySmokeDust(ref int type)
		{
			return true;
		}

		/// <summary>
		/// Allows you to modify the explosion smoke gore. Return <see langword="false"/> to stop the gore from spawning. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="velocityDecline">The amount of velocity the gores decline</param>
		/// <param name="type1">The first gore type</param>
		/// <param name="type2">The second gore type</param>
		/// <param name="type3">The third gore type</param>
		/// <param name="type4">The forth gore type</param>
		/// <returns></returns>
		public virtual bool ModifyGore(ref float velocityDecline, ref int type1, ref int type2, ref int type3, ref int type4)
        {
			return true;
        }

		/// <summary>
		/// Allows you to modify the bouncyness of this grenade. Return <see langword="false"/> to stop the grenade from bouncing. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="velocityReduction">The velocity it loses on each bounce</param>
		/// <returns></returns>
		public virtual bool ModifyBouncyness(ref float velocityReduction)
		{
			return true;
		}

		/// <summary>
		/// Allows you to modify the behaviour of this grenade. Return <see langword="false"/> to stop the default behaviour of the grenade. Returns <see langword="true"/> by default
		/// </summary>
		/// <returns></returns>
		public virtual bool CustomBehaviour()
		{
			return true;
		}

		/// <summary>
		/// Allows you to modify the behaviour of when this grenade explodes near the player. Return <see langword="false"/> to stop the default behaviour. Returns <see langword="true"/> by default
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool ModifyOwnerHurt(Player player)
        {
			return true;
        }

		/// <summary>
		/// Allows you to add some behaviour when this projectile gets destroyed
		/// </summary>
		public virtual void OnKill()
        {
        }

		public void Defaults(int timeLeft, int size)
        {
			projectile.width = size;
			projectile.height = size;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.tileCollide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 45;
			projectile.timeLeft = timeLeft;
			sticking = false;
			npcSticking = -1;
		}

        public override void AI()
        {
			if (CustomBehaviour())
            {
				if (Sticky)
				{
					projectile.penetrate = -1;
				}

				if (npcSticking == -1 && !sticking)
                {
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.active)
						{
							if (projectile.Hitbox.Intersects(npc.Hitbox))
							{
								npcSticking = i;
								break;
							}
						}
					}
				}

				if (projectile.timeLeft <= 3)
				{
					projectile.tileCollide = false;
					projectile.alpha = 255;
					projectile.Resize((int)ExplosionSize.X, (int)ExplosionSize.Y);
					projectile.knockBack = 8f;

					if (Main.player[projectile.owner].active && !Main.player[projectile.owner].dead && !Main.player[projectile.owner].immune && (!projectile.ownerHitCheck))
					{
						if (projectile.Hitbox.Intersects(Main.player[projectile.owner].Hitbox) && projectile.timeLeft <= 1)
						{
							if (ModifyOwnerHurt(Main.player[projectile.owner]))
                            {
								if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2))
									projectile.direction = -1;
								else
									projectile.direction = 1;

								int num4 = Main.DamageVar(projectile.damage);
								int playerIndex = projectile.owner;
								bool pvp = true;

								Main.player[projectile.owner].Hurt(PlayerDeathReason.ByProjectile(playerIndex, projectile.whoAmI), num4, projectile.direction, pvp);
							}
						}
					}
				}

				if (npcSticking == -1)
                {
					projectile.ai[0] += 1f;

					if (projectile.ai[0] > 10f)
					{
						projectile.ai[0] = 10f;
						if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
						{
							projectile.velocity.X *= 0.97f;

							if (projectile.velocity.X > -0.01f && projectile.velocity.X < 0.01f)
							{
								projectile.velocity.X = 0f;
								projectile.netUpdate = true;
							}
						}

						if (!sticking)
							projectile.velocity.Y += 0.2f;
					}

					projectile.rotation += projectile.velocity.X * 0.1f;
				}
				else
                {
					if (Main.npc[npcSticking].active)
                    {
						projectile.Center = Main.npc[npcSticking].Center - projectile.velocity * 2f;
						projectile.gfxOffY = Main.npc[npcSticking].gfxOffY;
					}
					else
                    {
						projectile.velocity *= 0f;
						projectile.velocity.Y += 0.2f;
						npcSticking = -1;
						sticking = false;
                    }
                }
			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			float velocityReduction = 0.1f;

			if (ModifyBouncyness(ref velocityReduction))
            {
				if (oldVelocity.Y > projectile.velocity.Y)
					projectile.velocity.Y = -oldVelocity.Y * velocityReduction;
				if (oldVelocity.Y < projectile.velocity.Y)
					projectile.velocity.Y = -oldVelocity.Y * velocityReduction;

				if (oldVelocity.X > projectile.velocity.X)
					projectile.velocity.X = -oldVelocity.X * velocityReduction;
				if (oldVelocity.X < projectile.velocity.X)
					projectile.velocity.X = -oldVelocity.X * velocityReduction;
			}

			if (Sticky && npcSticking == -1)
            {
				projectile.velocity = Vector2.Zero;
				sticking = true;
			}

			return false;
        }

        public override bool? CanCutTiles()
        {
			return true;
        }

        public override bool CanDamage()
        {
			if (Sticky)
            {
				return projectile.timeLeft <= 2;
            }
			return !Sticky;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (Main.expertMode)
			{
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail)
				{
					damage /= 5;
				}
			}

			if (npcSticking == -1 && Sticky)
            {
				sticking = true;
				npcSticking = target.whoAmI;
            }
        }

        public sealed override void Kill(int timeLeft)
        {
			if (BigExplosion)
            {
				int soundType = SoundID.Item62.SoundId;
				int soundStyle = SoundID.Item62.Style;
				Vector2 soundPosition = projectile.Center;

				if (ModifyExplosionSound(ref soundType, ref soundStyle, ref soundPosition))
				{
					Main.PlaySound(soundType, (int)soundPosition.X, (int)soundPosition.Y, soundStyle);
				}

				projectile.position.X += projectile.width / 2;
				projectile.position.Y += projectile.height / 2;
				projectile.width = 22;
				projectile.height = 22;
				projectile.position.X -= projectile.width / 2;
				projectile.position.Y -= projectile.height / 2;

				int smokeType = DustID.Smoke;

				if (ModifySmokeDust(ref smokeType))
				{
					for (int i = 0; i < 30; i++)
					{
						Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, smokeType, 0f, 0f, 100, default, 1.5f).velocity *= 1.4f;
					}
				}

				int fireType = DustID.Fire;

				if (ModifyFireDust(ref fireType))
				{
					for (int i = 0; i < 20; i++)
					{
						int myDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, fireType, 0f, 0f, 100, default, 3.5f);
						Dust dust = Main.dust[myDust];
						dust.noGravity = true;
						dust.velocity *= 7f;

						myDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, fireType, 0f, 0f, 100, default, 1.5f);
						dust = Main.dust[myDust];
						dust.velocity *= 3f;
					}
				}

				for (int i = 0; i < 2; i++)
				{
					float velDecline = 0.4f;
					if (i == 1)
						velDecline = 0.8f;

					int g1 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
					int g2 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
					int g3 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
					int g4 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));

					if (ModifyGore(ref velDecline, ref g1, ref g2, ref g3, ref g4))
					{
						Gore gore = Main.gore[g1];
						gore.velocity *= velDecline;
						gore.velocity += new Vector2(1f, 1f);

						gore = Main.gore[g2];
						gore.velocity *= velDecline;
						gore.velocity += new Vector2(-1f, 1f);

						gore = Main.gore[g3];
						gore.velocity *= velDecline;
						gore.velocity += new Vector2(1f, -1f);

						gore = Main.gore[g4];
						gore.velocity *= velDecline;
						gore.velocity += new Vector2(-1f, -1f);
					}
				}
			}
			else
            {
				int soundType = SoundID.Item14.SoundId;
				int soundStyle = SoundID.Item14.Style;
				Vector2	soundPosition = projectile.Center;

				if (ModifyExplosionSound(ref soundType, ref soundStyle, ref soundPosition))
				{
					Main.PlaySound(soundType, (int)soundPosition.X, (int)soundPosition.Y, soundStyle);
				}

				projectile.position.X += projectile.width / 2;
				projectile.position.Y += projectile.height / 2;
				projectile.width = 22;
				projectile.height = 22;
				projectile.position.X -= projectile.width / 2;
				projectile.position.Y -= projectile.height / 2;

				int smokeType = DustID.Smoke;

				if (ModifySmokeDust(ref smokeType))
				{
					for (int j = 0; j < 20; j++)
					{
						Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, smokeType, 0f, 0f, 100, default, 1.5f).velocity *= 1.4f;
					}
				}

				int fireType = DustID.Fire;

				if (ModifyFireDust(ref fireType))
				{
					for (int j = 0; j < 10; j++)
					{
						int fireDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, fireType, 0f, 0f, 100, default, 2.5f);

						Dust dust = Main.dust[fireDust];
						dust.noGravity = true;
						dust.velocity *= 5f;

						fireDust = Dust.NewDust(projectile.position, projectile.width, projectile.height, fireType, 0f, 0f, 100, default, 1.5f);
						dust = Main.dust[fireDust];
						dust.velocity *= 3f;
					}
				}

				float velocityDecline = 0.4f;

				int gore1 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
				int gore2 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
				int gore3 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));
				int gore4 = Gore.NewGore(projectile.position, default, Main.rand.Next(61, 64));

				if (ModifyGore(ref velocityDecline, ref gore1, ref gore2, ref gore3, ref gore4))
				{
					Gore gore = Main.gore[gore1];
					gore.velocity *= velocityDecline;
					gore.velocity += new Vector2(1f, 1f);

					gore = Main.gore[gore2];
					gore.velocity *= velocityDecline;
					gore.velocity += new Vector2(-1f, 1f);

					gore = Main.gore[gore3];
					gore.velocity *= velocityDecline;
					gore.velocity += new Vector2(1f, -1f);

					gore = Main.gore[gore4];
					gore.velocity *= velocityDecline;
					gore.velocity += new Vector2(-1f, -1f);
				}
			}

			OnKill();
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = (int)(projectile.width * 0.2f);
			height = (int)(projectile.height * 0.2f);
			fallThrough = !Sticky;
			return true;
        }
    }
}
