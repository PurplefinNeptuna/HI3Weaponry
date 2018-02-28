using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace hiweapons.Items
{
	public class Blooddance : ModItem
	{
		public static short glowID = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Dance");
			Tooltip.SetDefault("Himeko's Greatsword");
			glowID = Hiweapons.AddGlow("Blooddance_glow");
		}
		public override void SetDefaults()
		{
			item.damage = 74;
			item.melee = true;
			item.crit = 0;
			item.width = 62;
			item.height = 62;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 8;
			item.scale = 1.45f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.glowMask = glowID;
		}

		public override bool AltFunctionUse(Player player)
		{
			if (!player.HasBuff(mod.BuffType<Buff.USCD>()))
				return true;
			else
				return false;
		}

		public override bool UseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				if (player.ownedProjectileCounts[mod.ProjectileType<Projectiles.BloodNado>()] < 1)
				{
					Projectile.NewProjectile(player.position.X + player.direction * 12 + 12, player.position.Y + 19 - 19 * player.gravDir, 4f * player.direction, 0, mod.ProjectileType<Projectiles.BloodNado>(), (item.damage * 315) / 200, 0f, player.whoAmI, 2.5f);
					player.AddBuff(mod.BuffType<Buff.USCD>(), 900);
				}
			}
			return true;
		}

		public override void UseStyle(Player player)
		{
			Hiweapons.CustRot(player, new Vector2(15, 47));

			//can change when not slashing
			if (player.itemAnimation < 2 || player.itemAnimation > player.itemAnimationMax - 2)
			{
				item.useTurn = true;
			}
			else item.useTurn = false;
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			Hiweapons.CustSwingHitbox(player, ref hitbox, 62, 77);
		}
	}
}
