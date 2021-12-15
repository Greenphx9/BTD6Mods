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

namespace MilitaryParagons.Paragons.Towers
{
    public class ParagonDartlingGunner
    {
        public class DartlingGunnerParagon : ModVanillaParagon
        {
            public override string BaseTower => "DartlingGunner-250";
        }
        public class RayOfMAD : ModParagonUpgrade<DartlingGunnerParagon>
        {
            public override string DisplayName => "Ray Of MAD";
            public override int Cost => 1700000;
            public override string Description => "A machine so powerful not even Dr Monkey could make it at full power. The explosive MAD bullets had to be less powerful for it to even be possible to make.";
            public override string Icon => "RayOfMAD_Icon";
            public override string Portrait => "RayOfMAD_Portrait";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                var attackModel = towerModel.GetAttackModel();
                attackModel.GetDescendants<DamageModifierForTagModel>().ForEach(damage => damage.damageMultiplier = 0.25f);
                attackModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.005f);
                attackModel.GetDescendants<ProjectileModel>().ForEach(proj => proj.ApplyDisplay<DartlingGunnerParagonDisplayProj>());
                towerModel.GetAbilites().ForEach(ability => ability.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.05f));
                towerModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("EPASEM")));
                attackModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-025").GetWeapon().projectile.GetBehavior<KnockbackModel>().Duplicate()));

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class RayOfMADDisplay : ModTowerDisplay<DartlingGunnerParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.DartlingGunner, 2, 5, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "RayOfMAD_Display");
                //node.GetMeshRenderer().material.mainTexture = GetTexture("FirstStrikeCommander_Display");
            }
        }
        /*public static TowerModel DartlingGunnerParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("DartlingGunner-250").Duplicate();
            TowerModel backup = model.GetTowerFromId("DartlingGunner-250").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "DartlingGunner";
            towerModel.name = "DartlingGunner-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "DartlingGunner Paragon";
            towerModel.appliedUpgrades = appliedUpgrades;

            towerModel.paragonUpgrade = null;
            towerModel.isSubTower = false;
            towerModel.isBakable = true;
            towerModel.powerName = null;
            towerModel.showPowerTowerBuffs = false;
            towerModel.animationSpeed = 1f;
            towerModel.towerSelectionMenuThemeId = "ActionButton";
            towerModel.ignoreCoopAreas = false;
            towerModel.canAlwaysBeSold = false;
            towerModel.isParagon = true;
            towerModel.icon = ModContent.GetSpriteReference<Main>("RayOfMAD_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("RayOfMAD_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("RayOfMAD_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<DartlingGunnerParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<DartlingGunnerParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            //var attackModel = towerModel.GetAttackModel();

            var attackModel = towerModel.GetAttackModel();
            attackModel.GetDescendants<DamageModifierForTagModel>().ForEach(damage => damage.damageMultiplier = 0.25f);
            attackModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.005f);
            attackModel.GetDescendants<ProjectileModel>().ForEach(proj => proj.ApplyDisplay<DartlingGunnerParagonDisplayProj>());
            towerModel.GetAbilites().ForEach(ability => ability.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.05f));
            towerModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("EPASEM")));
            attackModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(model.GetTowerFromId("DartlingGunner-025").GetWeapon().projectile.GetBehavior<KnockbackModel>().Duplicate()));

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class DartlingGunnerParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-250").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "RayOfMAD_Display");
                //node.SaveMeshTexture();
                
            }
        }*/
        public class DartlingGunnerParagonDisplayProj : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("DartlingGunner-250").GetWeapon().projectile.display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "RayOfMADProj_Display");
                //node.SaveMeshTexture();

            }
        }
    }
    
}
