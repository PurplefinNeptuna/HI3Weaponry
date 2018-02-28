using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace hiweapons.Projectiles
{

	// Using custom drawing, dust effects, and custom collision checks for tiles
	public class BronyaLaser : ModProjectile
	{
		// The distance laser from the player center
		private float MoveDistance = 54f;
		
		// The maximum laser distance
		private float MaxDistance = 720f;

		// Weapon type are saved in ai0
		// This can be used for special skills for many beam cannon later
		// List:
		// 1f = Project Bunny 19C
		// 2f = Cathode Type-09
		// 3f = MiG-7
		// 4f = Devourer's Laser

		// The actual distance is stored in the ai1 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance
		{
			get { return projectile.ai[1]; }
			set { projectile.ai[1] = value; }
		}


		// Get lasercharge
		public int Charge
		{
			get
			{
				Hiplayers modPlayer = Main.player[projectile.owner].GetModPlayer<Hiplayers>(mod);
				return modPlayer.laserCharge;
			}
			set
			{
				Hiplayers modPlayer = Main.player[projectile.owner].GetModPlayer<Hiplayers>(mod);
				modPlayer.laserCharge = value;
			}
		}

		// Check if laser overcharged
		public bool OverCharge
		{
			get
			{
				Hiplayers modPlayer = Main.player[projectile.owner].GetModPlayer<Hiplayers>(mod);
				return modPlayer.laserOvercharge;
			}
		}

		public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ranged = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			// We start drawing the laser if not overcharged
			if (!OverCharge)
			{
				DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
					projectile.velocity, 10, projectile.damage, -1.57f, 1f, Color.White, (int)MoveDistance);
			}
			return false;
		}

		// The core function of drawing a laser
		public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, Color color = default(Color), int transDist = 50)
		{
			Vector2 origin = start;
			float r = unit.ToRotation() + rotation;

			#region Draw laser body
			float lastbody = 0f;
			for (float i = transDist; i <= Distance; i += step)
			{
				origin = start + i * unit;
				spriteBatch.Draw(texture, origin - Main.screenPosition,
					new Rectangle(0, 26, 28, 10), i < transDist ? Color.Transparent : color * (Math.Max(1f - i / MaxDistance, .3f)), r,
					new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
				lastbody = i;
			}
			#endregion

			/*#region Draw laser tail
			spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
				new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
			#endregion*/

			#region Draw laser head
			origin = start + (lastbody + step) * unit;
			spriteBatch.Draw(texture, origin - Main.screenPosition, new Rectangle(0, 52, 28, 26),
				color * (Math.Max(1f - (lastbody - step) / MaxDistance, .3f)), r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
			#endregion
		}

		// Change the way of collision check of the projectile
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!OverCharge)
			{
				Player player = Main.player[projectile.owner];
				Vector2 unit = projectile.velocity;
				float point = 0f;
				// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
				// It will look for collisions on the given line using AABB
				return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
					player.Center + unit * Distance, 20, ref point);
			}
			return false;
		}


		// Set damage by range (96 is roughly 3m)
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			int minDmg = damage / 10;
			double oldDmg = (double)damage - minDmg;
			int newDmg = damage;
			Vector2 dist = target.position - Main.player[projectile.owner].position;
			double pDist = dist.Length();
			oldDmg *= Math.Min(Math.Max(1 - ((pDist - 96) / (MaxDistance - 96)), 0), 1);
			oldDmg = Math.Ceiling(oldDmg);
			newDmg = (int)oldDmg + minDmg;
			if(newDmg == 1 && crit)
			{
				newDmg = 2;
			}
			damage = newDmg;
		}

		// Set custom immunity time on hitting an NPC
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			HiNPC tmod = target.GetGlobalNPC<HiNPC>();
			float fdamage = damage;
			fdamage /= tmod.raged ? 1.36f : 1f;
			fdamage /= crit ? 2f : 1f;
			fdamage /= (float)projectile.damage;
			fdamage *= 3;
			target.immune[projectile.owner] = 2 + (int)fdamage;
			Dust dust;
			dust = Dust.NewDustDirect(target.position, target.width, target.height, 133, 0f, 0f, 0, Color.White, 1.44f);
			dust.noGravity = true;

			#region Project Bunny 19C (1f)
			if (!target.active && projectile.ai[0] == 1f)
			{
				if (Main.rand.NextFloat() < 0.36f)
				{
					Main.player[projectile.owner].AddBuff(mod.BuffType<Buff.Pb19cBuff2>(), 180);
				}
			}
			#endregion
		}


		// The AI of the projectile
		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];

			#region Set projectile position
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();
				projectile.velocity = diff;
				projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				projectile.netUpdate = true;
			}
			projectile.position = player.Center + projectile.velocity * MoveDistance;
			projectile.timeLeft = 2;
			int dir = projectile.direction;
			player.ChangeDir(dir);
			//player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
			#endregion

			#region Charging process
			// Kill the projectile if the player stops channeling
			if (!player.channel)
			{
				projectile.Kill();
			}

			else
			{
				if (!OverCharge)
				{
					Charge += 10;
				}
			}
			#endregion

			if (!OverCharge)
			{
				#region Check Distance and Visual Effects
				Vector2 start = player.Center;
				Vector2 unit = projectile.velocity;
				unit *= -1;
				for (Distance = MoveDistance; Distance <= MaxDistance; Distance += 5f)
				{
					start = player.Center + projectile.velocity * Distance;
					if (!Collision.CanHitLine(player.Center, 1, 1, start, 0, 0))
					{
						Distance -= 5f;
						break;
					}
				}

				Vector2 pos = player.Center + projectile.velocity * (MoveDistance - 4) - new Vector2(10, 10);
				if (Main.rand.NextFloat() < 0.42f)
				{
					Dust dust;
					dust = Main.dust[Dust.NewDust(pos, 20, 20, 35, 0f, 0f, 147, new Color(255, 192, 35), 1.381579f)];
					dust.noGravity = true;
					dust.fadeIn = 1.105263f;
				}

				//Add lights
				DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
				Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
					DelegateMethods.CastLight);
				#endregion
			}
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override void CutTiles()
		{
			if (!OverCharge)
			{
				DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
				Vector2 unit = projectile.velocity;
				Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
			}
		}
	}
}
