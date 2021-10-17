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
    public class EnergyShooterTower
    {
        public class EnergyShooter : ModTower
        {
            public override string BaseTower => "DartMonkey";
            public override string Name => "EnergyShooter";
            public override int Cost => 500;
            public override string DisplayName => "Energy Shooter";
            public override string Description => "Shoots balls of energy at bloons.";
            public override string TowerSet => "Magic";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.range = 20f;
                towerModel.GetAttackModel().range = 1000f;
                towerModel.towerSize = TowerModel.TowerSize.medium;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.ApplyDisplay<EnergyShooterProjDisplay>();
                attackModel.weapons[0].projectile.pierce = 3.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 2.0f;
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2.0f;
                towerModel.display = "482a273a5f7105242a528ba030c28b09";
                towerModel.GetBehavior<DisplayModel>().display = "482a273a5f7105242a528ba030c28b09";
            }
            public override string Icon => "EnergyShooter_Icon";
            public override string Portrait => "EnergyShooter_Icon";
        }
        public class EnergyShooterProjDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "EnergyBall");
            }
        }
        public class StrongerEnergy : ModUpgrade<EnergyShooter>
        {
            public override string Name => "StrongerEnergy";
            public override string DisplayName => "Stronger Energy";
            public override string Description => "Energy pops through 4 layers of bloon at once and can pierce through an extra bloon!";
            public override int Cost => 1000;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "StrongerEnergy_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 2.0f;
                towerModel.GetAttackModel().weapons[0].projectile.pierce += 1.0f;
            }
        }
        public class FasterEnergy : ModUpgrade<EnergyShooter>
        {
            public override string Name => "FasterEnergy";
            public override string DisplayName => "Faster Energy";
            public override string Description => "Energy is shot out of the tower faster.";
            public override int Cost => 2700;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "FasterEnergy_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].Rate *= 0.5f;
            }
        }
        public class BurningEnergy : ModUpgrade<EnergyShooter>
        {
            public override string Name => "BurningEnergy";
            public override string DisplayName => "Burning Energy";
            public override string Description => "Energy burns bloons on contact.";
            public override int Cost => 3400;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "BurningEnergy_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile = Game.instance.model.GetTowerFromId("WizardMonkey-030").GetAttackModel(3).weapons[0].projectile.Duplicate();
                towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<EnergyShooterProjDisplay>();
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 3.0f;
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 4.0f;
                towerModel.GetAttackModel().weapons[0].projectile.RemoveBehavior<TravelStraitModel>();
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().projectile.GetBehavior<TravelStraitModel>().Duplicate());
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 2.0f;
            }
        }
        public class DoubleEnergy : ModUpgrade<EnergyShooter>
        {
            public override string Name => "DoubleEnergy";
            public override string DisplayName => "Double Energy";
            public override string Description => "Shoots two blasts of energy at once. Energy can pop through 2 more bloons each.";
            public override int Cost => 4000;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "DoubleEnergy_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0.0f, 20.0f, null, false);
                towerModel.GetAttackModel().weapons[0].projectile.pierce += 2.0f;
            }
        }
        public class SuperEnergy : ModUpgrade<EnergyShooter>
        {
            public override string Name => "SuperEnergy";
            public override string DisplayName => "Super Energy";
            public override string Description => "Shoots three blasts of energy at once. Shoots energy faster. Energy pops through more layers of bloon and can hit more bloons.";
            public override int Cost => 33000;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override string Icon => "SuperEnergy_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].emission.Cast<ArcEmissionModel>().count = 3;
                towerModel.GetAttackModel().weapons[0].Rate *= 0.3f;
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 5.0f;
                towerModel.GetAttackModel().weapons[0].projectile.pierce += 12.0f;
                towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<SuperEnergyBall>();
            }
        }
        public class SuperEnergyBall : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "SuperEnergyBall");
            }
        }
    }

}