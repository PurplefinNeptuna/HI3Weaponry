using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;

namespace hiweapons
{
	public class Hiplayers : ModPlayer
	{
		public int laserDischargeSpeed = 10;
		public int laserCharge = 0;
		public int laserMaxCharge = 2400;
		public bool laserOvercharge = false;
		private int? lastWrittenCharge = null;

		public override void OnRespawn(Player player)
		{
			laserDischargeSpeed = 10;
			laserCharge = 0;
			laserOvercharge = false;
			lastWrittenCharge = null;
		}

		public override void OnEnterWorld(Player player)
		{
			laserDischargeSpeed = 10;
			laserCharge = 0;
			laserOvercharge = false;
			lastWrittenCharge = null;
		}

		public override void PostUpdate()
		{
			if (!player.dead && player.active)
			{
				/*
				if (lastWrittenCharge == null || lastWrittenCharge != laserCharge/10)
				{
					Main.NewText(string.Format("Charge: {0}", laserCharge/10));
					lastWrittenCharge = laserCharge/10;
				}*/

				if (!player.channel)
				{
					laserDischargeSpeed = Math.Max(30 - (laserCharge * 30) / laserMaxCharge, 10);
					laserDischargeSpeed *= 3;
					laserCharge = Math.Max(0, laserCharge - laserDischargeSpeed / (laserOvercharge ? 4 : 2));
				}
				if (laserCharge <= 0 && laserOvercharge)
				{
					laserCharge = 0;
					laserOvercharge = false;
				}
				else if (laserCharge >= laserMaxCharge && !laserOvercharge)
				{
					laserCharge = laserMaxCharge;
					laserOvercharge = true;
				}
			}
		}
	}
}
