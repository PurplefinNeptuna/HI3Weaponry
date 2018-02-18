using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Items
{
    public class Des : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Des");
            Tooltip.SetDefault("This is a modded sword.");
        }
        public override void SetDefaults()
        {
            item.damage = 50;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useTime = 60;
            item.useAnimation = 60;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

		public override void UseStyle(Player player)
		{
			item.position.X = 0;
			item.position.Y = 0;
			Vector2 dist = new Vector2(20, 29) - item.BottomLeft;
			dist.X *= player.direction;
			dist = Vector2.Transform(dist, Matrix.CreateRotationZ(player.itemRotation));
			player.itemLocation -= dist;
		}
	}
}
