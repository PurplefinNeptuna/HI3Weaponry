using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Projectiles
{
	public class Balmungp : ModProjectile
	{
		public static short glowNorm = 0;
		public static short glowCrit = 0;

		public override void SetStaticDefaults()
		{
			glowNorm = Hiweapons.AddGlow("Balmungp_off");
			glowCrit = Hiweapons.AddGlow("Balmungp_on");
		}

		public override void SetDefaults()
		{
			projectile.Name = "Balmung Projectile";
			projectile.height = 32;
			projectile.width = 60;
			projectile.scale = 1.44f;
			projectile.timeLeft = 48;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.melee = true;
			projectile.aiStyle = 0;
		}

		public override void AI()
		{
			projectile.rotation = (float)((Math.PI/2f)*projectile.ai[0]);
			bool pCrit = projectile.ai[1]==1f;
			if (pCrit)
			{
				projectile.glowMask = glowCrit;
			}
			else
			{
				projectile.glowMask = glowNorm;
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			bool pCrit = projectile.ai[1] == 1f;
			if (Main.rand.NextFloat() < (pCrit ? 0.60f : 0.10f))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 263, 0f, 0f, 157, new Color(179, 143, 234), pCrit ? 2.0f : 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.5f;
			}
			Rectangle newhitbox = new Rectangle(hitbox.X+((int)projectile.ai[0]>0?17:-2), (2 * hitbox.Center.Y - projectile.width) / 2, projectile.height, projectile.width);
			hitbox = newhitbox;
		}
	}
}