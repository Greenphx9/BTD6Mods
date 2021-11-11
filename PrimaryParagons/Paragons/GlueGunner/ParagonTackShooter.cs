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
    public class ParagonTackShooter
    {
        public static TowerModel TackShooterParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("TackShooter-205").Duplicate();
            TowerModel backup = model.GetTowerFromId("TackShooter-205").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "TackShooter";
            towerModel.name = "TackShooter-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "TackShooter Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("FieryDoom_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("FieryDoom_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("FieryDoom_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<TackShooterParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<TackShooterParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            towerModel.range *= 2.0f;
            attackModel.range *= 2.0f;
            attackModel.weapons[0].Rate = 0.05f;
            attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count *= 2;

            var projectileModel = attackModel.weapons[0].projectile;
            projectileModel.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
            projectileModel.display = "c184360c85b9d70499bb2fff7c77ecb2";
            projectileModel.pierce = 100.0f;
            projectileModel.GetDamageModel().damage = 75f;
            projectileModel.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            towerModel.AddBehavior(model.GetTowerFromId("WizardMonkey-030").GetAttackModel(3).Duplicate());

            var attackModel2 = towerModel.GetAttackModel(1);
            attackModel2.range = 2000.0f;
            attackModel2.fireWithoutTarget = true;
            attackModel2.weapons[0].Rate = 5.0f;
            attackModel2.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 250, 0.0f, 360.0f, null, false);
            attackModel2.weapons[0].projectile.pierce = 50.0f;
            attackModel2.weapons[0].projectile.GetDamageModel().damage = 100.0f;
            attackModel2.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 100.0f;
            attackModel2.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
            attackModel2.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            towerModel.AddBehavior(model.GetTowerFromId("TackShooter-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());

            var attackModel3 = towerModel.GetAttackModel(2);
            attackModel3.range = 1000.0f;
            attackModel3.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenModel_"));
            attackModel3.weapons[0].emission.Cast<ArcEmissionModel>().count = 6;
            attackModel3.weapons[0].projectile.GetDamageModel().damage = 20.0f;
            attackModel3.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class TackShooterParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("TackShooter-005").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("FieryDoom_Display");
                }
            }
        }
        /*public class TackShooterParagonDisplay_Projectile : ModDisplay
        {
            public override string BaseDisplay => "e5edd901992846e409326a506d272633";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.GetRenderers<MeshRenderer>())
                {
                    renderer.material.mainTexture = GetTexture("MOABExecutioner_Display");
                }
            }
        }*/
    }
    
}
