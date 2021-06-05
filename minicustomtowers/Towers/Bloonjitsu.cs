using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Main.Scenes;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Powers;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.Localization;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.Upgrade;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem.Collections.Generic;
using MelonLoader;
//using NKHook6.Api.Events;
using UnhollowerBaseLib;
using UnityEngine;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using System.Net;
using Assets.Scripts.Unity.UI_New.Popups;
using TMPro;
using System;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Simulation;
using static Assets.Scripts.Simulation.Simulation;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Simulation.Towers.Projectiles;
using MelonLoader;
using Harmony;

namespace minicustomtowers.Towers
{
    class Bloonjitsu
    {
        //https://github.com/gurrenm3/BloonsTD6-Mod-Helper/releases



                //if (!File.Exists("Mods/NKHook6.dll"))
                //{
                //    using (WebClient client = new WebClient())
                //    {
                //        client.DownloadFile("https://github.com/TDToolbox/NKHook6/releases/download/41/NKHook6.dll", "Mods/NKHook6.dll");
                //    }
                //    Console.WriteLine("downloaded NKHook6.dll");
                //}
                //if (!File.Exists("Mods/BloonsTD6.Mod.Helper.dll"))
                //{
                //    Il2CppSystem.Action<int> mod = (Il2CppSystem.Action<int>)delegate (int s)
                //    {
                //        if (s == 1) {
                //            using (WebClient client = new WebClient())
                //            {
                //                client.DownloadFile("https://github.com/gurrenm3/BloonsTD6-Mod-Helper/releases/download/0.0.2/BloonsTD6.Mod.Helper.dll", "Mods/BloonsTD6.Mod.Helper.dll");
                //                File.Delete("Mods/BloonsTD6_Mod_Helper.dll");
                //            }
                //            Console.WriteLine("downloaded BloonsTD6.Mod.Helper.dll");
                //            Application.Quit(0);
                //        }


                //    };

                //    //PopupScreen.instance.ShowSetValuePopup("your btd6 mod helper seems to be outdated", "type 1 to update it", mod, 1);

                //    //PopupScreen.instance.GetFirstActivePopup().GetComponentInChildren<TMP_InputField>().characterValidation = TMP_InputField.CharacterValidation.None;

                //}


                public static void Init()
        { 
                Console.WriteLine("Initializing Bloonjitsu");

                if (!LocalizationManager.instance.textTable.ContainsKey(customTowerName))
                {
                    LocalizationManager.instance.textTable.Add(customTowerName, "Bloonjitsu");
                }
                string[] array = new string[]
                {
                customTowerUpgrade1,
                customTowerUpgrade2,
                customTowerUpgrade3,
                customTowerUpgrade4,
                customTowerUpgrade5
                };
                string[] array2 = new string[]
                {
                "Blows back bloons.",
                "+2 Pierce.",
                "+1 Shuriken.",
                "Every 5 shots, shoots out flamin' hot shurikens. They can pop leads.",
                "+Fire-rate, +10 pierce, +1 Shuriken."
                };

                for (int i = 0; i < array.Length; i++)
                {
                    if (!LocalizationManager.instance.textTable.ContainsKey(array[i] + " Description"))
                    {
                        LocalizationManager.instance.textTable.Add(array[i] + " Description", array2[i]);
                    }
                }

                Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\010.png", "https://i.imgur.com/xOFofPY.png", default, out customTowerIcon010);
            Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\020.png", "https://i.imgur.com/SQ8ZPZh.png", default, out customTowerIcon020);
            Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\040.png", "https://i.imgur.com/avMipN2.png", default, out customTowerIcon040);
            Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\050.png", "https://i.imgur.com/NJO6PqM.png", default, out customTowerIcon050);



            System.Collections.Generic.List<UpgradeModel> list = new System.Collections.Generic.List<UpgradeModel>();
                list.Add(new UpgradeModel(customTowerUpgrade1, 650, 0, new SpriteReference(customTowerIcon010), 1, 1, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade2, 1250, 0, new SpriteReference(customTowerIcon020), 1, 2, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade3, 1350, 0, new SpriteReference(customTowerIcon020), 1, 3, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade4, 3450, 0, new SpriteReference(customTowerIcon040), 1, 4, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade5, 19520, 0, new SpriteReference(customTowerIcon050), 1, 5, 0, "", ""));
                Game.instance.model.upgrades = Game.instance.model.upgrades.Add(list);
                System.Collections.Generic.List<TowerModel> list2 = new System.Collections.Generic.List<TowerModel>();
                list2.Add(getT0(Game.instance.model));
                list2.Add(getT1(Game.instance.model));
                list2.Add(getT2(Game.instance.model));
                list2.Add(getT3(Game.instance.model));
                list2.Add(getT4(Game.instance.model));
                list2.Add(getT5(Game.instance.model));
                Game.instance.model.towers = Game.instance.model.towers.Add(list2);
                System.Collections.Generic.List<TowerDetailsModel> list3 = new System.Collections.Generic.List<TowerDetailsModel>();
                foreach (TowerDetailsModel item in Game.instance.model.towerSet)
                {
                    list3.Add(item);
                }
                ShopTowerDetailsModel newPart = new ShopTowerDetailsModel(customTowerName, (int)Game.instance.model.GetTowerFromId("Druid").GetIndex(), 0, 5, 0, -1, 0, null);
                Game.instance.model.towerSet = Game.instance.model.towerSet.Add(newPart);
                bool flag = false;
                foreach (TowerDetailsModel towerDetailsModel in Game.instance.model.towerSet)
                {
                    if (flag)
                    {
                        int towerIndex = towerDetailsModel.towerIndex;
                        towerDetailsModel.towerIndex = towerIndex + 1;
                    }
                    if (towerDetailsModel.towerId.Contains(customTowerName))
                    {
                        flag = true;
                    }
                }
            CacheBuilder.toBuild.PushAll("FlaminShuriken");
            Console.WriteLine("Bloonjitsu Initialized!");
            }
   

