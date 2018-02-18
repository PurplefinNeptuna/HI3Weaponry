using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;

namespace hiweapons
{
	class Hiweapons : Mod
	{
		static Mod mod;
		public Hiweapons()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
			mod = this;
		}

		//strange formula for custom rotation pivot(center)
		public static void CustRot(ref Player player, Item item, Vector2 center)
		{
			player.HeldItem.position = Vector2.Zero;
			Vector2 dist = center - item.BottomLeft;
			dist.X *= player.direction;
			dist = Vector2.Transform(dist, Matrix.CreateRotationZ(player.itemRotation));
			player.itemLocation -= dist;
		}

		//fix hitbox with custom rotation
		public static void CustSwingHitbox(Player player, Item item, ref Rectangle hitbox, int Wreduce, int offset)
		{
			bool progress = (float)player.itemAnimation >= (float)player.itemAnimationMax * 0.666f;
			bool progress2 = (float)player.itemAnimation >= (float)player.itemAnimationMax * 0.333f;
			if (progress)
			{
				hitbox.Width -= Wreduce;
				if (player.direction < 0)
				{
					hitbox.Offset(offset, 0);
				}
			}
		}

		//add glowmask
		public static short AddGlow(string name)
		{
			if (!Main.dedServ)
			{
				Texture2D[] glow = new Texture2D[Main.glowMaskTexture.Length + 1];
				for(int i = 0; i < Main.glowMaskTexture.Length; i++)
				{
					glow[i] = Main.glowMaskTexture[i];
				}
				glow[glow.Length - 1] = mod.GetTexture("glow/" + name);
				Main.glowMaskTexture = glow;
				return (short)(glow.Length - 1);
			}
			else return 0;
		}
	}
}
