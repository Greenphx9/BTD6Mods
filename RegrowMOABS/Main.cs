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
using BTD_Mod_Helper;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Unity.UI_New.InGame.BloonMenu;

[assembly: MelonInfo(typeof(RegrowMOABS.Main), "Regrow MOABS", "1.0.0", "Greenphx")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace RegrowMOABS
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            MelonLogger.Msg("Regrow MOABS Mod Loaded!");
        }
        public static List<BloonModel> models = new List<BloonModel>();
        public override void OnGameModelLoaded(GameModel model)
        {

            base.OnGameModelLoaded(model);
            foreach(var bloon in model.bloons)
            {
                BloonModel bloon2 = null;
                if(bloon.id == ("CeramicRegrow"))
                {
                    bloon.GetBehavior<GrowModel>().growToId = "Moab";
                    bloon2 = bloon;
                }
                if (bloon.id == ("Moab"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "Bfb"));
                    bloon.isGrow = true;
                    Il2CppSystem.Collections.Generic.List<Assets.Scripts.Models.Bloons.BloonModel> list = new Il2CppSystem.Collections.Generic.List<Assets.Scripts.Models.Bloons.BloonModel>();
                    for (int i = 0; i < 5; i++)
                    {
                        list.Add(bloon2);
                    }
                    bloon.childBloonModels = list;
                    for(int i = 0; i < bloon.GetBehavior<SpawnChildrenModel>().children.Length; i++)
                    {
                        bloon.GetBehavior<SpawnChildrenModel>().children[i] = "CeramicRegrow";
                    }
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("CeramicRegrowFortified"))
                {
                    bloon.GetBehavior<GrowModel>().growToId = "MoabFortified";
                    bloon2 = bloon;
                }
                if (bloon.id == ("MoabFortified"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "BfbFortified"));
                    bloon.isGrow = true;
                    Il2CppSystem.Collections.Generic.List<Assets.Scripts.Models.Bloons.BloonModel> list = new Il2CppSystem.Collections.Generic.List<Assets.Scripts.Models.Bloons.BloonModel>();
                    for (int i = 0; i < 5; i++)
                    {
                        list.Add(bloon2);
                    }
                    bloon.childBloonModels = list;
                    for (int i = 0; i < bloon.GetBehavior<SpawnChildrenModel>().children.Length; i++)
                    {
                        bloon.GetBehavior<SpawnChildrenModel>().children[i] = "CeramicRegrowFortified";
                    }
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("Bfb"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "Zomg"));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("BfbFortified"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "ZomgFortified"));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("Zomg"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "Bad"));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("ZomgFortified"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, "BadFortified"));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("Bad"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, ""));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("BadFortified"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, ""));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("Ddt"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, ""));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("DdtFortified"))
                {
                    bloon.AddBehavior(new GrowModel("GrowModel_", 3.0f, ""));
                    bloon.isGrow = true;
                    bloon.tags.AddItem("Grow");
                    models.Add(bloon);
                }
                if (bloon.id == ("CeramicRegrowCamo"))
                {
                    bloon.GetBehavior<GrowModel>().growToId = "DdtCamo";
                }
                if (bloon.id == ("CeramicRegrowFortifiedCamo"))
                {
                    bloon.GetBehavior<GrowModel>().growToId = "DdtFortifiedCamo";
                }
            }
        }
        [HarmonyPatch(typeof(BloonMenu), nameof(BloonMenu.CreateBloonButtons))]
        public class CreateBloonButtons
        {
            [HarmonyPrefix]
            private static bool Prefix(ref BloonMenu __instance, ref Il2CppSystem.Collections.Generic.List<BloonModel> sortedBloons)
            {
                foreach(var bloon in models)
                {
                    sortedBloons.Add(bloon);
                }
                return true;
            }
        }
    }

}