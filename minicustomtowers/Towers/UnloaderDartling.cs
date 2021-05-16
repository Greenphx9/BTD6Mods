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
using minicustomtowers.Resources;

namespace minicustomtowers.Towers
{
    class UnloaderDartling
    {
       
                public static void Init()
        { 
               

                if (!LocalizationManager.instance.textTable.ContainsKey(customTowerName))
                {
                    LocalizationManager.instance.textTable.Add(customTowerName, "Unloader Dartling Gunner");
                }
            


       
                System.Collections.Generic.List<TowerModel> list2 = new System.Collections.Generic.List<TowerModel>();
                list2.Add(getT0(Game.instance.model));
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
            CacheBuilder.toBuild.PushAll("UnloaderDartling", "UnloaderDartlingGunner");
            }
   

        static string customTowerImageID;
        static string customTowerIcon010;
        static string customTowerIcon020;
        static string customTowerIcon050;
        static string customTowerIcon040;
        static string customTowerImages = @"Mods/cobramonkey/";
        static string customTowerName = "Unloader Dartling Gunner";
        static string customTowerDisplay = "";
        //static string customTowerUpgrade1 = "Bloon Distraction";
        //static string customTowerUpgrade2 = "Sharper Shurikens";
        //static string customTowerUpgrade3 = "More Shurikens";
        //static string customTowerUpgrade4 = "Flamin' Hot Shurikens";
        //static string customTowerUpgrade5 = "Shuriken Mastery";





        public static TowerModel getT0(GameModel gameModel)
        {
            TowerModel towerModel = gameModel.GetTowerFromId("DartlingGunner").Duplicate<TowerModel>(); //gameModel.GetTowerFromId(Alchemist).Duplicate<TowerModel>();
            towerModel.name = customTowerName;
            towerModel.baseId = customTowerName;
            towerModel.portrait = new SpriteReference(guid: "UnloaderDartling");
            towerModel.icon = new SpriteReference(guid: "UnloaderDartling");
            towerModel.instaIcon = new SpriteReference(guid: "UnloaderDartling");
            towerModel.dontDisplayUpgrades = true;
            towerModel.cost = 5700f;
            towerModel.isGlobalRange = false;
            towerModel.display = "UnloaderDartlingGunner";
            towerModel.GetBehavior<DisplayModel>().display = "UnloaderDartlingGunner";
            towerModel.tiers = new int[] { 0, 0, 0 };
            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].Rate = 2.0f;
            /*attackModel.weapons[0].projectile.pierce = 5f;
            attackModel.weapons[0].emission = new ArcEmissionModel("unloader", 2, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[1].projectile.GetBehavior<TravelStraitModel>().Speed = 325f;
            attackModel.weapons[1].emission = new ArcEmissionModel("unloader", 3, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[2].projectile.GetBehavior<TravelStraitModel>().Speed = 300f;
            attackModel.weapons[2].emission = new ArcEmissionModel("unloader", 4, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[3].projectile.GetBehavior<TravelStraitModel>().Speed = 275f;
            attackModel.weapons[3].emission = new ArcEmissionModel("unloader", 5, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[4].projectile.GetBehavior<TravelStraitModel>().Speed = 250f;
            attackModel.weapons[4].emission = new ArcEmissionModel("unloader", 6, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[5].projectile.GetBehavior<TravelStraitModel>().Speed = 225f;
            attackModel.weapons[5].emission = new ArcEmissionModel("unloader", 7, 0.0f, 15.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate<WeaponModel>());
            attackModel.weapons[6].projectile.GetBehavior<TravelStraitModel>().Speed = 200f;
            attackModel.weapons[6].emission = new ArcEmissionModel("unloader", 8, 0.0f, 15.0f, null, false, false);*/
            attackModel.weapons[0].projectile.pierce = 5f;
            attackModel.weapons[0].emission = new ArcEmissionModel("unloader", 2, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
            attackModel.weapons[1].startInCooldown = true;
            attackModel.weapons[1].customStartCooldown = 0.075f;
            attackModel.weapons[1].emission = new ArcEmissionModel("unloader", 3, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[2].customStartCooldown = 0.150f;
            attackModel.weapons[2].emission = new ArcEmissionModel("unloader", 4, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[3].customStartCooldown = 0.225f;
            attackModel.weapons[3].emission = new ArcEmissionModel("unloader", 5, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[4].customStartCooldown = 0.300f;
            attackModel.weapons[4].emission = new ArcEmissionModel("unloader", 6, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[5].customStartCooldown = 0.375f;
            attackModel.weapons[5].emission = new ArcEmissionModel("unloader", 7, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[6].customStartCooldown = 0.450f;
            attackModel.weapons[6].emission = new ArcEmissionModel("unloader", 8, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[7].customStartCooldown = 0.525f;
            attackModel.weapons[7].emission = new ArcEmissionModel("unloader", 9, 0.0f, 25.0f, null, false, false);
            attackModel.AddWeapon(attackModel.weapons[1].Duplicate());
            attackModel.weapons[8].customStartCooldown = 0.600f;
            attackModel.weapons[8].emission = new ArcEmissionModel("unloader", 10, 0.0f, 25.0f, null, false, false);
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

             
            }
        }


        /*[HarmonyPatch(typeof(StandardTowerPurchaseButton), "SetTower")]
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
        }*/



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
