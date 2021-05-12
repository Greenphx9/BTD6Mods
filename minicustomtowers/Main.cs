using MelonLoader;
using Harmony;

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
using minicustomtowers.Resources;
using Assets.Scripts.Models.Rounds;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Models.TowerSets;

namespace minicustomtowers
{
    public class Main : MelonMod
    {
        
        [HarmonyPatch(typeof(TitleScreen), "Start")]
        public class Awake_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                minicustomtowers.Towers.Bloonjitsu.Init();
                
                MelonLogger.Msg("Bloonjitsu Loaded");
                minicustomtowers.Towers.SunTerror.Init();
                MelonLogger.Msg("Sun Terror Loaded");
                minicustomtowers.Towers.BionicMOARGlaives.Init();
                MelonLogger.Msg("Bionic MOAR Glaives Loaded");
                minicustomtowers.Towers.Bombjitsu.Init();
                MelonLogger.Msg("Bombjitsu Loaded");
                minicustomtowers.Towers.OperationNevaMiss.Init();
                MelonLogger.Msg("Operation: Neva-Miss Loaded");
                minicustomtowers.Towers.AceGunner.Init();
                MelonLogger.Msg("Ace Gunner Loaded");
                minicustomtowers.Towers.TripleJuggernaut.Init();
                MelonLogger.Msg("Triple Juggernaut Loaded");
                minicustomtowers.Towers.CannonDestroyer.Init();
                MelonLogger.Msg("Cannon Destroyer Loaded");
                minicustomtowers.Towers.BladeSprayer.Init();
                MelonLogger.Msg("Blade Sprayer Loaded");
                minicustomtowers.Towers.RetroBananaFarm.Init();
                MelonLogger.Msg("Retro Banana Farm Loaded");
                minicustomtowers.Towers.UnloaderDartling.Init();
                MelonLogger.Msg("Unloader Dartling Gunner Loaded");
                minicustomtowers.Towers.BloontoniumDarts.Init();
                MelonLogger.Msg("Bloontonium Darts Loaded");
                minicustomtowers.Towers.FrostBreath.Init();
                MelonLogger.Msg("Frost Breath Loaded");
                CacheBuilder.Build();
                MelonLogger.Msg("Cache Built");
            }
        }

        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            CacheBuilder.Build();
            //MelonLogger.Msg("Cache Built");
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            CacheBuilder.Flush();
            MelonLogger.Msg("Cache Flushed");
        }
        
        public override void OnUpdate()
        {
            base.OnUpdate();

            bool inAGame = InGame.instance != null && InGame.instance.bridge != null;
            if (inAGame)
            {
               
            }
        }







    }

}