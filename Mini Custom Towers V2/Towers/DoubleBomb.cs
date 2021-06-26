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
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;

namespace minicustomtowersv2
{
    public class DoubleBombTower
    {
        public class DoubleBomb : ModTower
        {
            public override string BaseTower => "NinjaMonkey-302";
            public override string Name => "DoubleBomb";
            public override int Cost => 3800;
            public override string DisplayName => "Double Bomb";
            public override string Description => "Throws 2 shurikens at bloons. Every once in a while, throws 2 bombs.";
            public override string TowerSet => "Magic";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 1;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-003").GetAttackModel().weapons[0].GetBehavior<AlternateProjectileModel>().Duplicate());
                towerModel.ApplyDisplay<DoubleBombDisplay>();
                attackModel.weapons[0].GetBehavior<AlternateProjectileModel>().emissionModel = attackModel.weapons[0].emission;
            }
            public override string Icon => "DoubleBomb_Icon";
            public override string Portrait => "DoubleBomb_Icon";
        }
        public class DoubleBombDisplay : ModDisplay
        {
            public override string BaseDisplay => "c1d0d7ac37da52146b4b3b7a0b267ba4";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach(Renderer renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture(mod, "DoubleBombDisplay");
                }
            }
        }
        public class Bombjitsu : ModUpgrade<DoubleBomb>
        {
            public override string Name => "Bombjitsu";
            public override string DisplayName => "Bombjitsu";
            public override string Description => "Shoots 5 shurikens out.";
            public override int Cost => 4400;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "Bombjitsu_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0.0f, 72.0f, null, false, false);
                attackModel.weapons[0].GetBehavior<AlternateProjectileModel>().emissionModel = attackModel.weapons[0].emission;
            }
        }
    }

}