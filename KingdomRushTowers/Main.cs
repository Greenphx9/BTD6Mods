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
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Models.Towers.Upgrades;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity.Display.Animation;
using UnityEngine.Rendering;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using UnityEngine.UI;
using Assets.Scripts.Simulation.Towers.Projectiles;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Input;

[assembly: MelonInfo(typeof(KingdomRushTowers.Main), "Kingdom Rush Towers", "1.0.0", "Greenphx, Kingdom Rush Devs")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace KingdomRushTowers
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Kingdom Rush Towers Loaded!");
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton), nameof(StandardTowerPurchaseButton.UpdateIcon))]
        public class SetTower
        {
            [HarmonyPrefix]
            internal static bool Prefix(ref StandardTowerPurchaseButton __instance)
            {
                __instance.bg = __instance.gameObject.GetComponent<Image>();
                var towerModel = __instance.baseTowerModel;
                if (towerModel.baseId.Contains("KingdomRushTowers"))
                {
                    __instance.SetBackground(ModContent.GetTexture<Main>("TowerContainerKingdomRush"));
                }
                return true;
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(InGame.instance != null && InGame.instance.bridge != null)
            {
                foreach(var tower in InGame.instance.GetAllTowerToSim())
                {

                }
            }
        }
    }
}