using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.ModLoader.UI;
using System.Collections.Generic;
using hiweapons.UI;

namespace hiweapons
{

	// Greatsword 90
	// Katana 60
	// Cannnon 50
	class Hiweapons : Mod
	{
		static Mod mod;
		public UserInterface chargeResources;
		public ChargeLevel chargeLevel;

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
			if (!Main.dedServ)
			{
				chargeResources = new UserInterface();
				chargeLevel = new ChargeLevel();
				ChargeLevel.visible = true;
				chargeResources.SetState(chargeLevel);
			}
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup anyTitanium = new RecipeGroup(() => "Any Titanium Bar", new int[]{
				ItemID.AdamantiteBar,
				ItemID.TitaniumBar
			});
			RecipeGroup.RegisterGroup("Hiweapon:AnyTitanium", anyTitanium);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int ResourceIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (ResourceIndex != -1)
			{
				layers.Insert(ResourceIndex, new LegacyGameInterfaceLayer(
					"Honkai Impact 3: Laser Charge Level",
					delegate
					{
						if (ChargeLevel.visible)
						{
							chargeResources.Update(Main._drawInterfaceGameTime);
							chargeLevel.Draw(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.None)
				);
			}
		}

		#region Static Method for Utilities
		//strange formula for custom rotation pivot(center)
		public static void CustRot(Player player, Vector2 center)
		{
			player.HeldItem.position = Vector2.Zero;
			Vector2 dist = center - player.HeldItem.BottomLeft;
			dist.X *= player.direction;
			dist.Y *= player.gravDir; //dunno why
			dist = Vector2.Transform(dist, Matrix.CreateRotationZ(player.itemRotation));
			player.itemLocation -= dist;
		}

		//custom rotation pivot for held projectile
		public static void CustRot(Player owner, Projectile projectile, Vector2 displacement)
		{
			displacement.X *= owner.direction;
			displacement.Y *= owner.gravDir; //dunno why
			displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(projectile.rotation));
			projectile.position += displacement;
		}

		//fix hitbox with custom rotation
		public static void CustSwingHitbox(Player player, ref Rectangle hitbox, int Wreduce, int offset)
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
				for (int i = 0; i < Main.glowMaskTexture.Length; i++)
				{
					glow[i] = Main.glowMaskTexture[i];
				}
				glow[glow.Length - 1] = mod.GetTexture("glow/" + name);
				Main.glowMaskTexture = glow;
				return (short)(glow.Length - 1);
			}
			else return 0;
		}
		#endregion
	}
}
