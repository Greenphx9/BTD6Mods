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
using MilitaryParagons.Paragons.Towers;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Assets.Scripts.Simulation.Towers.Weapons;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Objects;
using UnhollowerRuntimeLib;

[assembly: MelonColor(ConsoleColor.Green)]
[assembly: MelonInfo(typeof(MilitaryParagons.Main), "Military Paragons", "1.0.0", "Greenphx")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MilitaryParagons
{
    public class Main : BloonsTD6Mod
    {

        public override string MelonInfoCsURL => "https://github.com/Greenphx9/BTD6Mods/blob/main/MilitaryParagons/Main.cs";
        public override string LatestURL => "https://github.com/Greenphx9/BTD6Mods/blob/main/MilitaryParagons/MilitaryParagons.dll?raw=true";

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Military Paragons loaded!");

        }
        static string[] ParagonTowers =
        {
            "SniperMonkey",
            "MonkeySub",
            "MonkeyBuccaneer",
            "MonkeyAce",
            "HeliPilot",
            "MortarMonkey",
            "DartlingGunner",
        };
        public override void OnGameModelLoaded(GameModel model)
        {
            base.OnGameModelLoaded(model);
            //thanks to depletednova for this 
            for(int i = 0; i < ParagonTowers.Count(); i++)
            {
                var baseTower = ParagonTowers[i];
                for (int tier = 0; tier <= 2; tier++)
                {
                    model.GetTower($"{baseTower}", 5, tier, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    model.GetTower($"{baseTower}", 5, 0, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    model.GetTower($"{baseTower}", tier, 5, 0).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    model.GetTower($"{baseTower}", 0, 5, tier).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    model.GetTower($"{baseTower}", tier, 0, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                    model.GetTower($"{baseTower}", 0, tier, 5).paragonUpgrade = new UpgradePathModel(upgrade: $"{baseTower} Paragon", tower: $"{baseTower}-Paragon");
                }
            }

            CreateUpgrade(model.GetTowerFromId("SniperMonkey"), 650000, ModContent.GetSpriteReference<Main>("EliteMOABCrippler_Icon"), model);
            model.AddTowerToGame(ParagonSniperMonkey.SniperMonkeyParagon(model));
            LocalizationManager.Instance.textTable.Add("SniperMonkey Paragon", "Elite MOAB Crippler");
            LocalizationManager.Instance.textTable.Add("SniperMonkey Paragon Description", "A fast firing, smart, MOAB crippling rifle can deal with almost anything.");
            CreateUpgrade(model.GetTowerFromId("MonkeySub"), 950000, ModContent.GetSpriteReference<Main>("FirstStrikeCommander_Icon"), model);
            model.AddTowerToGame(ParagonMonkeySub.MonkeySubParagon(model));
            LocalizationManager.Instance.textTable.Add("MonkeySub Paragon", "First Strike Commander");
            LocalizationManager.Instance.textTable.Add("MonkeySub Paragon Description", "A submarine that fires 20 deadly missiles every second. What could go wrong?");
            CreateUpgrade(model.GetTowerFromId("MonkeyBuccaneer"), 700000, ModContent.GetSpriteReference<Main>("PirateEmpire_Icon"), model);
            model.AddTowerToGame(ParagonMonkeyBuccaneer.MonkeyBuccaneerParagon(model));
            LocalizationManager.Instance.textTable.Add("MonkeyBuccaneer Paragon", "Pirate Empire");
            LocalizationManager.Instance.textTable.Add("MonkeyBuccaneer Paragon Description", "Thanks to the help from other pirates, this monkey can hook multiple Bloons in rapidly.");
            CreateUpgrade(model.GetTowerFromId("MonkeyAce"), 1000000, ModContent.GetSpriteReference<Main>("NevaMissingShredder_Icon"), model);
            model.AddTowerToGame(ParagonMonkeyAce.MonkeyAceParagon(model));
            LocalizationManager.Instance.textTable.Add("MonkeyAce Paragon", "Neva-Missing Shredder");
            LocalizationManager.Instance.textTable.Add("MonkeyAce Paragon Description", "If only the Bloons knew what was about to hit them.");
            CreateUpgrade(model.GetTowerFromId("HeliPilot"), 485000, ModContent.GetSpriteReference<Main>("ApacheCommander_Icon"), model);
            model.AddTowerToGame(ParagonHeliPilot.HeliPilotParagon(model));
            LocalizationManager.Instance.textTable.Add("HeliPilot Paragon", "Apache Commander");
            LocalizationManager.Instance.textTable.Add("HeliPilot Paragon Description", "Whats stronger than one Apache Prime? Multiple!");
            CreateUpgrade(model.GetTowerFromId("MortarMonkey"), 550000, ModContent.GetSpriteReference<Main>("BlooncinerationAwe_Icon"), model);
            model.AddTowerToGame(ParagonMortarMonkey.MortarMonkeyParagon(model));
            LocalizationManager.Instance.textTable.Add("MortarMonkey Paragon", "Blooncineration and Awe");
            LocalizationManager.Instance.textTable.Add("MortarMonkey Paragon Description", "Did you know if you combine huge damage, fast firing, and deadly flames, you get a super powerful monkey?");
            CreateUpgrade(model.GetTowerFromId("DartlingGunner"), 1400000, ModContent.GetSpriteReference<Main>("RayOfMAD_Icon"), model);
            model.AddTowerToGame(ParagonDartlingGunner.DartlingGunnerParagon(model));
            LocalizationManager.Instance.textTable.Add("DartlingGunner Paragon", "Ray of MAD");
            LocalizationManager.Instance.textTable.Add("DartlingGunner Paragon Description", "A machine so powerful not even Dr Monkey could make it. The MAD explosive bullets had to be less powerful for it to even be possible to make.");

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
        public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
        {
            base.OnTowerCreated(tower, target, modelToUse);
            if(tower.towerModel.name.Contains("ApacheCommander_Apache"))
            {
               
                tower.display.scaleOffset = new Assets.Scripts.Simulation.SMath.Vector3(0.5f, 0.5f, 0.5f);
                for(int i = 0; i < tower.towerBehaviors.count; i++)
                {
                    var beh = tower.towerBehaviors[i];
                    if(beh.GetIl2CppType() == UnhollowerRuntimeLib.Il2CppType.Of<AirUnit>())
                    {
                        var air = beh.Cast<AirUnit>();
                        air.display.scaleOffset = new Assets.Scripts.Simulation.SMath.Vector3(0.5f, 0.5f, 0.5f);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Weapon), nameof(Weapon.SpawnDart))]
        public class SpawnDart
        {
            [HarmonyPostfix]
            public static void Postfix(Weapon __instance)
            {
                try
                {
                    // code
                }
                catch (Exception e)
                {
                    
                }
            }
        }
        
       
    }

}