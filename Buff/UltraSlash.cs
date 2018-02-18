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
	public class UltraSlash : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ultra Slash - Break");
			Description.SetDefault("13% More damage and enchance slash");
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if(player.HeldItem.type != mod.ItemType<Items.Balmung>() )
			{
				player.DelBuff(buffIndex);
			}
			else
			{
				player.meleeDamage += 0.13f;
			}
		}
	}
}
