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
using UnhollowerBaseLib;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using BTD_Mod_Helper;

namespace minicustomtowersv2
{
    public class SpikeBallShooterTower
    {
        public class SpikeBallShooter : ModTower
        {
            public override string BaseTower => "TackShooter";
            public override string Name => "SpikeBallShooter";
            public override int Cost => 260;
            public override string DisplayName => "Spike Ball Shooter";
            public override string Description => "Pops bloons using his small gun.";
            public override bool Use2DModel => true;
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 4;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 4;
            public override float PixelsPerUnit => 5f;
            public override string Icon => "SpikeBallShooter_Icon";
            public override string Portrait => "SpikeBallShooter_Icon";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.isGlobalRange = false;
                towerModel.range = 32.0f;
                towerModel.GetAttackModel().range = towerModel.range;
                towerModel.GetAttackModel().weapons[0].Rate = 3.7f;
                towerModel.GetAttackModel().weapons[0].emission.Cast<ArcEmissionModel>().count = 4;
                towerModel.GetAttackModel().weapons[0].projectile = Game.instance.model.GetTowerFromId("DartMonkey-300").GetAttackModel().weapons[0].projectile.Duplicate();
                towerModel.GetAttackModel().weapons[0].projectile.pierce = 2.0f;
                towerModel.GetAttackModel().weapons[0].projectile.radius *= 0.5f;
                towerModel.GetAttackModel().weapons[0].projectile.scale *= 0.5f;
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 0.5f;
            }
            public override string Get2DTexture(int[] tiers)
            {
                return "SpikeBallShooter";
            }
        }
        public class SharperSpikes : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "SharperSpikes";
            public override string DisplayName => "Sharper Spikes";
            public override string Description => "Spike balls can pop an 3 extra bloons.";
            public override int Cost => 200;
            public override int Path => TOP;
            public override int Tier => 1;
            public override string Icon => "SharperSpikes_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 3.0f;
            }
        }
        public class IronSpikeBalls : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "IronSpikeBalls";
            public override string DisplayName => "Iron Spike Balls";
            public override string Description => "Iron spike balls are more durable and can pop up to 10 bloons each.";
            public override int Cost => 400;
            public override int Path => TOP;
            public override int Tier => 2;
            public override string Icon => "IronSpikeBalls_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 5.0f;
            }
        }
        public class HyperthermicChemicals : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "HyperthermicChemicals";
            public override string DisplayName => "Hyperthermic Chemicals";
            public override string Description => "Spike balls are loaded with unstable chemicals, making them white hot and explosive.";
            public override int Cost => 1500;
            public override int Path => TOP;
            public override int Tier => 3;
            public override string Icon => "HyperthermicChemicals_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("CreateProjectileOnExhaustFractionModel_", Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate(), new SingleEmissionModel("SingleEmissionModel_", null), 1.0f, -1.0f, true));
                attackModel.weapons[0].projectile.AddBehavior(new CreateEffectOnExhaustFractionModel("CreateEffectOnExhaustFractionModel_", "6d84b13b7622d2744b8e8369565bc058", Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.Duplicate(), 2.0f, false, 1.0f, -1.0f, false));
                var sound = Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                attackModel.weapons[0].projectile.AddBehavior(new CreateSoundOnProjectileExhaustModel("CreateSoundOnProjectileExhaustModel_", sound.sound1, sound.sound2, sound.sound3, sound.sound4, sound.sound5));
            }
        }
        public class LoadedSteel : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "LoadedSteel";
            public override string DisplayName => "Loaded Steel";
            public override string Description => "Steel spike balls can pop up to 50 bloons each and explode much more violently, expelling frags.";
            public override int Cost => 3000;
            public override int Path => TOP;
            public override int Tier => 4;
            public override string Icon => "LoadedSteel_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach(var createProj in Game.instance.model.GetTowerFromId("BombShooter-002").GetAttackModel().weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>())
                {
                    if (createProj.projectile.id.Contains("Frag"))
                    {
                        attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnExhaustFractionModel("CreateProjectileOnExhaustFractionModel_", createProj.projectile.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 8, 0.0f, 360.0f, null, true), 1.0f, -1.0f, true));
                    }
                }
                towerModel.GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnExhaustFractionModel>().effectModel.scale *= 1.25f;
                foreach (var createProj in attackModel.weapons[0].projectile.GetBehaviors<CreateProjectileOnExhaustFractionModel>())
                {
                    if (!createProj.projectile.id.Contains("Frag"))
                    {
                        createProj.projectile.radius *= 1.25f;
                        createProj.projectile.scale *= 1.25f;
                    }
                }
                attackModel.weapons[0].projectile.pierce += 40.0f;
            }
        }
        public class LongRangeSpikeBalls : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "LongRangeSpikeBalls";
            public override string DisplayName => "Long Range Spike Balls";
            public override string Description => "Allows the Spike Ball Shooter to attack bloons from much further.";
            public override int Cost => 150;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override string Icon => "LongRangeSpikeBalls_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range += 10.0f;
                attackModel.range = towerModel.range;
            }
        }
        public class FasterShooters : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "FasterShooters";
            public override string DisplayName => "Faster Shooters";
            public override string Description => "Spike Ball Shooter attacks 50% faster.";
            public override int Cost => 400;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override string Icon => "FasterShooters_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate *= 0.5f;
            }
        }
        public class Octofire : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "Octofire";
            public override string DisplayName => "Octofire";
            public override string Description => "Spike Ball Shooter gains additional barrels, allowing it to fire much faster and hit more bloons.";
            public override int Cost => 1500;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override string Icon => "Octofire_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count = 8;
                attackModel.weapons[0].Rate *= 0.5f;
            }
        }
        public class SpikeyChaos : ModUpgrade<SpikeBallShooter>
        {
            public override string Name => "SpikeyChaos";
            public override string DisplayName => "Spikey Chaos";
            public override string Description => "Ability: Showers the screen in spike balls, doing massive damage to bloons.";
            public override int Cost => 19200;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override string Icon => "SpikeyChaos_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-040").GetAbility().Duplicate());
                towerModel.GetAbility().icon = GetSpriteReference(mod, "SpikeyChaos_Icon");
                var activate = towerModel.GetAbility().GetBehavior<ActivateAttackModel>();
                activate.attacks[0].weapons[0].RemoveBehavior<SpinModel>();
                activate.attacks[0].weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 8, 0.0f, 360.0f, null, true);
                activate.attacks[0].weapons[0].projectile = attackModel.weapons[0].projectile.Duplicate();
                activate.attacks[0].weapons[0].projectile.RemoveBehavior<TravelStraitModel>();
                activate.attacks[0].weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("TackShooter-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<TravelStraitModel>().Duplicate());
                activate.attacks[0].weapons[0].projectile.pierce += 50.0f;
            }
        }
    }

}