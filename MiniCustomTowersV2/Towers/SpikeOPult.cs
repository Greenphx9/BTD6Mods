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
    public class SpikeOPultTower
    {
        public class SpikeOPult : ModTower
        {
            public override string BaseTower => "DartMonkey-300";
            public override string Name => "SpikeOPult";
            public override int Cost => 500;
            public override string DisplayName => "Spike-O-Pult";
            public override string Description => "Hurls a spiked ball that pops everything that it touches.";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 4;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 4;
            public override float PixelsPerUnit => 5f;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.scale *= 0.75f;
                attackModel.weapons[0].projectile.radius *= 0.75f;
                towerModel.range = 40f;
                attackModel.range = towerModel.range;
            }
            public override string Icon => "SpikeOPult_Icon";
            public override string Portrait => "SpikeOPult_Icon";
        }
        public class GreasedRunners : ModUpgrade<SpikeOPult>
        {
            public override string Name => "GreasedRunners";
            public override string DisplayName => "Greased Runners";
            public override string Description => "Lubricants added to the spike-o-pult allow it to shoot faster.";
            public override int Cost => 300;
            public override int Path => TOP;
            public override int Tier => 1;
            public override string Icon => "GreasedRunners_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate -= 0.4f;
            }
        }
        public class GreaterVision : ModUpgrade<SpikeOPult>
        {
            public override string Name => "GreaterVision";
            public override string DisplayName => "Greater Vision";
            public override string Description => "Allows the spike-o-pult to shoot farther and pop Camo bloons.";
            public override int Cost => 120;
            public override int Path => TOP;
            public override int Tier => 2;
            public override string Icon => "GreaterVision_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range += 10f;
                attackModel.range = towerModel.range;
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            }
        }
        public class MultiShot : ModUpgrade<SpikeOPult>
        {
            public override string Name => "MultiShot";
            public override string DisplayName => "Multi-Shot";
            public override string Description => "Shoots 3 spikey balls at once!";
            public override int Cost => 1150;
            public override int Path => TOP;
            public override int Tier => 3;
            public override string Icon => "MultiShot_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0.0f, 20.0f, null, false);
            }
        }
        public class FragSpikes : ModUpgrade<SpikeOPult>
        {
            public override string Name => "FragSpikes";
            public override string DisplayName => "Frag Spikes";
            public override string Description => "Spikey balls now have extra spikes that detach as they roll.";
            public override int Cost => 10000;
            public override int Path => TOP;
            public override int Tier => 4;
            public override string Icon => "FragSpikes_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("FragSpikes", Game.instance.model.GetTowerFromId("SpikeFactory").GetWeapon().projectile.Duplicate(), new SingleEmissionModel("FragSpikes_Emission", null), true, false, true));
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.pierce = 3f;
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("FragSpikes", Game.instance.model.GetTowerFromId("SpikeFactory").GetWeapon().projectile.Duplicate(), Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().emission.Duplicate(), 1.0f, -1.0f, false));
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.RemoveBehavior<ArriveAtTargetModel>();

            }
        }
        public class TightenedSprings : ModUpgrade<SpikeOPult>
        {
            public override string Name => "TightenedSprings";
            public override string DisplayName => "Tightened Springs";
            public override string Description => "Faster recoil allows the spiked balls to pop more bloons and roll faster.";
            public override int Cost => 275;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override string Icon => "TightenedSprings_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 9f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed += 40f;

            }
        }
        public class ReinforcedRatchets : ModUpgrade<SpikeOPult>
        {
            public override string Name => "ReinforcedRatchets";
            public override string DisplayName => "Reinforced Ratchets";
            public override string Description => "Spiked balls roll much farther, pop frozen bloons, and deal extra damage to Ceramic bloons.";
            public override int Cost => 375;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override string Icon => "ReinforcedRatchets_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
                attackModel.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_", "Ceramic", 1.0f, 5.0f, false, true));
            }
        }
        public class Juggernaut : ModUpgrade<SpikeOPult>
        {
            public override string Name => "Juggernaut ";
            public override string DisplayName => "Juggernaut ";
            public override string Description => "Hurls a giant unstoppable killer spiked ball that can pop lead bloons and excels at crushing Ceramic bloons.";
            public override int Cost => 1000;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override string Icon => "Juggernaut_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("DartMonkey-400").GetWeapon().projectile.Duplicate();
                attackModel.weapons[0].projectile.GetBehavior<DamageModifierForTagModel>().damageAddative = 6f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed += 40f;
            }
        }
        public class Juggerlanche : ModUpgrade<SpikeOPult>
        {
            public override string Name => "Juggerlanche";
            public override string DisplayName => "Juggerlanche";
            public override string Description => "Increases attack speed and damage greatly. Ability: Hurls juggernauts rapidly around the monkey.";
            public override int Cost => 6000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override string Icon => "Juggerlanche_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate -= 0.4f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 4f;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-040").GetBehavior<AbilityModel>().Duplicate());
                towerModel.GetBehavior<AbilityModel>().icon = GetSpriteReference(mod, "Juggerlanche_Icon");
                towerModel.GetAbility().name = "Juggerlanche";
                var activateAttack = towerModel.GetBehavior<AbilityModel>().GetBehavior<ActivateAttackModel>();
                activateAttack.attacks[0].weapons[0].projectile = towerModel.GetWeapon().projectile.Duplicate();
            }
        }
    }

}