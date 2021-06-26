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
using Assets.Scripts.Models.Towers.Weapons;

namespace minicustomtowersv2
{
    public class SummonWhirlwindTower
    {
        public class SummonWhirlwind : ModTower
        {
            public override string BaseTower => "WizardMonkey-012";
            public override string Name => "SummonWhirlwind";
            public override int Cost => 4350;
            public override string DisplayName => "Summon Whirlwind";
            public override string Description => "Whirlwind blows bloons of the path away from the exit.";
            public override string TowerSet => "Magic";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 1;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].Rate = 0.8857142857f;
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 3.0f;
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage = 1.0f;
                towerModel.GetAttackModel(1).weapons[0].Rate = 2.1f;
                towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].Duplicate());
                towerModel.GetAttackModel().weapons[1].Rate = 1.6857142857f;
                towerModel.GetAttackModel().AddWeapon(Game.instance.model.GetTowerFromId("Druid-300").GetAttackModel().weapons[1].Duplicate());
                towerModel.GetAttackModel().weapons[2].Rate = 1.8f;
                towerModel.GetAttackModel().weapons[2].projectile.pierce = 31f;
                towerModel.GetAttackModel().weapons[2].projectile.scale *= 1.5f;
                towerModel.GetAttackModel().weapons[2].projectile.radius *= 1.5f;
            }
            public override bool Use2DModel => true;
            public override string Icon => "SummonWhirlwind_Icon";
            public override string Portrait => "SummonWhirlwind_Icon";
            public override string Get2DTexture(int[] tiers)
            {
                if(tiers[1] == 1)
                {
                    return "TempestTornadoDisplay";
                }
                return "SummonWhirlwindDisplay";
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class TempestTornado : ModUpgrade<SummonWhirlwind>
        {
            public override string Name => "TempestTornado";
            public override string DisplayName => "Tempest Tornado";
            public override string Description => "Pops bloons and blows them back faster, further and more often. Also improves the lightning bolt attack.";
            public override int Cost => 8000;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[1].Rate -= 1.41428571428f;
                towerModel.GetAttackModel().weapons[2].Rate = 1.3f;
                towerModel.GetAttackModel().weapons[2].projectile.pierce = 150f;
                towerModel.GetAttackModel().weapons[2].projectile.scale *= 1.5f;
                towerModel.GetAttackModel().weapons[2].projectile.radius *= 1.5f;
                towerModel.GetAttackModel().weapons[2].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().projectile.GetDamageModel().Duplicate());
                towerModel.GetAttackModel().weapons[2].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
            }
            public override string Icon => "TempestTornado_Icon";
        }


    }

}