using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Items
{
    public class Magstorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magstorm");
            Tooltip.SetDefault("This is a modded sword.");
        }
        public override void SetDefaults()
        {
            item.damage = 76;
            item.melee = true;
			item.crit = -4;
            item.width = 64;
            item.height = 64;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = 10000;
            item.rare = 2;
            item.scale = 0.94f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
			item.useTurn = true;
        }
		public override void UseStyle(Player player)
		{
			Hiweapons.CustRot(player, new Vector2(8, 58));
		}
	}
}
