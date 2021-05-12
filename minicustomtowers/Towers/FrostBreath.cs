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
    class FrostBreath
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
                    LocalizationManager.instance.textTable.Add(customTowerName, "Frost Breath");
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
                "+1 Dart damage.",
                "+1 Dart damage, again.",
                "+Fire-Rate",
                "Darts are turned into bombs. Bombs can hit blacks/zebras.",
                "Bombs now stun bloons."
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
                list.Add(new UpgradeModel(customTowerUpgrade1, 1450, 0, new SpriteReference(guid: "FrostBreath1"), 1, 1, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade2, 3450, 0, new SpriteReference(guid: "337c7c30ed5744543a6cfed237186735"), 1, 2, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade3, 5605, 0, new SpriteReference(guid: "77e70f89f900b944ea3f32e7feaae075"), 1, 3, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade4, 8750, 0, new SpriteReference(guid: "FrostBreath1"), 1, 4, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade5, 45020, 0, new SpriteReference(guid: "d6b7340621dee7d4ab2a48c0cbd8b529"), 1, 5, 0, "", ""));
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
            CacheBuilder.toBuild.PushAll("FrostBreath", "FrostBreath1", "FrostBreathBlue", "FrostBreathWhite");
            //Console.WriteLine("Bloonjitsu Initialized!");
            }
   

        static string customTowerImageID;
        static string customTowerIcon010;
        static string customTowerIcon020;
        static string customTowerIcon050;
        static string customTowerIcon040;
        static string customTowerImages = @"Mods/cobramonkey/";
        static string customTowerName = "Frost Breath";
        static string customTowerDisplay = "";
        static string customTowerUpgrade1 = "Faster Breathing";
        static string customTowerUpgrade2 = "Metal Freeze ";
        static string customTowerUpgrade3 = "Deep Freeze ";
        static string customTowerUpgrade4 = "Advanced Freezing";
        static string customTowerUpgrade5 = "MOAB Freeze";





        public static TowerModel getT0(GameModel gameModel)
        {
            TowerModel towerModel = gameModel.GetTowerFromId("WizardMonkey-032").Duplicate<TowerModel>(); //gameModel.GetTowerFromId(Alchemist).Duplicate<TowerModel>();
            towerModel.name = customTowerName;
            towerModel.baseId = customTowerName;
            towerModel.dontDisplayUpgrades = false;
            towerModel.cost = 2400;
            towerModel.isGlobalRange = false;
            towerModel.portrait = new SpriteReference(guid: "FrostBreath");
            towerModel.icon = new SpriteReference(guid: "FrostBreath");
            towerModel.instaIcon = new SpriteReference(guid: "FrostBreath");
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                        new UpgradePathModel(customTowerUpgrade1, customTowerName + "-010", 1, 1)
            });
            towerModel.tiers = new int[] { 0, 0, 0 };
            var attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].projectile = gameModel.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
            towerModel.GetAttackModel(1).weapons[0].projectile = gameModel.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
            towerModel.GetAttackModel(2).weapons[0].projectile.pierce = 0f;
            towerModel.GetAttackModel(2).weapons[0].Rate = 9999f;
            towerModel.GetAttackModel(2).weapons[0].projectile.display = "a";
            towerModel.GetAttackModel(3).weapons[0].projectile = gameModel.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
            towerModel.GetAttackModel(3).weapons[0].Rate = 0.25f;
            towerModel.GetAttackModel(3).weapons[0].projectile.display = "FrostBreathBlue";
            towerModel.GetAttackModel(3).weapons[0].projectile.radius *= 1.5f;
            towerModel.AddBehavior<OverrideCamoDetectionModel>(new OverrideCamoDetectionModel("frostbreath", true));
            towerModel.GetAttackModel(3).weapons[0].projectile.SetHitCamo(true);
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
            towerModel.GetAttackModel(3).weapons[0].Rate = 0.15f;   
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
            towerModel.GetAttackModel(3).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            towerModel.GetAttackModel(3).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 3f;
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
            towerModel.GetAttackModel(3).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 4f;
            towerModel.GetAttackModel(3).weapons[0].Rate = 0.1f;
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
            towerModel.GetAttackModel(3).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel(3).weapons[0].projectile.display = "FrostBreathWhite";
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
            towerModel.GetAttackModel(3).weapons[0].projectile = gameModel.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
            towerModel.GetAttackModel(3).weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.GetAttackModel(3).weapons[0].projectile.display = "FrostBreathWhite";
            towerModel.GetAttackModel(3).weapons[0].projectile.radius *= 1.5f;
            towerModel.GetAttackModel(3).weapons[0].projectile.SetHitCamo(true);
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


        [HarmonyPatch(typeof(StandardTowerPurchaseButton), "SetTower")]
        private class SetTower
        {

            [HarmonyPrefix]
            internal static bool Fix(ref StandardTowerPurchaseButton __instance, ref TowerModel towerModel, ref bool showTowerCount, ref bool hero, ref int buttonIndex)
            {
                if (towerModel.baseId.Contains(customTowerName))
                {
                    __instance.UpdateTowerDisplay();
                    Texture2D pngTexture = TextureFromPNG(@"Mods\bananafarmer\bananafarmer.png");
                    Sprite temp = Sprite.Create(pngTexture, new Rect(0.0f, 0.0f, pngTexture.width, pngTexture.height), default);
                    __instance.bg.sprite = temp;
                    __instance.icon.sprite = temp;
                    __instance.icon.overrideSprite = temp;
                    __instance.icon.material.mainTexture = temp.texture;
                    __instance.UpdateIcon();

                }
                return true;
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
