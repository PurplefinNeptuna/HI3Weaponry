using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace hiweapons
{
	public class HiNPC : GlobalNPC
	{
		public override bool InstancePerEntity
		{
			get { return true; }
		}
		public bool raged = false;

		public override void ResetEffects(NPC npc)
		{
			raged = false;
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (raged)
			{
				Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, 0, 0, 0, 0, new Color(255,0,0), 1.25f)];
			}
		}

		public override void ModifyHitPlayer(NPC npc, Player target, ref int damage, ref bool crit)
		{
			if (raged)
			{
				float newDamage = damage;
				float defMult = Main.expertMode ? 0.75f : 0.5f;
				float def = target.statDefense;
				float endurance = target.endurance;
				/*
					A = applied damage
					B = damage
					D = defense
					d = defense multiplier
					E = endurance
					X = additional damage
					Find damage (X) so applied damage become 2x (2A)
					A			= (B - D * d) * (1 - E)
					2A			= (B + X - D * d) * (1 - E)
					2			= (B + X - D * d) / (B - D * d)
					2B - 2D * d = B + X - D * d
					B - D * d	= X
					
					Find minimum damage (B) so applied damage become 1 (A = 1)
					1					= (B - D * d) * (1 - E)
					1 / (1 - E)			= B - D * d
					1 / (1 - E) + D * d = B
				*/
				float A = 1f / (1f - endurance) + def * defMult;
				if (newDamage - A < 0.000001f)
				{
					newDamage = A;
				}
				else
				{
					newDamage = newDamage - def * defMult;
				}

				damage += (int)newDamage;
			}
		}

		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if (raged)
			{
				damage *= 1.36f;
			}
			return true;
		}
	}
}
