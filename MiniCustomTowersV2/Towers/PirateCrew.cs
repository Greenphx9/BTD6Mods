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
    public class PirateCrewTower
    {
        public class PirateCrew : ModTower
        {
            public override string BaseTower => "MonkeySub";
            public override string Name => "PirateCrew";
            public override int Cost => 200;
            public override string DisplayName => "Pirate Crew";
            public override string Description => "The best selection of Captain Cassie's comrades with powerful pistols and pirate skills.";
            public override bool Use2DModel => true;
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 4;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 4;
            public override float PixelsPerUnit => 20f;
            public override string Icon => "PirateCrew_Icon";
            public override string Portrait => "PirateCrew_Icon";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.radius = 4.0f;
                towerModel.range = 40f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range = towerModel.range;
                attackModel.weapons[0].Rate = 1.694915254f;
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("DartlingGunner-003").GetAttackModel().weapons[0].projectile.Duplicate();
                attackModel.weapons[0].projectile.ApplyDisplay<BulletDisplay>();
                attackModel.weapons[0].projectile.scale *= 2f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed = 400f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 1.5f;
                attackModel.weapons[0].projectile.pierce = 1f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 2f;
                towerModel.RemoveBehavior<CreateSoundOnTowerPlaceModel>();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetBehavior<CreateSoundOnTowerPlaceModel>().Duplicate());
            }
            public override string Get2DTexture(int[] tiers)
            {
                return "PirateCrewDisplay";
            }
        }
        public class BulletDisplay : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "PirateCrewBullet");
            }
        }
        public class ImprovedMuzzle : ModUpgrade<PirateCrew>
        {
            public override string Name => "ImprovedMuzzle";
            public override string DisplayName => "Improved Muzzle";
            public override string Description => "Crew monkey reloads quicker.";
            public override int Cost => 200;
            public override int Path => TOP;
            public override int Tier => 1;
            public override string Icon => "ImprovedMuzzle_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate -= 0.5084745762f;
            }
        }
        public class LongerBarrel : ModUpgrade<PirateCrew>
        {
            public override string Name => "LongerBarrel";
            public override string DisplayName => "Longer Barrel";
            public override string Description => "Revolver can now shoot further.";
            public override int Cost => 120;
            public override int Path => TOP;
            public override int Tier => 2;
            public override string Icon => "LongerBarrel_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range += 15f;
                attackModel.range = towerModel.range;
            }
        }
        public class Merchantman : ModUpgrade<PirateCrew>
        {
            public override string Name => "Merchantman ";
            public override string DisplayName => "Merchantman ";
            public override string Description => "Switch into a small boat and give extra cash at the end of each round.";
            public override int Cost => 2000;
            public override int Path => TOP;
            public override int Tier => 3;
            public override string Icon => "Merchantman_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(new PerRoundCashBonusTowerModel("merchantmen_pirate_crew", 200.0f, 0.0f, 1.0f, "80178409df24b3b479342ed73cffb63d", false));
            }
        }
        public class FavoredTrades : ModUpgrade<PirateCrew>
        {
            public override string Name => "FavoredTrades ";
            public override string DisplayName => "Favored Trades ";
            public override string Description => "Gives increased cash, double attack speed and nearby monkeys are 5% cheaper.";
            public override int Cost => 5500;
            public override int Path => TOP;
            public override int Tier => 4;
            public override string Icon => "FavoredTrades_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 500f;
                attackModel.weapons[0].Rate -= 0.5932203389f;
                towerModel.AddBehavior(new DiscountZoneModel("favoredtrades", 0.05f, 1, "DiscountZoneMajorFavoredTrades", "Village", false, 5, "MonkeyBusinessBuff", "BuffIconVillagexx1"));
            }
        }
        public class PiercingBullets : ModUpgrade<PirateCrew>
        {
            public override string Name => "PiercingBullets";
            public override string DisplayName => "Piercing Bullets";
            public override string Description => "Bullets now pierce up to 4 bloons each.";
            public override int Cost => 500;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override string Icon => "PiercingBullets_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce = 4f;
            }
        }
        public class Cutlass : ModUpgrade<PirateCrew>
        {
            public override string Name => "Cutlass";
            public override string DisplayName => "Cutlass";
            public override string Description => "Adds an independent close range, bloon-slashing cutlass attack.";
            public override int Cost => 400;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override string Icon => "Cutlass_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Sauda").GetAttackModel().Duplicate());
                towerModel.GetAttackModel(1).range = attackModel.range * 0.75f;
                towerModel.GetAttackModel(1).weapons[0].Rate = 1.1f;
                towerModel.GetAttackModel(1).weapons[0].projectile.pierce = 8f;
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", false));
            }
        }
        public class DeckAssistant : ModUpgrade<PirateCrew>
        {
            public override string Name => "DeckAssistant";
            public override string DisplayName => "Deck Assistant";
            public override string Description => "Reduces ability cooldown by 5%. Crew weapons can pop Lead and gain extra pierce and damage.";
            public override int Cost => 2500;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override string Icon => "DeckAssistant_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(new AbilityCooldownScaleSupportModel("abilitycooldown", true, 1.05f, false, true, null, "EnergizerBuff", "BuffIconSub5xx", true));
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 3f;
                attackModel.weapons[0].projectile.pierce = 5f;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.GetAttackModel(1).weapons[0].projectile.pierce += 2f;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 1f;
            }
        }
        public class SharpeyeAssault : ModUpgrade<PirateCrew>
        {
            public override string Name => "SharpeyeAssault";
            public override string DisplayName => "Sharpeye Assault";
            public override string Description => "Increased gun and sword damage. Ability: all weapons do +15 damage for 13 seconds";
            public override int Cost => 6500;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override string Icon => "SharpeyeAssault_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().damage += 2f;
                towerModel.GetAttackModel(1).weapons[0].projectile.GetDamageModel().damage += 2f;
                towerModel.AddBehavior<AbilityModel>(new AbilityModel("SharpeyeAssault", "Sharpeye Assault", "All weapons do +15 damage for 13 seconds", 0, 0.0f, GetSpriteReference(mod, "SharpeyeAssault_Icon"), 40.0f, null, false, false, "SharpeyeAssault", 1.0f, 0, 9999, false, false));
                towerModel.GetBehavior<AbilityModel>().AddBehavior<DamageUpModel>(new DamageUpModel("SharpeyeAssaultDamage", 900, 15, new Assets.Scripts.Models.Effects.AssetPathModel("buckshotproj", attackModel.weapons[0].projectile.display)));

            }
        }
    }

}