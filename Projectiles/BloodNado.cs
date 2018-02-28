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
	public class BloodNado : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.Name = "Blood Tornado";
			projectile.width = 40;
			projectile.height = 40;
			projectile.scale = 1;
			projectile.timeLeft = 180;
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
			projectile.scale = projectile.ai[0];
			if (++projectile.frameCounter >= 5)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 6)
				{
					projectile.frame = 0;
				}
			}
		}

		//396
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			float chance = Main.rand.NextFloat();
			bool getDebuff = target.boss ? chance < 0.66f : chance < 0.88f;
			if (getDebuff)
				target.AddBuff(mod.BuffType<Buff.BloodRage>(), 396);

		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.Inflate(hitbox.Width / 4, hitbox.Height / 2);
		}
	}
}