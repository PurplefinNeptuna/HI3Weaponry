using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Items
{
	public class ProtoKatana : ModItem
	{
		public bool buffed = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Proto Katana");
			Tooltip.SetDefault("Mei's Katana");
		}

		public override void SetDefaults()
		{
			item.damage = 59;
			item.melee = true;
			item.crit = 11;
			item.width = 62;
			item.height = 62;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = 10000;
			item.rare = 4;
			item.scale = 0.97f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("Hiweapon:AnyTitanium", 2);
			recipe.AddRecipeGroup("Hiweapon:AnyMechSoul", 2);
			recipe.AddIngredient(ItemID.HallowedBar, 2);
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

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -20);
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
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
				Hiweapons.CustRot(player, new Vector2(10, 54));
			}
		}

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				if (!buffed && !player.HasBuff(mod.BuffType<Buff.TempOverclock>()))
				{
					buffed = true;
					player.AddBuff(mod.BuffType<Buff.TempOverclock>(), 300);
					player.AddBuff(mod.BuffType<Buff.USCD>(), 900);
				}
			}
			return true;
		}
		public override void UpdateInventory(Player player)
		{
			if (!player.HasBuff(mod.BuffType<Buff.TempOverclock>()) && buffed)
			{
				buffed = false;
			}
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			if (player.altFunctionUse == 2)
			{
				noHitbox = true;
			}
		}
	}
}