        static string customTowerImageID;
        static string customTowerIcon010;
        static string customTowerIcon020;
        static string customTowerIcon050;
        static string customTowerIcon040;
        static string customTowerImages = @"Mods/cobramonkey/";
        static string customTowerName = "Bloonjitsu";
        static string customTowerDisplay = "";
        static string customTowerUpgrade1 = "Bloon Distraction";
        static string customTowerUpgrade2 = "Sharper Shurikens";
        static string customTowerUpgrade3 = "More Shurikens";
        static string customTowerUpgrade4 = "Flamin' Hot Shurikens";
        static string customTowerUpgrade5 = "Shuriken Mastery";





        public static TowerModel getT0(GameModel gameModel)
        {
            TowerModel towerModel = gameModel.GetTowerFromId("NinjaMonkey-402").Duplicate<TowerModel>(); //gameModel.GetTowerFromId(Alchemist).Duplicate<TowerModel>();
            towerModel.name = customTowerName;
            towerModel.baseId = customTowerName;
            towerModel.portrait = new SpriteReference(guid: "d8c64bfe0bba44433ab6b47cf34b37b9");
            towerModel.display = "9f25bf209b143894da9e6e2cf353c78c";
            towerModel.instaIcon = new SpriteReference(guid: "d8c64bfe0bba44433ab6b47cf34b37b9");
            towerModel.icon = new SpriteReference(guid: "d8c64bfe0bba44433ab6b47cf34b37b9");
            towerModel.GetBehavior<DisplayModel>().display = "9f25bf209b143894da9e6e2cf353c78c";
            towerModel.towerSet = "Primary";
            towerModel.dontDisplayUpgrades = false;
            towerModel.cost = 2900;
            towerModel.isGlobalRange = false;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                        new UpgradePathModel(customTowerUpgrade1, customTowerName + "-010", 1, 1)
            });
            towerModel.tiers = new int[] { 0, 0, 0 };
            var attackModel = towerModel.GetBehavior<AttackModel>();
            return towerModel;

        }


        public static TowerModel getT1(GameModel gameModel)
        {
            TowerModel towerModel = getT0(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-010";
            towerModel.tier = 1;
            towerModel.tiers = new int[] { 0, 1, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade2, customTowerName + "-020", 1, 2)
            });
            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            WindModel windModel = gameModel.GetTowerFromId("NinjaMonkey-020").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate<WindModel>();
            attackModel.weapons[0].projectile.AddBehavior(windModel);
            return towerModel;
        }


        public static TowerModel getT2(GameModel gameModel)
        {
            TowerModel towerModel = getT1(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-020";
            towerModel.tier = 2;
            towerModel.tiers = new int[] { 0, 2, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1,
                    customTowerUpgrade2
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade3, customTowerName + "-030", 1, 3)
            });

            //balance stuff
            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].projectile.pierce += 2f;
            return towerModel;
        }


        public static TowerModel getT3(GameModel gameModel)
        {
            TowerModel towerModel = getT2(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-030";
            towerModel.tier = 3;
            towerModel.tiers = new int[] { 0, 3, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1,
                    customTowerUpgrade2,
                    customTowerUpgrade3
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade4, customTowerName + "-040", 1, 4)
            });


            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count = 6;
            return towerModel;
        }


        public static TowerModel getT4(GameModel gameModel)
        {
            TowerModel towerModel = getT3(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-040";
            towerModel.tier = 4;
            towerModel.tiers = new int[] { 0, 4, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1,
                    customTowerUpgrade2,
                    customTowerUpgrade3,
                    customTowerUpgrade4
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade5, customTowerName + "-050", 1, 5)
            });


            //balance stuff
            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            var flameProj = gameModel.GetTowerFromId("WizardMonkey-010").GetAttackModel(1).weapons[0].projectile.Duplicate<ProjectileModel>();
            attackModel.weapons[1].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            attackModel.weapons[1].Rate *= 5f;
            attackModel.weapons[1].projectile.display = "FlaminShuriken";
            //attackModel.weapons[1].projectile.display = flameProj.display;
            //attackModel.weapons[1].projectile.display = "SharpShuriken";
            var trackTarget = attackModel.weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate<TrackTargetModel>();
            attackModel.weapons[1].projectile.AddBehavior(trackTarget);
            WindModel windModel = gameModel.GetTowerFromId("NinjaMonkey-020").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate<WindModel>();
            attackModel.weapons[1].projectile.AddBehavior(windModel);

            return towerModel;
        }


        public static TowerModel getT5(GameModel gameModel)
        {
            TowerModel towerModel = getT4(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-050";
            towerModel.tier = 5;
            towerModel.tiers = new int[] { 0, 5, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1,
                    customTowerUpgrade2,
                    customTowerUpgrade3,
                    customTowerUpgrade4,
                    customTowerUpgrade5
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[0]);


            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].Rate /= 5f;
            attackModel.weapons[1].Rate /= 5f;
            attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count = 8;
            attackModel.weapons[1].emission.Cast<ArcEmissionModel>().count = 8;
            attackModel.weapons[0].projectile.pierce += 10f;
            attackModel.weapons[1].projectile.pierce += 10f;
            return towerModel;

        }









        [HarmonyPatch(typeof(ProfileModel), "Validate")]
        public class ProfileModel_Patch
        {

            [HarmonyPostfix]
            public static void Postfix(ref ProfileModel __instance)
            {
                var unlockedTowers = __instance.unlockedTowers;
                var acquiredUpgrades = __instance.acquiredUpgrades;
                //if (unlockedTowers.Contains(customTowerName)) return;

                unlockedTowers.Add(customTowerName);

                string[] array = new string[]
                {
                    customTowerUpgrade1,
                    customTowerUpgrade2,
                    customTowerUpgrade3,
                    customTowerUpgrade4,
                    customTowerUpgrade5
                };
                checked
                {
                    for (int i = 0; i < array.Length; i++)
                    {

                        acquiredUpgrades.Add(array[i]);
                    }
                }
            }
        }





        public static Texture2D TextureFromPNG(string path)
        {
            Texture2D text = new Texture2D(2, 2);

            if (!ImageConversion.LoadImage(text, File.ReadAllBytes(path)))
            {
                throw new Exception("Could not acquire texture from file " + Path.GetFileName(path) + ".");
            }

            return text;
        }
       
    }
}
