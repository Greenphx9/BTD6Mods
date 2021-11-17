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

namespace MilitaryParagons.Paragons.Towers
{
    public class ParagonSniperMonkey
    {
        public static TowerModel SniperMonkeyParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("SniperMonkey-025").Duplicate();
            TowerModel backup = model.GetTowerFromId("SniperMonkey-025").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "SniperMonkey";
            towerModel.name = "SniperMonkey-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "SniperMonkey Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("EliteMOABCrippler_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("EliteMOABCrippler_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("EliteMOABCrippler_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<SniperMonkeyParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<SniperMonkeyParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].Rate = 0.02f;
            attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 40.0f;
            attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().damage = 10.0f;
            attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.pierce = 100.0f;
            attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[0].projectile.AddBehavior(model.GetTowerFromId("SniperMonkey-500").GetWeapon().projectile.GetBehavior<SlowMaimMoabModel>().Duplicate());

            towerModel.AddBehavior(new ActivateAbilityOnRoundStartModel("AAORSM", model.GetTowerFromId("SniperMonkey-250").GetAbility().Duplicate()));
            towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.Cooldown = 30.0f;
            towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.resetCooldownOnTierUpgrade = false;
            towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.maximum = 10000f);
            towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.minimum = 10000f);
            towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.enabled = false;
            
            towerModel.AddBehavior(towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel);

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class SniperMonkeyParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("SniperMonkey-025").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach(var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("EliteMOABCrippler_Display");
                }
            }
        }
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
