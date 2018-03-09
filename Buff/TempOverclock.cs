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
	public class TempOverclock : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Temporary Overclock");
			Description.SetDefault("32% More damage");
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.HeldItem.type != mod.ItemType<Items.ProtoKatana>())
			{
				player.DelBuff(buffIndex);
			}
			else
			{
				player.meleeDamage += 0.32f;
			}
		}

	}
}
