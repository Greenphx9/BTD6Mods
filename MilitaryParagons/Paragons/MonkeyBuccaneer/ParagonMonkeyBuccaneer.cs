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
using Assets.Scripts.Models.GenericBehaviors;

namespace MilitaryParagons.Paragons.Towers
{
    public class ParagonMonkeyBuccaneer
    {
        public static TowerModel MonkeyBuccaneerParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("MonkeyBuccaneer-520").Duplicate();
            TowerModel backup = model.GetTowerFromId("MonkeyBuccaneer-520").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "MonkeyBuccaneer";
            towerModel.name = "MonkeyBuccaneer-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "MonkeyBuccaneer Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("PirateEmpire_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("PirateEmpire_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("PirateEmpire_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<MonkeyBuccaneerParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<MonkeyBuccaneerParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.doesntRotate = false;
            towerModel.GetDescendants<DisplayModel>().ForEach(display => display.ignoreRotation = false);

            var attackModel = towerModel.GetAttackModel();
            towerModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.025f);
            towerModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.scale *= 0.8f);
            towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 5.0f);
            towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
            towerModel.GetAttackModels().Last().GetDescendants<DamageModel>().ForEach(damage => damage.damage = 100.0f);
            

            towerModel.AddBehavior(model.GetTowerFromId("MonkeyBuccaneer-050").GetAbility().Duplicate());
            towerModel.AddBehavior(new ActivateAbilityAfterIntervalModel("ActivateAbilityAfterIntervalModel_", towerModel.GetAbility(), 3.0f));
            towerModel.GetAbility().enabled = false;

            var tradeEmpire = model.GetTowerFromId("MonkeyBuccaneer-005").Duplicate();
            towerModel.AddBehavior(tradeEmpire.GetBehavior<PerRoundCashBonusTowerModel>());
            towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound *= 4.0f;
            towerModel.AddBehavior(tradeEmpire.GetBehavior<TradeEmpireBuffModel>());
            towerModel.AddBehavior(tradeEmpire.GetBehavior<CashbackZoneModel>());
            //towerModel.AddBehavior(towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
            //towerModel.RemoveBehavior<AbilityModel>();

            //var attackModel2 = towerModel.GetAttackModel(1);
            //attackModel2.weapons[0].Rate = 3.0f;

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class MonkeyBuccaneerParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("MonkeyBuccaneer-250").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("PirateEmpire_Display");
                }
            }
        }

    }
    
}
