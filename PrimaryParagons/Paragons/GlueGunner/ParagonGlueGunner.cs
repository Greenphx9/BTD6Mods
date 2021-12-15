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

namespace PrimaryParagons.Paragons.Towers
{
    public class ParagonGlueGunner
    {
        public class GlueGunnerParagon : ModVanillaParagon
        {
            public override string BaseTower => "GlueGunner-205";
        }
        public class SuperbGlue : ModParagonUpgrade<GlueGunnerParagon>
        {
            public override string DisplayName => "Superb Glue";
            public override int Cost => 600000;
            public override string Description => "Glue that completely stops almost all Bloons and decimates every type of Bloon. Bloons affected by glue take extra damage.";
            public override string Icon => "SuperbGlue_Icon";
            public override string Portrait => "SuperbGlue_Portrait";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                var attackModel = towerModel.GetAttackModel();
                towerModel.range *= 1.5f;
                attackModel.range *= 1.5f;
                attackModel.weapons[0].Rate = 0.2f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-250").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 25.0f;
                attackModel.weapons[0].projectile.GetDescendants<DamageOverTimeModel>().ForEach(model3 => model3.Interval = 0.05f);

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class SuperbGlueDisplay : ModTowerDisplay<GlueGunnerParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.GlueGunner, 2, 0, 5);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("SuperbGlue_Display");
                    //node.SaveMeshTexture();
                }
            }
        }
        /*public static TowerModel GlueGunnerParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("GlueGunner-205").Duplicate();
            TowerModel backup = model.GetTowerFromId("GlueGunner-205").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "GlueGunner";
            towerModel.name = "GlueGunner-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "GlueGunner Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("SuperbGlue_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("SuperbGlue_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("SuperbGlue_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<GlueGunnerParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<GlueGunnerParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            towerModel.range *= 1.5f;
            attackModel.range *= 1.5f;
            attackModel.weapons[0].Rate = 0.2f;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
            attackModel.weapons[0].projectile.AddBehavior(model.GetTowerFromId("GlueGunner-250").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().Duplicate());
            attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 25.0f;
            attackModel.weapons[0].projectile.GetDescendants<DamageOverTimeModel>().ForEach(model3 => model3.Interval = 0.05f);

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }*/
        /*public class GlueGunnerParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("GlueGunner-205").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("SuperbGlue_Display");
                }
            }
        }*/
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
