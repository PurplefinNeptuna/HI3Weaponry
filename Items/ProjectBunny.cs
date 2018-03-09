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


namespace hiweapons.Items
{
	public class ProjectBunny : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Project Bunny 19C");
			Tooltip.SetDefault("Bronya's Laser Cannon");
		}

		public override void SetDefaults()
		{
			item.damage = 92;
			item.ranged = true;
			item.channel = true;
			item.width = 51;
			item.height = 21;
			item.useTime = 4;
			// 5% critical chance
			item.crit = 1;
			item.useAnimation = 4;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 8;
			item.UseSound = SoundID.Item11;
			item.shoot = mod.ProjectileType<Projectiles.BronyaLaser>(); 
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddRecipeGroup("Hiweapon:AnyTitanium", 5);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void HoldItem(Player player)
		{
			if (!player.HasBuff(mod.BuffType<Buff.Pb19cBuff>()))
			{
				player.AddBuff(mod.BuffType<Buff.Pb19cBuff>(), 10);
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType<ProjectBunnyp>()] < 1)
				Projectile.NewProjectile(player.itemLocation, new Vector2(0, 0), mod.ProjectileType<ProjectBunnyp>(), 0, 0f, player.whoAmI);

			Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 1f);
			return false;
		}

	}

	public class ProjectBunnyp : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.Name = "Project Bunny 19C";
			projectile.penetrate = -1;
			projectile.width = 51;
			projectile.height = 21;
			projectile.scale = 1.2f;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.aiStyle = 0;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override void AI()
		{
			Player projOwner = Main.player[projectile.owner];
			projOwner.heldProj = projectile.whoAmI;
			projectile.rotation = projOwner.itemRotation;
			projectile.direction = projOwner.direction;
			projectile.spriteDirection = projOwner.direction;
			projectile.position = projOwner.itemLocation;
			Hiweapons.CustRot(projOwner, projectile, new Vector2(15.5f, 0));
			if (!projOwner.channel)
			{
				projectile.Kill();
			}
		}
	}
}
