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
    class CobraMonkey
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
                //Console.WriteLine("Initializing Bloonjitsu");

                if (!LocalizationManager.instance.textTable.ContainsKey(customTowerName))
                {
                    LocalizationManager.instance.textTable.Add(customTowerName, "Cobra Monkey");
                }
                string[] array = new string[]
                {
                customTowerUpgrade1U,
                customTowerUpgrade2U,
                customTowerUpgrade3U,
                customTowerUpgrade4U,
                customTowerUpgrade5U,
                customTowerUpgrade1D,
                customTowerUpgrade2D,
                customTowerUpgrade3D,
                customTowerUpgrade4D,
                customTowerUpgrade5D,
                };
                string[] array2 = new string[]
                {
                "Generates $80 per round.",
                "Every 15 seconds, takes 5 layers of a bloon. Also removes, camo, regrow, and fortified modifiers from the bloon.",
                "All monkeys in radius attack 1.25x faster.",
                "Every second, all bloons in radius are damaged for 5 damage.",
                "Generates $5000 per round.",
                "Cobra Monkey shoots twice as fast.",
                "Generates a live per round.",
                "Bloons in radius are slowed.",
                "Every 5 seconds, knocks a MOAB back.",
                "Generates $5000 per round.",
                };

                for (int i = 0; i < array.Length; i++)
                {
                    if (!LocalizationManager.instance.textTable.ContainsKey(array[i] + " Description"))
                    {
                        LocalizationManager.instance.textTable.Add(array[i] + " Description", array2[i]);
                    }
                }

                //Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\010.png", "https://i.imgur.com/xOFofPY.png", default, out customTowerIcon010);
            //Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\020.png", "https://i.imgur.com/SQ8ZPZh.png", default, out customTowerIcon020);
            //Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\040.png", "https://i.imgur.com/avMipN2.png", default, out customTowerIcon040);
            //Game.instance.GetSpriteRegister().RegisterSpriteFromURL(@"Mods\minicustomtowers\bloonjitsu\050.png", "https://i.imgur.com/NJO6PqM.png", default, out customTowerIcon050);



            System.Collections.Generic.List<UpgradeModel> list = new System.Collections.Generic.List<UpgradeModel>();
                list.Add(new UpgradeModel(customTowerUpgrade1U, 400, 0, new SpriteReference(guid: "WiredFunds"), 0, 1, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade2U, 650, 0, new SpriteReference(guid: "BloonAdjustment"), 0, 2, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade3U, 2560, 0, new SpriteReference(guid: "MonkeyStim"), 0, 3, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade4U, 3405, 0, new SpriteReference(guid: "OffensivePush"), 0, 4, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade5U, 25000, 0, new SpriteReference(guid: "A"), 0, 5, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade1D, 595, 0, new SpriteReference(guid: "DoubleTap"), 2, 1, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade2D, 1200, 0, new SpriteReference(guid: "Attrition"), 2, 2, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade3D, 18900, 0, new SpriteReference(guid: "FinishHim"), 2, 3, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade4D, 21500, 0, new SpriteReference(guid: "Misdirection"), 2, 4, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade5D, 21500, 0, new SpriteReference(guid: "A"), 2, 5, 0, "", ""));
            Game.instance.model.upgrades = Game.instance.model.upgrades.Add(list);
                System.Collections.Generic.List<TowerModel> list2 = new System.Collections.Generic.List<TowerModel>();
                list2.Add(getT0(Game.instance.model));
                list2.Add(getT1U(Game.instance.model));
                list2.Add(getT2U(Game.instance.model));
                list2.Add(getT3U(Game.instance.model));
                list2.Add(getT4U(Game.instance.model));
                list2.Add(getT5U(Game.instance.model));
                list2.Add(getT1D(Game.instance.model));
                list2.Add(getT2D(Game.instance.model));
                list2.Add(getT3D(Game.instance.model));
                list2.Add(getT4D(Game.instance.model));
                list2.Add(getT5D (Game.instance.model));
            Game.instance.model.towers = Game.instance.model.towers.Add(list2);
                System.Collections.Generic.List<TowerDetailsModel> list3 = new System.Collections.Generic.List<TowerDetailsModel>();
                foreach (TowerDetailsModel item in Game.instance.model.towerSet)
                {
                    list3.Add(item);
                }
                ShopTowerDetailsModel newPart = new ShopTowerDetailsModel(customTowerName, (int)Game.instance.model.GetTowerFromId("Druid").GetIndex(), 4, 0, 4, -1, 0, null);
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
            CacheBuilder.toBuild.PushAll("CobraMonkey", "WiredFunds", "BloonAdjustment", "MonkeyStim", "OffensivePush", "DoubleTap", "Attrition", "FinishHim", "Misdirection");
            //Console.WriteLine("Bloonjitsu Initialized!");
            }
   

        static string customTowerImageID;
        static string customTowerIcon010;
        static string customTowerIcon020;
        static string customTowerIcon050;
        static string customTowerIcon040;
        static string customTowerImages = @"Mods/cobramonkey/";
        static string customTowerName = "Cobra Monkey";
        static string customTowerDisplay = "";
        static string customTowerUpgrade1U = "Wired Funds";
        static string customTowerUpgrade2U = "Bloon Adjustment";
        static string customTowerUpgrade3U = "Monkey Stim";
        static string customTowerUpgrade4U = "Offensive Push";
        static string customTowerUpgrade5U = "IGNORE ME";
        static string customTowerUpgrade1D = "Double Tap";
        static string customTowerUpgrade2D = "Attrition";
        static string customTowerUpgrade3D = "Finish Him!";
        static string customTowerUpgrade4D = "Misdirection";
        static string customTowerUpgrade5D = " IGNORE ME";





        public static TowerModel getT0(GameModel gameModel)
        {
            TowerModel towerModel = gameModel.GetTowerFromId("SniperMonkey").Duplicate<TowerModel>(); //gameModel.GetTowerFromId(Alchemist).Duplicate<TowerModel>();
            towerModel.name = customTowerName;
            towerModel.baseId = customTowerName;
            towerModel.dontDisplayUpgrades = false;
            towerModel.cost = 425;
            towerModel.isGlobalRange = false;
            towerModel.portrait = new SpriteReference(guid: "CobraMonkey");
            towerModel.icon = new SpriteReference(guid: "CobraMonkey");
            towerModel.instaIcon = new SpriteReference(guid: "CobraMonkey");
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                        new UpgradePathModel(customTowerUpgrade1U, customTowerName + "-100", 1, 1),
                        new UpgradePathModel(customTowerUpgrade1D, customTowerName + "-001", 1, 1)
            });
            towerModel.tiers = new int[] { 0, 0, 0 };
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].Rate = 1.2f;
            towerModel.range = gameModel.GetTowerFromId("DartMonkey-001").range;
            attackModel.range = towerModel.range;
            return towerModel;

        }


        public static TowerModel getT1U(GameModel gameModel)
        {
            TowerModel towerModel = getT0(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-100";
            towerModel.tier = 1;
            towerModel.tiers = new int[] { 1, 0, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1U
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade2U, customTowerName + "-200", 1, 2)
            });
            towerModel.AddBehavior<PerRoundCashBonusTowerModel>(new PerRoundCashBonusTowerModel("wiredfunds", 80.0f, 0.0f, 1.0f, "80178409df24b3b479342ed73cffb63d", false));
            return towerModel;
        }


        public static TowerModel getT2U(GameModel gameModel)
        {
            TowerModel towerModel = getT1U(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-200";
            towerModel.tier = 2;
            towerModel.tiers = new int[] { 2, 0, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1U,
                    customTowerUpgrade2U
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade3U, customTowerName + "-300", 1, 3)
            });

            //balance stuff
            var attackModel = towerModel.GetAttackModel();
            towerModel.AddBehavior<AttackModel>(towerModel.GetAttackModel().Duplicate());
            towerModel.GetAttackModel(1).range = 500.0f;
            towerModel.GetAttackModel(1).weapons[0].Rate = 15f;
            towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage = 5f;
            towerModel.GetAttackModel(1).weapons[0].projectile.AddBehavior<RemoveBloonModifiersModel>(new RemoveBloonModifiersModel("bloonadjust", true, true, false, true, false, new Il2CppStringArray(0)));
            towerModel.AddBehavior<OverrideCamoDetectionModel>(new OverrideCamoDetectionModel("bloonadjust1", true));
            return towerModel;
        }


        public static TowerModel getT3U(GameModel gameModel)
        {
            TowerModel towerModel = getT2U(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-300";
            towerModel.tier = 3;
            towerModel.tiers = new int[] { 3, 0, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1U,
                    customTowerUpgrade2U,
                    customTowerUpgrade3U
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade4U, customTowerName + "-400", 1, 4)
            });
            var attackModel = towerModel.GetAttackModel();
            towerModel.AddBehavior<RateSupportModel>(gameModel.GetTowerFromId("MonkeyVillage-200").GetBehavior<RateSupportModel>().Duplicate());
            towerModel.GetBehavior<RateSupportModel>().multiplier = 0.75f;
            return towerModel;
        }


        public static TowerModel getT4U(GameModel gameModel)
        {
            TowerModel towerModel = getT3U(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-400";
            towerModel.tier = 4;
            towerModel.tiers = new int[] { 4, 0, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1U,
                    customTowerUpgrade2U,
                    customTowerUpgrade3U,
                    customTowerUpgrade4U
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade5U, customTowerName + "-500", 1, 5)
            });


            //balance stuff
            var attackModel = towerModel.GetAttackModel();
            attackModel.AddWeapon(gameModel.GetTowerFromId("TackShooter-402").GetAttackModel().weapons[0].Duplicate());
            attackModel.weapons[1].Rate = 1.0f;
            attackModel.weapons[1].projectile.display = "a";
            attackModel.weapons[1].projectile.pierce = 9999f;
            attackModel.weapons[1].projectile.GetBehavior<DisplayModel>().display = "a";
            attackModel.weapons[1].GetBehavior<EjectEffectModel>().effectModel.assetId = "a";
            attackModel.weapons[1].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            return towerModel;
        }


        public static TowerModel getT5U(GameModel gameModel)
        {
            TowerModel towerModel = getT4U(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-500";
            towerModel.tier = 5;
            towerModel.tiers = new int[] { 5, 0, 0 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1U,
                    customTowerUpgrade2U,
                    customTowerUpgrade3U,
                    customTowerUpgrade4U,
                    customTowerUpgrade5U
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[0]);
            return towerModel;

        }

        public static TowerModel getT1D(GameModel gameModel)
        {
            TowerModel towerModel = getT0(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-001";
            towerModel.tier = 1;
            towerModel.tiers = new int[] { 0, 0, 1 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1D
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade2D, customTowerName + "-002", 1, 2)
            });
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].Rate = 0.6f;
            return towerModel;
        }


        public static TowerModel getT2D(GameModel gameModel)
        {
            TowerModel towerModel = getT1D(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-002";
            towerModel.tier = 2;
            towerModel.tiers = new int[] { 0, 0, 2 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1D,
                    customTowerUpgrade2D
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade3D, customTowerName + "-003", 1, 3)
            });

            //balance stuff
            towerModel.AddBehavior<BonusLivesPerRoundModel>(new BonusLivesPerRoundModel("attrition", 1, 1.0f, "eb70b6823aec0644c81f873e94cb26cc"));
            return towerModel;
        }


        public static TowerModel getT3D(GameModel gameModel)
        {
            TowerModel towerModel = getT2D(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-003";
            towerModel.tier = 3;
            towerModel.tiers = new int[] { 0, 0, 3 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1D,
                    customTowerUpgrade2D,
                    customTowerUpgrade3D
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade4D, customTowerName + "-004", 1, 4)
            });
            var attackModel = towerModel.GetAttackModel();
            towerModel.AddBehavior<SlowBloonsZoneModel>(gameModel.GetTowerFromId("NaturesWardTotem").GetBehavior<SlowBloonsZoneModel>().Duplicate());
            towerModel.GetBehavior<SlowBloonsZoneModel>().zoneRadius = attackModel.range;
            towerModel.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.1f;
            towerModel.GetBehavior<SlowBloonsZoneModel>().bloonTag = "a";
            towerModel.GetBehavior<SlowBloonsZoneModel>().bloonTags = new Il2CppStringArray(0);
            towerModel.GetBehavior<SlowBloonsZoneModel>().inclusive = false;
            return towerModel;
        }


        public static TowerModel getT4D(GameModel gameModel)
        {
            TowerModel towerModel = getT3D(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-004";
            towerModel.tier = 4;
            towerModel.tiers = new int[] { 0, 0, 4 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1D,
                    customTowerUpgrade2D,
                    customTowerUpgrade3D,
                    customTowerUpgrade4D,
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                    new UpgradePathModel(customTowerUpgrade5D, customTowerName + "-005", 1, 5)
            });


            //balance stuff
            var attackModel = towerModel.GetAttackModel();
            attackModel.AddWeapon(gameModel.GetTowerFromId("PatFusty 11").GetAttackModel().weapons[0].Duplicate());
            attackModel.weapons[1].Rate = 5f;
            attackModel.weapons[1].projectile = attackModel.weapons[1].GetBehavior<AlternateProjectileModel>().projectile;
            return towerModel;
        }


        public static TowerModel getT5D(GameModel gameModel)
        {
            TowerModel towerModel = getT4D(gameModel).Duplicate<TowerModel>();
            towerModel.name = customTowerName + "-005";
            towerModel.tier = 5;
            towerModel.tiers = new int[] { 0, 0, 5 };
            towerModel.appliedUpgrades = new Il2CppStringArray(new string[]
            {
                    customTowerUpgrade1D,
                    customTowerUpgrade2D,
                    customTowerUpgrade3D,
                    customTowerUpgrade4D,
                    customTowerUpgrade5D
            });
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[0]);
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
                    customTowerUpgrade1U,
                    customTowerUpgrade2U,
                    customTowerUpgrade3U,
                    customTowerUpgrade4U,
                    customTowerUpgrade5U,
                    customTowerUpgrade1D,
                    customTowerUpgrade2D,
                    customTowerUpgrade3D,
                    customTowerUpgrade4D,
                    customTowerUpgrade5D,
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
