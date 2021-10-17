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

namespace minicustomtowersv2
{
    public class LightsabreThrowerTower
    {
        public class LightsabreThrower : ModTower
        {
            public override string BaseTower => "BoomerangMonkey-002";
            public override string Name => "LightsabreThrower";
            public override int Cost => 2000;
            public override string DisplayName => "Lightsabre Thrower";
            public override string Description => "Throws his lightsabre at bloons. Has 80 pierce";
            public override bool Use2DModel => true;
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 1;
            public override int BottomPathUpgrades => 0;
            public override float PixelsPerUnit => 5f;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 80f;
                towerModel.GetAttackModel().weapons[0].projectile.ApplyDisplay<LightsabreDisplay>();
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Ceramic", "Ceramic", 1.0f, 3.0f, false, true));
                towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 3.0f, false, true));
                towerModel.GetAttackModel().weapons[0].projectile.radius *= 1.5f;
                towerModel.GetAttackModel().weapons[0].projectile.scale *= 1.5f;
            }
            public override string Icon => "LightsabreThrowerDisplay";
            public override string Portrait => "LightsabreThrowerDisplay";
            public override string Get2DTexture(int[] tiers)
            {
                return "LightsabreThrowerDisplay";
            }
        }

        public class LightsabreDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "LightSabre");
            }
        }
        public class DoubleLightsabre : ModUpgrade<LightsabreThrower>
        {
            public override string Name => "DoubleLightsabre";
            public override string DisplayName => "Double Lightsabre";
            public override string Description => "Throws 2 lightsabres at once!";
            public override int Cost => 1450;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "DoubleLightsabre_Icon";
            public override void ApplyUpgrade(TowerModel tower)
            {
                tower.GetAttackModel().weapons[0].emission = new ArcEmissionModel("Double Lightsabre", 2, 0.0f, 15.0f, null, false);
            }
        }
    }

}