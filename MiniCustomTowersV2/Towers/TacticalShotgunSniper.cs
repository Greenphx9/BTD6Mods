using MelonLoader;
using Harmony;

using Assets.Scripts.Unity.UI_New.InGame;

using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Utils;
using System;
using System.Text.RegularExpressions;
using System.IO;
using Assets.Main.Scenes;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Simulation.Track;
using static Assets.Scripts.Models.Towers.TargetType;
using Assets.Scripts.Simulation;
using Assets.Scripts.Models.Towers.Pets;
using Assets.Scripts.Unity.Bridge;
using System.Windows;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;

namespace minicustomtowersv2
{
    public class TacticalShotgunSniperTower
    {
        public class TacticalShotgunSniper : ModTower
        {
            public override string BaseTower => "SniperMonkey-210";
            public override string Name => "TacticalShotgunSnipe";
            public override int Cost => 4000;
            public override string DisplayName => "Tactical Shotgun Sniper";
            public override string Description => "Sniper bullet pops surrounding bloons.";
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.ApplyDisplay<TacticalShotgunSniperDisplay>();
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", Game.instance.model.GetTowerFromId("BombShooter-002").GetWeapon().projectile.GetBehaviors<CreateProjectileOnContactModel>().Last().projectile.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 5, 0.0f, 360.0f, null, false, false), true, false, false));
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 6.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().maxDamage = 6.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().CapDamage(6.0f);
            }
            public override string Portrait => "TacticalShotgunSniper_Icon";
            public override string Icon => "TacticalShotgunSniper_Icon";
        }
        public class TacticalShotgunSniperDisplay : ModDisplay
        {
            public override string BaseDisplay => "9a88e6fa97600b148b7439b48cab222e";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.genericRenderers[2].material.mainTexture = GetTexture(mod, "TacticalShotgunSniperDisplay");
            }
        }
        public class Bloonzooka : ModUpgrade<TacticalShotgunSniper>
        {
            public override string Name => "Bloonzooka";
            public override string DisplayName => "Bloonzooka";
            public override string Description => "Does 4 layers to a large area.";
            public override int Cost => 3300;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.RemoveBehavior<CreateProjectileOnContactModel>();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 4.0f;
            }
            public override string Icon => "Bloonzooka_Icon";
        }
        public class RPGStrike : ModUpgrade<TacticalShotgunSniper>
        {
            public override string Name => "RPGStrike";
            public override string DisplayName => "RPG Strike";
            public override string Description => "Does 100 layers to a large crowd.";
            public override int Cost => 17600;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 100.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().maxDamage = 100.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().CapDamage(100.0f);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius *= 1.25f;
                attackModel.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale *= 1.25f;
            }
            public override string Icon => "RPGStrike_Icon";
        }
    }

}