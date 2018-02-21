using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.GameContent.UI.Elements;

namespace hiweapons.UI
{
	class ChargeLevel : UIState
	{
		public static bool visible = false;
		public UIChargeBack back;
		public UIChargeFront fill;
		public float leftPos = (Main.screenWidth - 100) / 2;
		public float topPos = 0f;
		public Color bgColor = Color.White;

		public override void OnInitialize()
		{
			back = new UIChargeBack(20,100);
			back.Left.Set(leftPos, 0f);
			back.Top.Set(topPos, 0f);
			back.backgroundColor = Color.White;
			fill = new UIChargeFront(8, 74);
			fill.Left.Set(leftPos + 4f, 0f);
			fill.Top.Set(topPos + 6f, 0f);
			fill.backgroundColor = Color.White;
			base.Append(back);
			base.Append(fill);
		}
		public override void Update(GameTime gameTime)
		{
			Player player = Main.player[Main.myPlayer];
			Vector2 playerPos = player.position - Main.screenPosition;
			playerPos.X += player.width / 2;
			back.Left.Set(playerPos.X - 50,0f);
			fill.Left.Set(playerPos.X - 46, 0f);
			back.Top.Set(playerPos.Y + player.height + 10f, 0f);
			fill.Top.Set(playerPos.Y + player.height + 16f, 0f);

			Hiplayers hiplayer = player.GetModPlayer<Hiplayers>();
			int charge = hiplayer.laserCharge;
			int maxCharge = hiplayer.laserMaxCharge;
			bool overCharge = hiplayer.laserOvercharge;
			float urgent = (float)hiplayer.laserCharge / (float)hiplayer.laserMaxCharge;

			fill.SetProgress(urgent);

			urgent = overCharge ? 1f : Math.Min(urgent * 1.5f, 1f);
			bgColor = Color.White * urgent;

			back.SetOvercharge(overCharge);
			fill.SetOvercharge(overCharge);
			back.backgroundColor = bgColor;
			fill.backgroundColor = bgColor;

			Recalculate();
			base.Update(gameTime);
		}
	}

	class UIChargeFront : UIElement
	{
		public Color backgroundColor = Color.White;
		private static Texture2D _backgroundTexture;
		private static Texture2D _backgroundTexture2;
		private float width;
		private float defaultWidth;
		private float height;
		private bool overCharge = false;

		public UIChargeFront(float height, float width)
		{
			if (_backgroundTexture == null)
			{
				_backgroundTexture = ModLoader.GetTexture("hiweapons/UI/cbarfillnorm");
			}
			if (_backgroundTexture2 == null)
			{
				_backgroundTexture2 = ModLoader.GetTexture("hiweapons/UI/cbarfillover");
			}
			this.height = height;
			this.width = width;
			defaultWidth = width;
		}

		public override void OnInitialize()
		{
			Height.Set(height, 0f);
			Width.Set(width, 0f);
		}

		public void SetProgress(float progress)
		{
			Width.Set(defaultWidth * progress, 0f);
		}

		public void SetOvercharge(bool over)
		{
			overCharge = over;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Texture2D used = overCharge ? _backgroundTexture2 : _backgroundTexture;

			CalculatedStyle dimensions = GetDimensions();
			Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);
			spriteBatch.Draw(used, new Rectangle(point1.X, point1.Y, width, height),
				new Rectangle(4, 6, width, height), backgroundColor);
		}
	}

	class UIChargeBack : UIElement
	{
		public Color backgroundColor = Color.White;
		private static Texture2D _backgroundTexture;
		private static Texture2D _backgroundTexture2;
		private float width;
		private float height;
		private bool overCharge = false;

		public UIChargeBack(float height, float width)
		{
			if (_backgroundTexture == null)
			{
				_backgroundTexture = ModLoader.GetTexture("hiweapons/UI/cbarbacknorm");
			}
			if (_backgroundTexture2 == null)
			{
				_backgroundTexture2 = ModLoader.GetTexture("hiweapons/UI/cbarbackover");
			}
			this.height = height;
			this.width = width;
		}

		public override void OnInitialize()
		{
			Height.Set(height, 0f);
			Width.Set(width, 0f);
		}

		public void SetOvercharge(bool over)
		{
			overCharge = over;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Texture2D used = overCharge ? _backgroundTexture2 : _backgroundTexture;

			CalculatedStyle dimensions = GetDimensions();
			Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
			int width = (int)Math.Ceiling(dimensions.Width);
			int height = (int)Math.Ceiling(dimensions.Height);
			spriteBatch.Draw(used, new Rectangle(point1.X, point1.Y, width, height),
				new Rectangle(0, 0, width, height), backgroundColor);
		}
	}
}
