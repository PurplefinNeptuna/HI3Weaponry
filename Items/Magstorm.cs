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
            item.damage = 50;
            item.melee = true;
            item.width = 64;
            item.height = 64;
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
			Hiweapons.CustSwingHitbox(player, item, ref hitbox, 32, 40);
        }
    }
}
