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
                CacheBuilder.Build();
                MelonLogger.Msg("Cache Built");
            }
        }

        [HarmonyPatch(typeof(Tower), nameof(Tower.Initialise))]
        public class Tower_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                //for some reason some textures have all other upgrade textures so gonna load those like this
                foreach (TowerToSimulation towerToSimulation in InGame.instance.bridge.GetAllTowers())
                {
                    if (towerToSimulation.Def.baseId == "Bombjitsu")
                    {
                        try
                        {
                            Texture2D tex;
                            tex = CacheBuilder.Get("Bombjitsu");
                            //Console.WriteLine("loading custom texture for " + towername);
                            //ImageConversion.LoadImage(CacheBuilder.Get("Bombjitsu"), null);
                            //Console.WriteLine("adding to dict");
                            //Console.WriteLine("found custom texture for " + towername);
                            foreach (Renderer renderer in towerToSimulation.tower.Node.graphic.genericRenderers)
                            {
                                //Texture2D tex = new Texture2D(2, 2);
                                //ImageConversion.LoadImage(tex, File.ReadAllBytes(filePath + "custom/" + towerlocation));
                                //Console.WriteLine("loaded custom texture for " + towername);

                                renderer.material.mainTexture = tex;
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
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