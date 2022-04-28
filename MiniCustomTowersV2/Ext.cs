using MelonLoader;
using HarmonyLib;

using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Main.Scenes;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Unity.Bridge;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity.Map;
using UnityEngine;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Utils;
using System;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using Assets.Scripts.Simulation.Towers.Projectiles;
using System.IO;
using BTD_Mod_Helper.Api.Towers;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using UnityEngine.UI;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using System.Collections.Generic;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using System.Reflection;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Assets.Scripts.Unity.UI_New.InGame.RightMenu;


namespace minicustomtowersv2
{
    public static class Ext
    {
        public static void SetBG(this StandardTowerPurchaseButton standardTowerPurchaseButton, Sprite sprite)
        {
            if (standardTowerPurchaseButton.bg.name != "TowerCustomBG")
            {
                standardTowerPurchaseButton.SetBackground(sprite);
                standardTowerPurchaseButton.bg.name = "TowerCustomBG";
            }
        }
    }

}