using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace hiweapons.Buff
{
	public class Pb19cBuff2 : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Quantum Outbreak");
			Description.SetDefault("55% more damage");
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.HeldItem.type != mod.ItemType<Items.ProjectBunny>())
			{
				player.DelBuff(buffIndex);
			}
			else
			{
				player.rangedDamage += 0.55f;
			}
		}

	}
}
