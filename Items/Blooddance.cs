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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Dance");
			Tooltip.SetDefault("This is a modded sword.");
		}
		public override void SetDefaults()
		{
			item.damage = 76;
			item.melee = true;
			item.crit = -4;
			item.width = 62;
			item.height = 62;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.scale = 0.9f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void UseItemHitbox(Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			Hiweapons.CustSwingHitbox(player, ref hitbox, 62, 77);
		}
	}
}
