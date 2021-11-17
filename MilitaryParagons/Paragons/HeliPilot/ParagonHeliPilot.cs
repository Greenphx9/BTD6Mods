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
    public class ParagonHeliPilot
    {
        public static TowerModel HeliPilotParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("HeliPilot-502").Duplicate();
            TowerModel backup = model.GetTowerFromId("HeliPilot-502").Duplicate();
            //thanks to depletednova for this 
            //towerModel.baseId = "HeliPilot";
            towerModel.name = "HeliPilot-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "HeliPilot Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            /*towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "Default";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;*/
            towerModel.isParagon = true;
            towerModel.doesntRotate = true;
            towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
            towerModel.icon = ModContent.GetSpriteReference<Main>("ApacheCommander_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("ApacheCommander_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("ApacheCommander_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<HeliPilotParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<HeliPilotParagonDisplayPad>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
            //towerModel.GetBehavior<AirUnitModel>().display = model.GetTowerFromId("HeliPilot-502").Duplicate().GetBehavior<AirUnitModel>().display;

            var attackModel = towerModel.GetAttackModel();
            towerModel.GetDescendants<EmissionWithOffsetsModel>().ForEach(emission => emission.projectileCount = 3);
            towerModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= 0.33f);
            towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 3.0f);
            towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            towerModel.AddBehavior(model.GetTowerFromId("HeliPilot-050").GetAbilites().Last().Duplicate());
            towerModel.AddBehavior(model.GetTowerFromId("HeliPilot-050").GetAbilites()[0].Duplicate());

            towerModel.AddBehavior(model.GetTowerFromId("HeliPilot-050").GetAbilites()[1].GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
            towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 30.0f);
            towerModel.AddBehavior(model.GetTowerFromId("HeliPilot-050").GetAbilites()[1].GetBehavior<ActivateAttackModel>().attacks[1].Duplicate());
            towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 50.0f);

            towerModel.AddBehavior(model.GetTowerFromId("EngineerMonkey-200").GetAttackModel().Duplicate());

            var attackModel2 = towerModel.GetAttackModels().Last();
            var createTower = attackModel2.weapons[0].projectile.GetBehavior<CreateTowerModel>();
            createTower.tower = backup;
            createTower.tower.cost = 0.0f;
            createTower.tower.radius = 0.0f;
            createTower.tower.isSubTower = true;
            //createTower.tower.RemoveBehavior<DisplayModel>();
            createTower.tower.display = "";
            createTower.tower.RemoveBehavior<CreateSoundOnTowerPlaceModel>();
            createTower.tower.RemoveBehavior<CreateSoundOnSellModel>();
            createTower.tower.name += "ApacheCommander_Apache";
            createTower.tower.AddBehavior(model.GetTowerFromId("Sentry").GetBehavior<TowerExpireModel>().Duplicate());
            createTower.tower.GetBehavior<AirUnitModel>().display = ModContent.GetDisplayGUID<HeliPilotParagonDisplay>();
            createTower.tower.GetBehavior<TowerExpireModel>().Lifespan *= 2.0f;

            //towerModel.AddBehavior(model.GetTowerFromId("HeliPilot-205").GetBehavior<ComancheDefenceModel>().Duplicate());
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel = backup.Duplicate();
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel.cost = 0.0f;
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel.dontDisplayUpgrades = true;
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel.isSubTower = true;
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel.tier = 0;
            //towerModel.GetBehavior<ComancheDefenceModel>().towerModel.tiers = new int[] { 0, 0, 0 };



            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            createTower.tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            createTower.tower.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            return towerModel;
        }
        public class HeliPilotParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("HeliPilot-205").GetBehavior<AirUnitModel>().display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("ApacheCommander_Display");
                }
            }
        }
        public class HeliPilotParagonDisplayPad : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("HeliPilot-205").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {

            }
        }
    }
    
}
