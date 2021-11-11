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
using PrimaryParagons.Paragons.Towers;
using BTD_Mod_Helper.Api;

[assembly: MelonInfo(typeof(PrimaryParagons.Main), "Primary Paragons", "1.0.0", "Greenphx")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace PrimaryParagons
{
    public class Main : BloonsTD6Mod
    {

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Primary Paragons loaded!");

        }
        public override void OnGameModelLoaded(GameModel model)
        {
            base.OnGameModelLoaded(model);
            //thanks to depletednova for this 
            var baseTower = "BombShooter";
            for (int tier = 0; tier <= 2; tier++)
            {
                model.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
            }
            baseTower = "TackShooter";
            for (int tier = 0; tier <= 2; tier++)
            {
                model.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
            }
            baseTower = "GlueGunner";
            for (int tier = 0; tier <= 2; tier++)
            {
                model.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
            }
            baseTower = "IceMonkey";
            for (int tier = 0; tier <= 2; tier++)
            {
                model.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                model.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
            }
            CreateUpgrade(model.GetTowerFromId("BombShooter"), 900000, ModContent.GetSpriteReference<Main>("MOABExecutioner_Icon"), model);
            model.AddTowerToGame(ParagonBombShooter.BombShooterParagon(model));
            LocalizationManager.Instance.textTable.Add("BombShooter Paragon", "MOAB Executioner");
            LocalizationManager.Instance.textTable.Add("BombShooter Paragon Description", "Get too close, and you'll be blown to dust.");

            CreateUpgrade(model.GetTowerFromId("TackShooter"), 1200000, ModContent.GetSpriteReference<Main>("FieryDoom_Icon"), model);
            model.AddTowerToGame(ParagonTackShooter.TackShooterParagon(model));
            LocalizationManager.Instance.textTable.Add("TackShooter Paragon", "Fiery Doom");
            LocalizationManager.Instance.textTable.Add("TackShooter Paragon Description", "Flaming tacks and blades so hot that not even purple Bloons are immune.");

            CreateUpgrade(model.GetTowerFromId("GlueGunner"), 600000, ModContent.GetSpriteReference<Main>("SuperbGlue_Icon"), model);
            model.AddTowerToGame(ParagonGlueGunner.GlueGunnerParagon(model));
            LocalizationManager.Instance.textTable.Add("GlueGunner Paragon", "Superb Glue");
            LocalizationManager.Instance.textTable.Add("GlueGunner Paragon Description", "Glue that completely stops almost all Bloons and decimates every type of Bloon. Bloons affected by glue take extra damage.");

            CreateUpgrade(model.GetTowerFromId("IceMonkey"), 400000, model.GetUpgrade("Snowstorm").icon, model);
            model.AddTowerToGame(ParagonIceMonkey.IceMonkeyParagon(model));
            LocalizationManager.Instance.textTable.Add("IceMonkey Paragon", "0° Kelvin");
            LocalizationManager.Instance.textTable.Add("IceMonkey Paragon Description", "Only the strongest of Bloons are able to resist the cold icy winds.");
        }
        public void CreateUpgrade(TowerModel towerModel, int price, SpriteReference icon, GameModel model)
        {
            //thanks to depletednova for this 
            UpgradeModel upgradeModel = new UpgradeModel(
            name: towerModel.baseId + " Paragon",
            cost: price,
            xpCost: 0,
            icon: icon,
            path: -1,
            tier: 5,
            locked: 0,
            confirmation: "Paragon",
            localizedNameOverride: ""
            );
            model.AddUpgrade(upgradeModel);
        }

    }

}