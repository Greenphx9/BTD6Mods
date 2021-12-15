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
using Assets.Scripts.Models.GenericBehaviors;

namespace MilitaryParagons.Paragons.Towers
{
    public class ParagonMonkeyAce
    {
        public class MonkeyAceParagon : ModVanillaParagon
        {
            public override string BaseTower => "MonkeyAce-502";
        }
        public class NevaMissingShredderCommander : ModParagonUpgrade<MonkeyAceParagon>
        {
            public override string DisplayName => "Neva-Missing Shredder";
            public override int Cost => 1385000;
            public override string Description => "If only the Bloons knew what was about to hit them.";
            public override string Icon => "NevaMissingShredder_Icon";
            public override SpriteReference PortraitReference => Game.instance.model.GetTowerFromId("MonkeyAce-520").portrait;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();
                towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-003").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<TrackTargetModel>().turnRatePerFrame = 6.0f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 0.5f;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3.0f;
                attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count *= 2;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 55.0f;

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
                var attackModel2 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
                attackModel2.weapons[0].Rate = 0.075f;
                attackModel2.RemoveBehavior<CheckAirUnitOverTrackModel>();
                attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 275.0f);
                attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-250").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-052").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyAce-205").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());

                var lastAttack = towerModel.GetAttackModels().Last();
                lastAttack.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= 0.25f);
                lastAttack.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 40.0f);

                //since we cant buff it always make it hit camo
                towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
            }

        }
        public class NevaMissingShredderDisplay : ModTowerDisplay<MonkeyAceParagon>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeyAce, 5, 0, 2);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
            }
        }
        /*public static TowerModel MonkeyAceParagon(GameModel model)
        {
            TowerModel towerModel = model.GetTowerFromId("MonkeyAce-502").Duplicate();
            TowerModel backup = model.GetTowerFromId("MonkeyAce-502").Duplicate();
            //thanks to depletednova for this 
            towerModel.baseId = "MonkeyAce";
            towerModel.name = "MonkeyAce-Paragon";
            towerModel.tier = 6;
            towerModel.tiers = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").tiers;
            towerModel.upgrades = new Il2CppReferenceArray<UpgradePathModel>(0);
            var appliedUpgrades = new Il2CppStringArray(6);
            for (int upgrade = 0; upgrade < 5; upgrade++)
            {
                appliedUpgrades[upgrade] = backup.appliedUpgrades[upgrade];
            }
            appliedUpgrades[5] = "MonkeyAce Paragon";
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
            towerModel.icon = ModContent.GetSpriteReference<Main>("NevaMissingShredder_Icon");
            towerModel.instaIcon = ModContent.GetSpriteReference<Main>("NevaMissingShredder_Icon");
            //towerModel.portrait = ModContent.GetSpriteReference<Main>("PirateEmpire_Portrait");
            var boomerangParagon = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").Duplicate();

            //towerModel.ApplyDisplay<MonkeyAceParagonDisplay>();

            towerModel.AddBehavior(boomerangParagon.GetBehavior<ParagonTowerModel>());
            towerModel.GetBehavior<ParagonTowerModel>().displayDegreePaths.ForEach(path => path.assetPath = backup.display);
            towerModel.AddBehavior(boomerangParagon.GetBehavior<CreateSoundOnAttachedModel>());

            var attackModel = towerModel.GetAttackModel();
            attackModel.weapons[0].projectile.AddBehavior(model.GetTowerFromId("MonkeyAce-003").GetWeapon().projectile.GetBehavior<TrackTargetModel>().Duplicate());
            attackModel.weapons[0].projectile.GetBehavior<TrackTargetModel>().turnRatePerFrame = 6.0f;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed *= 0.5f;
            attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3.0f;
            attackModel.weapons[0].emission.Cast<ArcEmissionModel>().count *= 2;
            attackModel.weapons[0].projectile.GetDamageModel().damage = 55.0f;

            towerModel.AddBehavior(model.GetTowerFromId("MonkeyAce-220").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            var attackModel2 = towerModel.GetBehaviors<AttackAirUnitModel>()[2];
            attackModel2.weapons[0].Rate = 0.075f;
            attackModel2.RemoveBehavior<CheckAirUnitOverTrackModel>();
            attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 275.0f);
            attackModel2.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);

            towerModel.AddBehavior(model.GetTowerFromId("MonkeyAce-250").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            towerModel.AddBehavior(model.GetTowerFromId("MonkeyAce-052").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());
            towerModel.AddBehavior(model.GetTowerFromId("MonkeyAce-205").GetBehaviors<AttackAirUnitModel>()[1].Duplicate());

            var lastAttack = towerModel.GetAttackModels().Last();
            lastAttack.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= 0.25f);
            lastAttack.GetDescendants<DamageModel>().ForEach(damage => damage.damage = 40.0f);

            //since we cant buff it always make it hit camo
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);

            return towerModel;
        }
        public class MonkeyAceParagonDisplay : ModDisplay
        {
            public override string BaseDisplay => Game.instance.model.GetTowerFromId("MonkeyAce-502").GetBehavior<AirUnitModel>().display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                //foreach (var renderer in node.genericRenderers)
                //{
                    //renderer.material.mainTexture = GetTexture("PirateEmpire_Display");
  
                //}
                node.SaveMeshTexture();
            }
        }*/

    }
    
}
