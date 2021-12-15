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
using UnhollowerBaseLib;
using Assets.Scripts.Models.Towers.Upgrades;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using NinjaKiwi.Common;
using Assets.Scripts.Models.Towers.Filters;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Unity.Towers.Mods;
using Assets.Scripts.Models.GenericBehaviors;

namespace PrimaryParagons.Paragons.Towers
{
    public class ParagonIceMonkey
    {
        public class IceMonkeyParagon : ModVanillaParagon
        {
            public override string BaseTower => "IceMonkey-520";
        }
        public class _0DegreesKelvin : ModParagonUpgrade<IceMonkeyParagon>
        {
            public override string DisplayName => "0° Kelvin";
            public override int Cost => 400000;
            public override string Description => "Only the strongest of Bloons are able to resist the cold icy winds.";
            public override SpriteReference IconReference => Game.instance.model.GetUpgrade("Snowstorm").icon;
            public override SpriteReference PortraitReference => Game.instance.model.GetTowerFromId("IceMonkey-520").portrait;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehavior<SlowBloonsZoneModel>().Duplicate());
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehavior<LinkDisplayScaleToTowerRangeModel>().Duplicate());
                towerModel.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.5f;
                towerModel.GetBehavior<SlowBloonsZoneModel>().bindRadiusToTowerRange = false;
                towerModel.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 9999.0f;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-042").GetBehaviors<DisplayModel>()[1].Duplicate());
                towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().baseTowerRange = 9999.0f;
                towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().displayRadius = 9999.0f;

                var attackModel = towerModel.GetAttackModel();
                towerModel.range *= 2.5f;
                attackModel.range *= 2.5f;
                attackModel.weapons[0].Rate = 0.25f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 100.0f;
                attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 50.0f;
                var iceShard = attackModel.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnPopModel>().Duplicate();
                iceShard.projectile.GetBehavior<TravelStraitModel>().Lifespan = 4.0f;
                iceShard.projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CPOEFM", iceShard.projectile, iceShard.emission, false, false, false));

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-025").GetAttackModel().Duplicate());
                var attackModel2 = towerModel.GetAttackModel(1);
                attackModel2.range = towerModel.range;
                attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 50.0f;
                attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class _0DegreesKelvinDisplay : ModTowerDisplay<IceMonkeyParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.IceMonkey, 5, 2, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {

            }
        }
        /*public static TowerModel IceMonkeyParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("IceMonkey-520").Duplicate();
            TowerModel backup = model.GetTowerFromId("IceMonkey-520").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "IceMonkey";
            towerModel.name = "IceMonkey-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "IceMonkey Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "Default";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;
            towerModel.isParagon = true;
            //towerModel.icon = ModContent.GetSpriteReference<Main>("SuperbGlue_Icon");
            //towerModel.instaIcon = ModContent.GetSpriteReference<Main>("SuperbGlue_Icon");
            //towerModel.portrait = ModContent.GetSpriteReference<Main>("SuperbGlue_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<IceMonkeyParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<IceMonkeyParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            towerModel.AddBehavior(model.GetTowerFromId("IceMonkey-042").GetBehavior<SlowBloonsZoneModel>().Duplicate());
            towerModel.AddBehavior(model.GetTowerFromId("IceMonkey-042").GetBehavior<LinkDisplayScaleToTowerRangeModel>().Duplicate());
            towerModel.GetBehavior<SlowBloonsZoneModel>().speedScale = 0.5f;
            towerModel.GetBehavior<SlowBloonsZoneModel>().bindRadiusToTowerRange = false;
            towerModel.GetBehavior<SlowBloonsZoneModel>().zoneRadius = 9999.0f;
            towerModel.AddBehavior(model.GetTowerFromId("IceMonkey-042").GetBehaviors<DisplayModel>()[1].Duplicate());
            towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().baseTowerRange = 9999.0f;
            towerModel.GetBehavior<LinkDisplayScaleToTowerRangeModel>().displayRadius = 9999.0f;

            var attackModel = towerModel.GetAttackModel();
            towerModel.range *= 2.5f;
            attackModel.range *= 2.5f;
            attackModel.weapons[0].Rate = 0.25f;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 100.0f;
            attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 50.0f;
            var iceShard = attackModel.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<EmitOnPopModel>().Duplicate();
            iceShard.projectile.GetBehavior<TravelStraitModel>().Lifespan = 4.0f;
            iceShard.projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenEdgeModel_"));
            attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CPOEFM", iceShard.projectile, iceShard.emission, false, false, false));

            towerModel.AddBehavior(model.GetTowerFromId("IceMonkey-025").GetAttackModel().Duplicate());
            var attackModel2 = towerModel.GetAttackModel(1);
            attackModel2.range = towerModel.range;
            attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 50.0f;
            attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);


            return towerModel;
        }
        public class IceMonkeyParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("IceMonkey-520").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {

            }
        }
        /*public class TackShooterParagonDisplay_Projectile : ModDisplay
        {
            public override string BaseDisplay => "e5edd901992846e409326a506d272633";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.GetRenderers<MeshRenderer>())
                {
                    renderer.material.mainTexture = GetTexture("MOABExecutioner_Display");
                }
            }
        }*/
    }
    
}
