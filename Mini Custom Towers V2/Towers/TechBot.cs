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
using Assets.Scripts.Models.Towers.Pets;
using Assets.Scripts.Unity.Bridge;
using System.Windows;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Powers;
using BTD_Mod_Helper.Api;

namespace minicustomtowersv2
{
    public class TechBotTower
    {
        public class TechBot : ModTower
        {
            public override string BaseTower => "DartMonkey";
            public override string Name => "Techbot ";
            public override int Cost => 200;
            public override string DisplayName => "Techbot";
            public override string Description => "Automatically activates abilites for you.";
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.behaviors = new UnhollowerBaseLib.Il2CppReferenceArray<Model>(0);
                foreach (Model model in Game.instance.model.GetTowerFromId("TechBot").behaviors.Duplicate())
                {
                    towerModel.AddBehavior(model);
                }
                var techbot = Game.instance.model.GetTowerFromId("TechBot");
                towerModel.display = techbot.display;
                towerModel.icon = techbot.portrait;
                towerModel.portrait = techbot.portrait;
                towerModel.instaIcon = techbot.portrait;
                towerModel.footprint = techbot.footprint;
                towerModel.ignoreBlockers = techbot.ignoreBlockers;
                towerModel.ignoreCoopAreas = techbot.ignoreCoopAreas;
                towerModel.ignoreTowerForSelection = techbot.ignoreTowerForSelection;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetBehavior<CreateSoundOnUpgradeModel>().Duplicate());
            }
        }
        public class Lasers : ModUpgrade<TechBot>
        {
            public override string Name => "Lasers";
            public override string DisplayName => "Lasers";
            public override string Description => "Shoots deadly lasers at bloons.";
            public override int Cost => 900;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 1.0f;
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("DartlingGunner-300").GetAttackModel().weapons[0].projectile.Duplicate();
                attackModel.range = 1000f;
            }
            public override string Icon => "Lasers_Icon";
        }
        public class TripleLasers : ModUpgrade<TechBot>
        {
            public override string Name => "TripleLasers";
            public override string DisplayName => "Triple Lasers";
            public override string Description => "Shoots 3 lasers at once!";
            public override int Cost => 2900;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0.0f, 30.0f, null, false, false);
            }
            public override string Icon => "TripleLasers_Icon";
        }

    }

}