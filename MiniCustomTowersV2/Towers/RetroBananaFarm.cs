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

namespace minicustomtowersv2
{
    public class RetroBananaFarmTower
    {
        public class RetroBananaFarm : ModTower
        {
            public override string BaseTower => "BananaFarm";
            public override string Name => "RetroBananaFarm";
            public override int Cost => 900;
            public override string DisplayName => "Retro Banana Farm";
            public override string Description => "Generates $80 after each round.";
            public override bool Use2DModel => true;
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 4;
            public override int BottomPathUpgrades => 0;
            public override float PixelsPerUnit => 5f;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.RemoveBehavior<AttackModel>();
                towerModel.AddBehavior(new PerRoundCashBonusTowerModel("retrobanana000", 80f, 0.0f, 1.0f, "80178409df24b3b479342ed73cffb63d", false));
            }
            public override string Icon => "RetroBananaFarm";
            public override string Portrait => "RetroBananaFarm";
            public override string Get2DTexture(int[] tiers)
            {
                if(tiers[1] == 1)
                {
                    return "MoreBananas_Display";
                }
                if (tiers[1] == 2)
                {
                    return "BananaPlantation_Display";
                }
                if (tiers[1] == 3)
                {
                    return "BananaRepublic_Display";
                }
                if (tiers[1] == 4)
                {
                    return "BananaResearchFacility_Display";
                }
                return "RetroBananaFarmDisplay";
            }
        }
        
        public class MoreBananas : ModUpgrade<RetroBananaFarm>
        {
            public override string Name => "MoreBananas ";
            public override string DisplayName => "More Bananas ";
            public override int Cost => 800;
            public override string Description => "Generates $120 per round";
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "MoreBananas_Icon";
            public override string Portrait => "MoreBananas_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 120f;
            }
        }
        
        public class BananaPlantation : ModUpgrade<RetroBananaFarm>
        {
            public override string Name => "BananaPlantation ";
            public override string DisplayName => "Banana Plantation ";
            public override int Cost => 1600;
            public override string Description => "Generates $250 per round";
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "BananaPlantation_Icon";
            public override string Portrait => "BananaPlantation_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 250f;
            }
        }
    
        public class BananaRepublic : ModUpgrade<RetroBananaFarm>
        {
            public override string Name => "BananaRepublic";
            public override string DisplayName => "Banana Republic";
            public override int Cost => 3000;
            public override string Description => "Generates $500 per round";
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "BananaRepublic_Icon";
            public override string Portrait => "BananaRepublic_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 500f;
            }
        }
        
        public class BananaResearchFacility : ModUpgrade<RetroBananaFarm>
        {
            public override string Name => "BananaResearchFacility ";
            public override string DisplayName => "Banana Research Facility ";
            public override int Cost => 14000;
            public override string Description => "Generates $2000 per round";
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "BananaResearchFacility_Icon";
            public override string Portrait => "BananaResearchFacility_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 2000f;
            }
        }
    }

}