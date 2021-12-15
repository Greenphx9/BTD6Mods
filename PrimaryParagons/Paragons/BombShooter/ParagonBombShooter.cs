using MelonLoader;
using HarmonyLib;

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
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using UnhollowerBaseLib;
using Assets.Scripts.Models.Towers.Upgrades;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using NinjaKiwi.Common;
using Assets.Scripts.Models.Towers.Filters;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api;

namespace PrimaryParagons.Paragons.Towers
{
    public class ParagonBombShooter
    {
        public class BombShooterParagon : ModVanillaParagon
        {
            public override string BaseTower => "BombShooter-520";
        }
        public class MOABExecutioner : ModParagonUpgrade<BombShooterParagon>
        {
            public override string DisplayName => "MOAB Executioner";
            public override int Cost => 900000;
            public override string Description => "Get too close, and you'll be blown to dust.";
            public override string Icon => "MOABExecutioner_Icon";
            public override string Portrait => "MOABExecutioner_Portrait";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 0.25f;
                attackModel.weapons[0].projectile.ApplyDisplay<BombShooterParagonDisplay_Projectile>();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-005").GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].Duplicate());

                var projectileModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                projectileModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 1000.0f, false, true));
                projectileModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 1000.0f, false, true));
                projectileModel.GetDamageModel().damage = 100.0f;
                projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                var clusterModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
                clusterModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                clusterModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 100.0f, false, true));
                clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
                clusterModel.pierce = 100.0f;
                clusterModel.maxPierce = 100.0f;
                clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce = 100.0f;
                clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.maxPierce = 100.0f;
                //clusterModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                //clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class MOABExecutionerDisplay : ModTowerDisplay<BombShooterParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.BombShooter, 0, 5, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("MOABExecutioner_Display");
                }
            }
        }
        /*public static TowerModel BombShooterParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("BombShooter-520").Duplicate();
            TowerModel backup = model.GetTowerFromId("BombShooter-520").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "BombShooter";
            towerModel.name = "BombShooter-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "BombShooter Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "Default";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;
            towerModel.isParagon = true;
            towerModel.icon = ModContent.GetSpriteReference<Main>("MOABExecutioner_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("MOABExecutioner_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("MOABExecutioner_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<BombShooterParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<BombShooterParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].Rate = 0.25f;
            attackModel.weapons[0].projectile.ApplyDisplay<BombShooterParagonDisplay_Projectile>();
            attackModel.weapons[0].projectile.AddBehavior(model.GetTowerFromId("BombShooter-005").GetWeapon().projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>()[1].Duplicate());

            var projectileModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
            projectileModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 1000.0f, false, true));
            projectileModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 1000.0f, false, true));
            projectileModel.GetDamageModel().damage = 100.0f;
            projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            var clusterModel = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile;
            clusterModel.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
            clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
            clusterModel.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1.0f, 100.0f, false, true));
            clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.AddBehavior(new DamageModifierForTagModel("Moabs", "Moabs", 1.0f, 100.0f, false, true));
            clusterModel.pierce = 100.0f;
            clusterModel.maxPierce = 100.0f;
            clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.pierce = 100.0f;
            clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.maxPierce = 100.0f;
            //clusterModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            //clusterModel.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }*/
        public class BombShooterParagonDisplay_Projectile : ModDisplay
        {
            public override string BaseDisplay => "e5edd901992846e409326a506d272633";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.GetRenderers<MeshRenderer>())
                {
                    renderer.material.mainTexture = GetTexture("MOABExecutioner_Display");
                }
            }
        }
    }
    
}
