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
    public class ParagonMortarMonkey
    {
        public static TowerModel MortarMonkeyParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("MortarMonkey-250").Duplicate();
            TowerModel backup = model.GetTowerFromId("MortarMonkey-250").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "MortarMonkey";
            towerModel.name = "MortarMonkey-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "MortarMonkey Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "MortarMonkey";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;
            towerModel.isParagon = true;
            towerModel.icon = ModContent.GetSpriteReference<Main>("BlooncinerationAwe_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("BlooncinerationAwe_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("BlooncinerationAwe_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<MortarMonkeyParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<MortarMonkeyParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            //var attackModel = towerModel.GetAttackModel();
            
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile = model.GetTowerFromId("MortarMonkey-520").GetAttackModel().weapons[0].projectile.Duplicate();
            attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 10.0f);
            foreach(var create in model.GetTowerFromId("MortarMonkey-205").GetDescendants<CreateProjectileOnExhaustFractionModel>().ToIl2CppList())
            {
                var proj = create.Duplicate();
                proj.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 7f);
                attackModel.weapons[0].projectile.AddBehavior(proj);
            }

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class MortarMonkeyParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("MortarMonkey-250").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "BlooncinerationAwe_Display");
                 
                
            }
        }
    }
    
}
