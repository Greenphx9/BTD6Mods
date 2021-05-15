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
    class BloonDrain
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
                    LocalizationManager.instance.textTable.Add(customTowerName, "Bloontonium Darts");
                }
                string[] array = new string[]
                {
                customTowerUpgrade1,
                customTowerUpgrade2,
                //customTowerUpgrade3,
                //customTowerUpgrade4,
                //customTowerUpgrade5
                };
                string[] array2 = new string[]
                {
                "Eight Plasma Whips",
                "Completely suck a MOAB-class bloon dry",
                //"+Fire-Rate",
                //"Darts are turned into bombs. Bombs can hit blacks/zebras.",
                //"Bombs now stun bloons."
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
                list.Add(new UpgradeModel(customTowerUpgrade1, 3500, 0, new SpriteReference(guid: "BloonDrain1"), 1, 1, 0, "", ""));
                list.Add(new UpgradeModel(customTowerUpgrade2, 5500, 0, new SpriteReference(guid: "BloonDrain2"), 1, 2, 0, "", ""));
                Game.instance.model.upgrades = Game.instance.model.upgrades.Add(list);
                System.Collections.Generic.List<TowerModel> list2 = new System.Collections.Generic.List<TowerModel>();
                list2.Add(getT0(Game.instance.model));
                list2.Add(getT1(Game.instance.model));
                list2.Add(getT2(Game.instance.model));
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
            CacheBuilder.toBuild.PushAll("BloonDrain1", "BloonDrain2");
            //Console.WriteLine("Bloonjitsu Initialized!");
            }
   

        static string customTowerImageID;
        static string customTowerIcon010;
        static string customTowerIcon020;
        static string customTowerIcon050;
        static string customTowerIcon040;
        static string customTowerImages = @"Mods/cobramonkey/";
        static string customTowerName = "Bloon Drain";
        static string customTowerDisplay = "";
        static string customTowerUpgrade1 = "Octo-Plasma";
        static string customTowerUpgrade2 = "MOAB Drain";





        public static TowerModel getT0(GameModel gameModel)
        {
            TowerModel towerModel = gameModel.GetTowerFromId("SuperMonkey-200").Duplicate<TowerModel>(); //gameModel.GetTowerFromId(Alchemist).Duplicate<TowerModel>();
            towerModel.name = customTowerName;
            towerModel.baseId = customTowerName;
            towerModel.dontDisplayUpgrades = false;
            towerModel.cost = 5010;
            towerModel.isGlobalRange = false;
            towerModel.portrait = new SpriteReference(guid: "BloontoniumDartsIcon");
            towerModel.icon = new SpriteReference(guid: "BloontoniumDartsPortrait");
            towerModel.instaIcon = new SpriteReference(guid: "BloontoniumDartsPortrait");
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[]
            {
                        new UpgradePathModel(customTowerUpgrade1, customTowerName + "-010", 1, 1)
            });
            towerModel.tiers = new int[] { 0, 0, 0 };
            var attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].emission = new ArcEmissionModel("plasmamonkey", 4, 0.0f, 15.0f, null, false, false);
            attackModel.weapons[0].projectile.GetDamageModel().damage = 0.25f;
            attackModel.weapons[0].projectile.pierce = 1f;
            var seeking = gameModel.GetTowerFromId("MonkeySub").GetAttackModel().weapons[0].projectile.GetBehavior<TrackTargetModel>().Duplicate();
            attackModel.weapons[0].projectile.AddBehavior(seeking);
            attackModel.weapons[0].projectile.display = "Plasma0";
            var cash = gameModel.GetTowerFromId("BananaFarm-003").GetAttackModel().weapons[0].projectile.GetBehavior<CashModel>().Duplicate();
            var bananaFarmText = gameModel.GetTowerFromId("BananaFarm-003").GetWeapon().projectile.GetBehavior<CreateTextEffectModel>().Duplicate<CreateTextEffectModel>();
            cash.minimum = 1f;
            cash.maximum = 1f;
            cash.salvage = 0.10f;
            bananaFarmText.assetId = "a";
            bananaFarmText.lifespan = 0f;
            attackModel.weapons[0].projectile.AddBehavior(cash);
            attackModel.weapons[0].projectile.AddBehavior(bananaFarmText);
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
            towerModel.portrait = new SpriteReference(guid: "BloontoniumDarts1");
            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
            attackModel.weapons[0].emission = new ArcEmissionModel("plasmamonkey", 8, 0.0f, 15.0f, null, false, false);
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
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(new UpgradePathModel[0]);
            towerModel.portrait = new SpriteReference(guid: "BloontoniumDarts2");
            //balance stuff
            AttackModel attackModel = towerModel.GetBehavior<AttackModel>();
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
