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
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using BTD_Mod_Helper.Api.ModOptions;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Models.Towers.Upgrades;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity.Display.Animation;
using UnityEngine.Rendering;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Simulation.Objects;

namespace KingdomRushTowers
{
    public static class DwarvenBombard_
    {
        public class DwarvenBombard : ModTower
        {
            public override string BaseTower => "BombShooter";
            public override string Name => "DwarvenBombard";
            public override int Cost => 1250;
            public override string DisplayName => "Dwarven Bombard";
            public override string Description => "Attacks bloons by launching bombs at them. \"Fully loaded!\"";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 5;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.radius = 11.0f;
                towerModel.doesntRotate = true;
                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.range *= 1.2f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.2f;
                attackModel.weapons[0].Rate = 3.0f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 0.5f;
                var proj = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                proj.pierce = 9999999.0f;
                proj.maxPierce = 9999999.0f;
                proj.GetDamageModel().damage = 11.0f;
            }
            public override string Icon => "DwarvenBombard_Icon";
            public override string Portrait => "DwarvenBombard_Icon";
            public override bool Use2DModel => true;
            public override string Get2DTexture(int[] tiers)
            {
                if (tiers[2] == 3 || tiers[2] == 4 || tiers[2] == 5)
                {
                    return "Teslax104Display";
                }
                if (tiers[0] == 3 || tiers[0] == 4 || tiers[0] == 5)
                {
                    return "500mmBigBerthaDisplay";
                }
                if (tiers[1] == 1)
                {
                    return "DwarvenArtilleryDisplay";
                }
                if (tiers[1] == 2)
                {
                    return "DwarvenHowitzerDisplay";
                }
                return "DwarvenBombardDisplay";
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                return Game.instance.model.GetTowerFromId("EngineerMonkey").GetTowerSetIndex() + 2;
            }
        }
        public class DwarvenArtillery : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Dwarven Artillery";
            public override string Description => "Bomb's explosion deals more than double the damage! \"Want some, get some!\"";
            public override int Cost => 2200;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var proj = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                proj.GetDamageModel().damage = 30.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "DwarvenArtillery_Portrait";
        }
        public class DwarvenHowitzer : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Dwarven Howitzer";
            public override string Description => "Bomb's explosion deals even more damage. \"Hail to the king, baby!\"";
            public override int Cost => 3200;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.125f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.125f;
                var proj = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                proj.GetDamageModel().damage = 40.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "DwarvenHowitzer_Portrait";
        }
        public class _500mmBigBertha : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "500mm Big Bertha";
            public override string Description => "A deadly explosion that destroys Bloons... \"Say hello to my little friend!\"";
            public override int Cost => 4000;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 3.5f;
                var proj = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile;
                proj.GetDamageModel().damage = 90.0f;
                proj.radius *= 1.4f;
                attackModel.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale *= 1.4f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "500mmBigBertha_Portrait";
        }
        public class DragonbreathLauncher : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Dragonbreath Launcher";
            public override string Description => "Launches a seeking missile that does huge damage to Bloons. \"Guaranteed mayhem!\"";
            public override int Cost => 3500;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel(1);
                attackModel.range *= 1.20f;
                attackModel.weapons[0].Rate = 13.0f;
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-003").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 100.0f;

            }
            public override string Icon => "DragonbreathLauncher_Icon";
            public override string Portrait => "500mmBigBertha_Portrait";
        }
        public class ClusterLauncherXtreme : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Cluster Launcher Xtreme";
            public override string Description => "Every 5th bomb explodes into Mini-Bombs. \"Guaranteed mayhem!\"";
            public override int Cost => 4500;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var proj = attackModel.weapons[0].projectile.Duplicate();
                proj.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage /= 2f;
                proj.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", proj.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 5, 0.0f, 360.0f, null, false, false), false, false, false));
                attackModel.weapons[0].AddBehavior(new AlternateProjectileModel("AlternateProjectileModel_", proj, new SingleEmissionModel("SEM_", null), 4, -1)); 

            }
            public override string Icon => "ClusterLauncherXtreme_Icon";
            public override string Portrait => "500mmBigBertha_Portrait";
        }
        public class Teslax104 : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Tesla x104";
            public override string Description => "Fires bolts of Lightning that hit multiple Bloons at once. \"Charged and Ready!\"";
            public override int Cost => 3750;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.03f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.03f;
                attackModel.weapons[0].Rate = 2.5f;
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("Druid-200").GetAttackModel().weapons[1].projectile.Duplicate();
                attackModel.weapons[0].projectile.pierce = 4.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 40.0f;
                attackModel.weapons[0].projectile.GetBehavior<LightningModel>().splitRange /= 2.0f;
                var effect = attackModel.weapons[0].projectile.GetBehavior<CreateLightningEffectModel>();
                effect.displayPaths[0] = ModContent.GetDisplayGUID<Lightning1>();
                effect.displayPaths[1] = ModContent.GetDisplayGUID<Lightning2>();
                effect.displayPaths[2] = ModContent.GetDisplayGUID<Lightning3>();
                effect.displayPaths[3] = ModContent.GetDisplayGUID<Lightning4>();
                effect.displayPaths[4] = ModContent.GetDisplayGUID<Lightning5>();
                effect.displayPaths[5] = ModContent.GetDisplayGUID<Lightning6>();
                effect.displayPaths[6] = ModContent.GetDisplayGUID<Lightning7>();
                effect.displayPaths[7] = ModContent.GetDisplayGUID<Lightning8>();
                effect.displayPaths[8] = ModContent.GetDisplayGUID<Lightning9>();
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "Teslax104_Portrait";
        }
        public class Lightning1 : ModDisplay
        {
            public override string BaseDisplay => "548c26e4e668dac4a850a4c016916016";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning1");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning2 : ModDisplay
        {
            public override string BaseDisplay => "ffed377b3e146f649b3e6d5767726a44";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning2");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning3 : ModDisplay
        {
            public override string BaseDisplay => "c5e4bf0202becd0459c47b8184b4588f";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning3");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning4 : ModDisplay
        {
            public override string BaseDisplay => "3e113b397a21a3a4687cf2ed0c436ec8";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning4");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning5 : ModDisplay
        {
            public override string BaseDisplay => "c6c2049a0c01e8a4d9904db8c9b84ca0";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning5");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning6 : ModDisplay
        {
            public override string BaseDisplay => "e9b2a3d6f0fe0e4419a423e4d2ebe6f6";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning6");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning7: ModDisplay
        {
            public override string BaseDisplay => "c8471dcde4c65fc459f7846c6a932a8c";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning7");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning8 : ModDisplay
        {
            public override string BaseDisplay => "a73b565de9c31c14ebcd3317705ab17e";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning8");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Lightning9 : ModDisplay
        {
            public override string BaseDisplay => "bd23939e7362b8e40a3a39f595a2a1dc";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Lightning9");
            }
            public override float PixelsPerUnit => 4.0f;
        }
        public class Overcharge : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Overcharge";
            public override string Description => "Damages all Bloons around it when attacking. \"Ride the lightning!\"";
            public override int Cost => 3500;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                //attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnIntervalModel("CreateProjectileOnIntervalModel_", Game.instance.model.GetTowerFromId("SuperMonkey-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.Duplicate(), new SingleEmissionModel("SingleEmissionModel_", null), 1, false, 1.0f, "First"));
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[1].projectile = Game.instance.model.GetTowerFromId("SuperMonkey-040").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.Duplicate();
                var effect = Game.instance.model.GetTowerFromId("SuperMonkey-040").GetAbility().GetBehavior<CreateEffectOnAbilityModel>().effectModel.Duplicate();
                effect.scale *= 0.5f;
                attackModel.weapons[1].projectile.AddBehavior(new Assets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel("CreateEffectOnContactModel_", effect.assetId, effect.lifespan, false, false, effect));
                attackModel.weapons[1].projectile.radius *= 0.5f;
                attackModel.weapons[1].projectile.GetDamageModel().damage = 25.0f;

            }
            public override string Icon => "Overcharge_Icon";
            public override string Portrait => "Teslax104_Portrait";
        }
        public class SuperchargedBolt : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Overcharge";
            public override string Description => "Supercharged lightning bolts can pop triple the amount of Bloons!. \"Ride the lightning!\"";
            public override int Cost => 2500;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.radius *= 3.0f;
                attackModel.weapons[0].projectile.GetBehavior<LightningModel>().splitRange *= 1.5f;

            }
            public override string Icon => "SuperchargedBolt_Icon";
            public override string Portrait => "Teslax104_Portrait";
        }






        public class DummyUpgrade1___ : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Buy me for 500mm Big Bertha.";
            public override string Description => "Buy this after you get tier 2 middle path. 500mm Big Bertha: A deadly explosion that destroys Bloons... \"Say hello to my little friend!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade2___ : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Buy me for 500mm Big Bertha";
            public override string Description => "Buy this after you get tier 2 middle path. 500mm Big Bertha: A deadly explosion that destroys Bloons... \"Say hello to my little friend!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade3___ : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Buy me for Telsa x104.";
            public override string Description => "Buy this after you get tier 2 middle path. Telsa x104: Fires bolts of Lightning that hit multiple Bloons at once. \"Charged and Ready!\"";
            public override int Cost => 0;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade4___ : ModUpgrade<DwarvenBombard>
        {
            public override string DisplayName => "Buy me for Sorcerer Mage.";
            public override string Description => "Buy this after you get tier 2 middle path. Telsa x104: Fires bolts of Lightning that hit multiple Bloons at once. \"Charged and Ready!\"";
            public override int Cost => 0;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
    }
}