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
	public class BloodRage : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Enraged");
			Description.SetDefault("Attack damage increased by 50%, but damage taken increased by 75%");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			HiNPC ModNpc = npc.GetGlobalNPC<HiNPC>();
			ModNpc.raged = true;
		}
	}
}
