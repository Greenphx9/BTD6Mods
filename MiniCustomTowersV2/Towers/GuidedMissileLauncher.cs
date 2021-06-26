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
using UnhollowerBaseLib;

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
            public override int TopPathUpgrades => 4;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 4;
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
            public override string Name => "GuidedMissileDisplay";
        }
        public class HeavyBombs : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "HeavyBombs ";
            public override string DisplayName => "Heavy Bombs";
            public override string Description => "Bombs explode more bloons and deal more damage.";
            public override int Cost => 700;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius *= 1.1f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce *= 1.25f;
            }
            public override string Icon => "HeavyBombs_Icon";
        }
        public class HeavierBombs : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "HeavierBombs";
            public override string DisplayName => "Heavier Bombs";
            public override string Description => "Bombs explode more bloons and deal more damage.";
            public override int Cost => 1200;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.radius *= 1.25f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce *= 1.5f;
            }
            public override string Icon => "HeavyBombs_Icon";
        }
        public class ClusterMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "ClusterMissiles";
            public override string DisplayName => "Cluster Missiles";
            public override string Description => "8 missiles explode out of the original on contact.";
            public override int Cost => 1500;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var proj = attackModel.weapons[0].projectile.Duplicate();
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", proj, new ArcEmissionModel("ArcEmissionModel_", 8, 0.0f, 360.0f, null, false, false), true, false, false));
                attackModel.weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>()[1].projectile.RemoveBehavior<TrackTargetModel>();
            }
            public override string Icon => "ClusterMissiles_Icon";
        }
        public class HomingClusterMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "HomingClusterMissiles";
            public override string DisplayName => "Homing Cluster Missiles";
            public override string Description => "Cluster missiles also home in on bloons!";
            public override int Cost => 12000;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>()[1].projectile.AddBehavior(attackModel.weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate());
            }
            public override string Icon => "HomingClusterMissiles_Icon";
        }
        public class FasterMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "FasterMissiles";
            public override string DisplayName => "Faster Missiles";
            public override string Description => "Missiles are shot out of the missile launcher faster, and fly faster.";
            public override int Cost => 700;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate *= 0.8f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.25f;
            }
            public override string Icon => "FasterMissiles_Icon";
        }
        public class LongerRangeMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "LongerRangeMissiles";
            public override string DisplayName => "Longer Range Missiles";
            public override string Description => "Missiles can target bloons from further away.";
            public override int Cost => 750;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range += 30.0f;
                attackModel.range = towerModel.range;
            }
            public override string Icon => "LongerRangeMissiles_Icon";
        }
        public class UltraMissiles : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "UltraMissiles";
            public override string DisplayName => "Ultra Missiles";
            public override string Description => "Missiles are shot out of the missile launcher even faster, and fly even faster.";
            public override int Cost => 2800;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate *= 0.5f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 1.3f;
            }
            public override string Icon => "FasterMissiles_Icon";
        }
        public class MissileOverload : ModUpgrade<GuidedMissileLauncher>
        {
            public override string Name => "MissileOverload";
            public override string DisplayName => "Missile Overload";
            public override string Description => "Ability: The guided missile launcher shoots missiles WAY faster for a short amount of time.";
            public override int Cost => 16000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BoomerangMonkey-040").GetAbility().Duplicate());
                towerModel.GetAbility().GetBehavior<TurboModel>().extraDamage += 1;
                towerModel.GetAbility().GetBehavior<TurboModel>().multiplier /= 2;
                towerModel.GetAbility().GetBehavior<TurboModel>().projectileDisplay.assetPath = "MiniCustomTowers_V2_GuidedMissileDisplay";
                towerModel.GetAbility().icon = GetSpriteReference(mod, "GuidedMissileLauncher_Icon");
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1.0f;
            }
            public override string Icon => "HomingClusterMissiles_Icon";
        }
    }

}