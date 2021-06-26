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
using Assets.Scripts.Models.Towers.Weapons.Behaviors;

namespace minicustomtowersv2
{
    public class HeroMasterTower
    {
        public class HeroMaster : ModTower
        {
            public override string BaseTower => "DartMonkey-002";
            public override string Name => "HeroMaster";
            public override int Cost => 1105;
            public override string DisplayName => "Hero Master";
            public override string Description => "Using the power of all the heroes, shoots out all of the hero projectiles.";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 1;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.ApplyDisplay<HeroMasterDisplay>();
                attackModel.weapons[0] = Game.instance.model.GetTowerFromId("Quincy").GetAttackModel().weapons[0].Duplicate();
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Gwendolin").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("StrikerJones").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("ObynGreenfoot").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("CaptainChurchill").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddBehavior(Game.instance.model.GetTowerFromId("CaptainChurchill").GetAttackModel().GetBehavior<DisplayModel>().Duplicate());
                attackModel.GetBehavior<DisplayModel>().scale = 0.0f;
                attackModel.GetBehavior<DisplayModel>().display = "a";
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Ezili").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("PatFusty").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Adora").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("AdmiralBrickell").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().weapons[0].Duplicate());
                attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Psi").GetAttackModel().weapons[0].Duplicate());
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Etienne").GetBehavior<DroneSupportModel>().Duplicate());
            }
            public override string Portrait => "HeroMaster_Icon";
            public override string Icon => "HeroMaster_Icon";
        }
        public class HeroMasterDisplay : ModDisplay
        {
            public override string BaseDisplay => "a7218c452777b624cbe92f09d4c3bf9f";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "HeroMasterDisplay");
            }
        }
        public class Level4 : ModUpgrade<HeroMaster>
        {
            public override string Name => "Level 4";
            public override string DisplayName => "Level 4";
            public override string Description => "Shoots out level 4 hero projectiles";
            public override int Cost => 2100;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetBehavior<DroneSupportModel>().droneModel = Game.instance.model.GetTowerFromId("Etienne 4").GetBehavior<DroneSupportModel>().droneModel.Duplicate();
                attackModel.weapons[0] = Game.instance.model.GetTowerFromId("Quincy 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[1] = Game.instance.model.GetTowerFromId("Gwendolin 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[2] = Game.instance.model.GetTowerFromId("StrikerJones 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[3] = Game.instance.model.GetTowerFromId("ObynGreenfoot 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[4] = Game.instance.model.GetTowerFromId("CaptainChurchill 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[5] = Game.instance.model.GetTowerFromId("Ezili 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[6] = Game.instance.model.GetTowerFromId("PatFusty 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[7] = Game.instance.model.GetTowerFromId("Adora 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[8] = Game.instance.model.GetTowerFromId("AdmiralBrickell 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[9] = Game.instance.model.GetTowerFromId("Sauda 4").GetAttackModel().weapons[0].Duplicate();
                attackModel.weapons[10] = Game.instance.model.GetTowerFromId("Psi 4").GetAttackModel().weapons[0].Duplicate();
            }
        }
        
    }

}