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

namespace minicustomtowersv2
{
    public class BananaFarmerTower
    {
        public class BananaFarmer : ModTower
        {
            public override string BaseTower => "DartMonkey";
            public override string Name => "BananaFarmer ";
            public override int Cost => 100;
            public override string DisplayName => "Banana Farmer";
            public override string Description => "Automatically collects bananas for you.";
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = Game.instance.model.GetTowerFromId("BananaFarmer").portrait;
                towerModel.portrait = Game.instance.model.GetTowerFromId("BananaFarmer").portrait;
                towerModel.radius = Game.instance.model.GetTowerFromId("BananaFarmer").radius;
                towerModel.radiusSquared = Game.instance.model.GetTowerFromId("BananaFarmer").radiusSquared;
                towerModel.range = Game.instance.model.GetTowerFromId("BananaFarmer").range;
                towerModel.display = Game.instance.model.GetTowerFromId("BananaFarmer").display;
                towerModel.GetBehavior<DisplayModel>().display = Game.instance.model.GetTowerFromId("BananaFarmer").GetBehavior<DisplayModel>().display;
                towerModel.RemoveBehavior<AttackModel>();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarmer").GetBehavior<CollectCashZoneModel>().Duplicate());
                towerModel.isGlobalRange.Equals(true);
            }
        }

        public class BananaCannon : ModUpgrade<BananaFarmer>
        {
            public override string Name => "BananaCannon";
            public override string DisplayName => "Banana Cannon";
            public override string Description => "Shoots bananas on the track, blowing bloons back.";
            public override int Cost => 750;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "BananaCannon_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var ninjaAttack = Game.instance.model.GetTowerFromId("NinjaMonkey-002").GetAttackModel(1).Duplicate();
                ninjaAttack.weapons[0].projectile.display = "595c7fb1d3705aa478d0d5171b74fb57";
                ninjaAttack.weapons[0].projectile.pierce = 1f;
                var windModel = Game.instance.model.GetTowerFromId("NinjaMonkey-010").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate();
                windModel.chance = 1.00f;
                windModel.distanceMin = 100f;
                ninjaAttack.weapons[0].projectile.AddBehavior(windModel);
                ninjaAttack.range = towerModel.range;
                ninjaAttack.weapons[0].projectile.GetDamageModel().damage = 0.0f;
                ninjaAttack.weapons[0].projectile.SetHitCamo(false);
                towerModel.AddBehavior(ninjaAttack);
            }
        }
        public class SharpPitchfork : ModUpgrade<BananaFarmer>
        {
            public override string Name => "SharpPitchfork";
            public override string DisplayName => "Sharp Pitchfork";
            public override string Description => "Hits bloons near him with his pitchfork.";
            public override int Cost => 1300;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "Pitchfork_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var saudaAttack = Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().Duplicate();
                saudaAttack.weapons[0].Rate = 0.6f;
                towerModel.AddBehavior(saudaAttack);
            }
        }
        public class GoldenBloons : ModUpgrade<BananaFarmer>
        {
            public override string Name => "GoldenBloons";
            public override string DisplayName => "Golden Bloons";
            public override string Description => "Generates money when a bloon is hit.";
            public override int Cost => 2450;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "GoldenBloon_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(1);
                var bananaFarmCash = Game.instance.model.GetTowerFromId("BananaFarm-003").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                var bananaFarmText = Game.instance.model.GetTowerFromId("BananaFarm-003").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                bananaFarmCash.minimum = 1f;
                bananaFarmCash.maximum = 1f;
                attackModel.weapons[0].projectile.AddBehavior(bananaFarmCash);
                attackModel.weapons[0].projectile.AddBehavior(bananaFarmText);
            }
        }
        public class BananaThrower : ModUpgrade<BananaFarmer>
        {
            public override string Name => "BananaThrower";
            public override string DisplayName => "Banana Thrower";
            public override string Description => "Instead of throwing bananas on the ground, the banana farmer throws bananas at bloons, gaining money and dealing damage to the bloon.";
            public override int Cost => 5500;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "BananaThrower_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.RemoveBehavior<AttackModel>();
                var ninja = Game.instance.model.GetTowerFromId("NinjaMonkey").GetAttackModel().Duplicate();
                ninja.name = "BananaFarmer-040";
                towerModel.AddBehavior(ninja);
                foreach(var attack in towerModel.GetAttackModels())
                {
                    if(attack.name == "BananaFarmer-040")
                    {
                        attack.range = towerModel.range;
                        attack.weapons[0].Rate = 0.25f;
                        attack.weapons[0].projectile.display = "595c7fb1d3705aa478d0d5171b74fb57";
                        attack.weapons[0].projectile.pierce = 3.0f;
                        attack.weapons[0].projectile.GetDamageModel().damage = 5.0f;
                        attack.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("BananaFarmer-Banana", attack.weapons[0].projectile.Duplicate(), new SingleEmissionModel("SingleEmissionModel_", null), true, false, true));
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.display = "a";
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 0.0f;
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<TravelStraitModel>().Lifespan = 0.01f;
                        var bananaFarmCash = Game.instance.model.GetTowerFromId("BananaFarm-003").GetWeapon().projectile.GetBehavior<CashModel>().Duplicate<CashModel>();
                        var bananaFarmText = Game.instance.model.GetTowerFromId("BananaFarm-003").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
                        bananaFarmCash.minimum = 5f;
                        bananaFarmCash.maximum = 5f;
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(bananaFarmCash);
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.AddBehavior(bananaFarmText);
                    }
                }
            }
        }
        public class BananaCrates : ModUpgrade<BananaFarmer>
        {
            public override string Name => "BananaCrates";
            public override string DisplayName => "Banana Crates";
            public override string Description => "A banana making machine...";
            public override int Cost => 47050;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override string Icon => "BananaCrates_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach (var attack in towerModel.GetAttackModels())
                {
                    if (attack.name == "BananaFarmer-040")
                    {
                        attack.weapons[0].Rate = 0.25f;
                        attack.weapons[0].projectile.display = "88442e0b3684e3446aaa70a036da69c9";
                        attack.weapons[0].projectile.pierce = 7.0f;
                        attack.weapons[0].projectile.GetDamageModel().damage = 15.0f;
                        attack.weapons[0].emission = new ArcEmissionModel("BananaFarmer-050", 2, 0.0f, 20.0f, null, false);
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CashModel>().minimum = 15.0f;
                        attack.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CashModel>().maximum = 15.0f;
                    }
                }
            }
        }
    }

}