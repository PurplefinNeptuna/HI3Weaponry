using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Items
{
	public class Balmung : ModItem
	{
		public static short glowSkillOff = 0;
		public static short glowSkillOn = 0;
		public bool buffed = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Balmung");
			Tooltip.SetDefault("Himeko's Greatsword");
			glowSkillOff = Hiweapons.AddGlow("Balmung_off");
			glowSkillOn = Hiweapons.AddGlow("Balmung_on");
		}
		public override void SetDefaults()
		{
            //40% of 4* balmung stats
			item.damage = 72;
            //10% crit chance
            item.crit = 6;
			item.melee = true;
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
			item.glowMask = glowSkillOff;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddIngredient(ItemID.SoulofSight, 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player)
		{
			if (!player.HasBuff(mod.BuffType<Buff.USCD>()))
				return true;
			else
				return false;
		}

		public override bool CanUseItem(Player player)
		{
			if(player.altFunctionUse == 2)
			{
				item.useStyle = 5;
			}
			else
			{
				item.useStyle = 1;
			}
			return base.CanUseItem(player);
		}

		public override void UseStyle(Player player)
        {
			if (player.altFunctionUse == 2)
			{
				return;
			}
			else
			{
				Hiweapons.CustRot(player, new Vector2(15,47));

				//can change when not slashing
				if (player.itemAnimation < 2 || player.itemAnimation > player.itemAnimationMax - 2)
				{
					item.useTurn = true;
				}
				else item.useTurn = false;
			}
        }

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (!buffed && !player.HasBuff(mod.BuffType<Buff.UltraSlash>()))
				{
					buffed = true;
					player.AddBuff(mod.BuffType<Buff.UltraSlash>(), 480);
					player.AddBuff(mod.BuffType<Buff.USCD>(), 1500);
					item.glowMask = glowSkillOn;
				}
			}
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (!target.friendly)
			{
				if (player.ownedProjectileCounts[mod.ProjectileType<Projectiles.Balmungp>()] < 1)
				{
					bool pCrit = Main.rand.NextFloat() < 0.15f || buffed;
					Projectile.NewProjectile(player.position.X + player.direction * 24 + 24, player.position.Y + 19 - 19 * player.gravDir, (pCrit ? 12.5f : 10) * player.direction, 0, mod.ProjectileType<Projectiles.Balmungp>(), (item.damage * (pCrit ? 8 : 4)) / 9, 0f, player.whoAmI, player.direction, pCrit ? 1f : 0f);
				}
			}
		}

		public override void UpdateInventory(Player player)
		{
			if(!player.HasBuff(mod.BuffType<Buff.UltraSlash>()) && buffed)
			{
				buffed = false;
				item.glowMask = glowSkillOff;
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -25);
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
			if (player.altFunctionUse == 2)
			{
				noHitbox = true;
			}
			else
			{
				Hiweapons.CustSwingHitbox(player, ref hitbox, 62, 77);
			}
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			if (Main.rand.NextFloat() < (buffed ? 0.30f : 0.10f))
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 263, 0f, 0f, 157, new Color(255, 0, 251), buffed ? 2.0f : 0.5f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].fadeIn = 0.5f;
			}
        }
    }
}
