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
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace minicustomtowersv2
{
    public class GuidedMissileLauncherTower
    {
        public class GuidedMissileLauncher : ModTower
        {
            public override string BaseTower => "BombShooter-020";
            public override string Name => "GuidedMissileLauncher";
            public override int Cost => 900;
            public override string DisplayName => "Guided Missile Launcher";
            public override string Description => "Launches missiles that home in on bloons.";
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 1;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range = 80.0f;
                attackModel.range = 80.0f;
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-002").GetAttackModel().weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3.0f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 0.33f;
                towerModel.ApplyDisplay<GuidedMissileLauncherDisplay>();
                attackModel.weapons[0].projectile.ApplyDisplay<GuidedMissileDisplay>();

            }
            public override string Portrait => "GuidedMissileLauncher_Icon";
            public override string Icon => "GuidedMissileLauncher_Icon";
        }
        public class GuidedMissileLauncherDisplay : ModDisplay
        {
            public override string BaseDisplay => "b0aa0dff63095b7489e76a12957dc390";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach(Renderer renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture(mod, "GuidedMissileLauncherDisplay");
                }
            }
        }
        public class GuidedMissileDisplay : ModDisplay
        {
            public override string BaseDisplay => "25806b6be5f85c54a84fefe2e95acfbc";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (Renderer renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture(mod, "GuidedMissileLauncherDisplay");
                }
            }
        }
        public class FasterMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "FasterMissiles";
            public override string DisplayName => "Faster Missiles";
            public override string Description => "Missiles and shot out of the launcher faster, and fly faster.";
            public override int Cost => 700;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate *= 0.8f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.25f;
            }
        }
    }

}