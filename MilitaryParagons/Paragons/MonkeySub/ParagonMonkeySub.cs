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
    public class ParagonMonkeySub
    {
        public class MonkeySubParagon : ModVanillaParagon
        {
            public override string BaseTower => "MonkeySub-205";
        }
        public class FirstStrikeCommander : ModParagonUpgrade<MonkeySubParagon>
        {
            public override string DisplayName => "First Strike Commander";
            public override int Cost => 950000;
            public override string Description => "A submarine that fires 20 deadly missiles every second. What could go wrong?";
            public override string Icon => "FirstStrikeCommander_Icon";
            public override string Portrait => "FirstStrikeCommander_Portrait";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission.Cast<EmissionWithOffsetsModel>().projectileCount = 4;
                attackModel.weapons[0].projectile.pierce = 125.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 150.0f;
                attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
                attackModel.weapons[0].Rate = 0.15f;
                towerModel.range *= 1.75f;
                attackModel.range *= 1.75f;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-050").GetAttackModel(1).Duplicate());
                var attackModel2 = towerModel.GetAttackModel(1);
                attackModel2.weapons[0].Rate = 0.05f;
                attackModel2.range = 1000.0f;
                attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 1000.0f;

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-050").GetBehavior<PreEmptiveStrikeLauncherModel>().Duplicate());

                var submergeEffect = Game.instance.model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeEffectModel>().effectModel;
                var submerge = Game.instance.model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeModel>();
                towerModel.AddBehavior(new HeroXpScaleSupportModel("HeroXpScaleSupportModel_", true, submerge.heroXpScale, null));
                towerModel.AddBehavior(new AbilityCooldownScaleSupportModel("AbilityCooldownScaleSupportModel_", true, submerge.abilityCooldownSpeedScale, true, false, null, submerge.buffLocsName, submerge.buffIconName, false, submerge.supportMutatorPriority));

                foreach (var attackModels in towerModel.GetAttackModels())
                {
                    if (attackModels.name.Contains("Submerge"))
                    {
                        attackModels.name = attackModel.name.Replace("Submerged", "");
                        attackModels.weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId = submerge.attackDisplayPath;
                    }

                    attackModel.RemoveBehavior<SubmergedTargetModel>();
                }

                towerModel.AddBehavior(new CreateEffectAfterTimeModel("CreateEffectAfterTimeModel_", submergeEffect, 0f, true));

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class FirstStrikeCommanderDisplay : ModTowerDisplay<MonkeySubParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeySub, 2, 5, 0);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("FirstStrikeCommander_Display");
                    //node.SaveMeshTexture();
                }
                //node.GetMeshRenderer().material.mainTexture = GetTexture("FirstStrikeCommander_Display");
            }
        }
        /*public static TowerModel MonkeySubParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("MonkeySub-205").Duplicate();
            TowerModel backup = model.GetTowerFromId("MonkeySub-205").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "MonkeySub";
            towerModel.name = "MonkeySub-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "MonkeySub Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("FirstStrikeCommander_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("FirstStrikeCommander_Icon");
            towerModel.portrait = ModContent.GetSpriteReference<Main>("FirstStrikeCommander_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            towerModel.ApplyDisplay<MonkeySubParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = ModContent.GetDisplayGUID<MonkeySubParagonDisplay>());
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].emission.Cast<EmissionWithOffsetsModel>().projectileCount = 4;
            attackModel.weapons[0].projectile.pierce = 125.0f;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 150.0f;
            attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
            attackModel.weapons[0].Rate = 0.15f;
            towerModel.range *= 1.75f;
            attackModel.range *= 1.75f;
            towerModel.AddBehavior(model.GetTowerFromId("MonkeySub-050").GetAttackModel(1).Duplicate());
            var attackModel2 = towerModel.GetAttackModel(1);
            attackModel2.weapons[0].Rate = 0.05f;
            attackModel2.range = 1000.0f;
            attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 1000.0f;

            towerModel.AddBehavior(model.GetTowerFromId("MonkeySub-050").GetBehavior<PreEmptiveStrikeLauncherModel>().Duplicate());

            var submergeEffect = model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeEffectModel>().effectModel;
            var submerge = model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeModel>();
            towerModel.AddBehavior(new HeroXpScaleSupportModel("HeroXpScaleSupportModel_", true, submerge.heroXpScale, null));
           towerModel.AddBehavior(new AbilityCooldownScaleSupportModel("AbilityCooldownScaleSupportModel_",true, submerge.abilityCooldownSpeedScale, true, false, null, submerge.buffLocsName, submerge.buffIconName, false, submerge.supportMutatorPriority));

            foreach (var attackModels in towerModel.GetAttackModels())
            {
                if (attackModels.name.Contains("Submerge"))
                {
                    attackModels.name = attackModel.name.Replace("Submerged", "");
                    attackModels.weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId = submerge.attackDisplayPath;
                }

                attackModel.RemoveBehavior<SubmergedTargetModel>();
            }

            towerModel.AddBehavior(new CreateEffectAfterTimeModel("CreateEffectAfterTimeModel_", submergeEffect, 0f, true));

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class MonkeySubParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("MonkeySub-250").display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach(var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("FirstStrikeCommander_Display");
                }
                //node.GetMeshRenderer().material.mainTexture = GetTexture("FirstStrikeCommander_Display");
            }
        }*/
    }
    
}
