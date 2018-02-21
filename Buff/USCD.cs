using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons.Buff
{
	public class USCD : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Skill Cooldown");
			Description.SetDefault("Can't use weapon skill");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}
	}
}
