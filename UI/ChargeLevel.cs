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
		public UIProgressImage back;
		public UIProgressImage fill;
		public float leftPos = (Main.screenWidth - 100) / 2;
		public float topPos = 0f;
		public Color bgColor = Color.White;

		public override void OnInitialize()
		{
			back = new UIProgressImage(20, 100, "hiweapons/UI/cbarbacknorm", "hiweapons/UI/cbarbackover");
			back.Left.Set(leftPos, 0f);
			back.Top.Set(topPos, 0f);
			back.backgroundColor = Color.White;
			fill = new UIProgressImage(8, 74, "hiweapons/UI/cbarfillnorm", "hiweapons/UI/cbarfillover", 4, 6);
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

			back.SetAlter(overCharge);
			fill.SetAlter(overCharge);
			back.backgroundColor = bgColor;
			fill.backgroundColor = bgColor;

			Recalculate();
			base.Update(gameTime);
		}
	}
}
