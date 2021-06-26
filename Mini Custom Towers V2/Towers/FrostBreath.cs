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

namespace minicustomtowersv2
{
    public class FrostBreathTower
    {
        public class FrostBreath : ModTower
        {
            public override string BaseTower => "WizardMonkey-030";
            public override string Name => "FrostBreath";
            public override int Cost => 2300;
            public override string DisplayName => "Frost Breath";
            public override string Description => "Shoots ice at bloons.";
            public override string TowerSet => "Magic";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
                towerModel.GetAttackModel(1).weapons[0].projectile = Game.instance.model.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
                towerModel.GetAttackModel(2).weapons[0].projectile.pierce = 0f;
                towerModel.GetAttackModel(2).weapons[0].Rate = 9999f;
                towerModel.GetAttackModel(2).weapons[0].projectile.display = "a";
                towerModel.GetAttackModel(3).weapons[0].projectile = Game.instance.model.GetTowerFromId("IceMonkey-003").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
                towerModel.GetAttackModel(3).weapons[0].Rate = 0.25f;
                towerModel.GetAttackModel(3).weapons[0].projectile.radius *= 1.5f;
                towerModel.AddBehavior<OverrideCamoDetectionModel>(new OverrideCamoDetectionModel("frostbreath", true));
                towerModel.GetAttackModel(3).weapons[0].projectile.SetHitCamo(true);
                towerModel.ApplyDisplay<FrostBreathDisplay>();
                towerModel.GetAttackModel(3).weapons[0].projectile.ApplyDisplay<FrostBreathBlue>();
            }
            public override string Icon => "FrostBreath_Icon";
            public override string Portrait => "FrostBreath_Icon";
        }
        public class FrostBreathDisplay : ModDisplay
        {
            public override string BaseDisplay => "ac12ead67e1228b49b74153614039736";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.genericRenderers[1].material.mainTexture = GetTexture(mod, "FrostBreathDisplay");
            }
        }
        public class FrostBreathBlue : ModDisplay
        {
            public override string BaseDisplay => "c73fd08146403e14fbcebd3cbf600b88";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "FrostBreathBlue");
            }
        }
        public class FasterBreathing : ModUpgrade<FrostBreath>
        {
            public override string Name => "FasterBreathing";
            public override string DisplayName => "Faster Breathing";
            public override string Description => "The frost breath monkey breaths frost out faster.";
            public override int Cost => 2600;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "FrostBreath1";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(3);
                attackModel.weapons[0].Rate = 0.15f;
            }
        }
        public class MetalFreeze : ModUpgrade<FrostBreath>
        {
            public override string Name => "MetalFreeze ";
            public override string DisplayName => "Metal Freeze ";
            public override string Description => "Frost can now pop leads.";
            public override int Cost => 1600;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "MetalFreeze_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(3);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.White;
            }
        }
        public class DeepFreeze : ModUpgrade<FrostBreath>
        {
            public override string Name => "DeepFreeze ";
            public override string DisplayName => "Deep Freeze ";
            public override string Description => "The frost breath monkey breaths frost out faster. Damage is increased.";
            public override int Cost => 3900;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "DeepFreeze_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(3);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1.0f;
                attackModel.weapons[0].Rate = 0.1f;
            }
        }
        public class IceColdBreath : ModUpgrade<FrostBreath>
        {
            public override string Name => "IceColdBreath";
            public override string DisplayName => "Ice Cold Breath";
            public override string Description => "Ice is so cold it can even pop white bloons. Damage is increased";
            public override int Cost => 5400;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "FrostBreath1";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(3);
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage += 1.0f;
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.ApplyDisplay<FrostBreathWhite>();
            }
        }
        public class FrostBreathWhite : ModDisplay
        {
            public override string BaseDisplay => "c73fd08146403e14fbcebd3cbf600b88";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "FrostBreathWhite");
            }
        }
        public class MOABFreeze : ModUpgrade<FrostBreath>
        {
            public override string Name => "MOABFreeze";
            public override string DisplayName => "MOAB Freeze";
            public override string Description => "Ice can freeze MOABs.";
            public override int Cost => 38000;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override string Icon => "MOABFreeze_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel(3);
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("IceMonkey-005").GetAttackModel().weapons[0].projectile.Duplicate<ProjectileModel>();
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.SetHitCamo(true);
                attackModel.weapons[0].projectile.ApplyDisplay<FrostBreathWhite>();
            }
        }
    }

}