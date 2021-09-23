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
    public static class ArcherTower_
    {
        public class ArcherTower : ModTower
        {
            public override string BaseTower => "DartMonkey-003";
            public override string Name => "ArcherTower";
            public override int Cost => 700;
            public override string DisplayName => "Archer Tower";
            public override string Description => "Shoots sharp arrows at Bloons. \"Dodge this!\"";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 5;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.radius = 9.0f;
                towerModel.doesntRotate = true;
                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.range *= 0.9f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 0.9f;
                attackModel.weapons[0].Rate = 0.8f;
                attackModel.weapons[0].projectile.pierce = 1.0f;
                attackModel.weapons[0].projectile.maxPierce = 1.0f;
                attackModel.weapons[0].projectile.CapPierce(1.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 5.0f;
            }
            public override string Icon => "ArcherTower_Icon";
            public override string Portrait => "ArcherTower_Icon";
            public override bool Use2DModel => true;
            public override string Get2DTexture(int[] tiers)
            {
                if (tiers[2] == 3 || tiers[2] == 4 || tiers[2] == 5)
                {
                    return "MusketeerGarrisonDisplay";
                }
                if (tiers[0] == 3 || tiers[0] == 4 || tiers[0] == 5)
                {
                    return "RangersHideoutDisplay";
                }
                if (tiers[1] == 1)
                {
                    return "MarksmenTowerDisplay";
                }
                if (tiers[1] == 2)
                {
                    return "SharpshooterTowerDisplay";
                }
                return "ArcherTowerDisplay";
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                return Game.instance.model.GetTowerFromId("EngineerMonkey").GetTowerSetIndex() + 1;
            }
        }
        public class MarksmenTower : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Marksmen Tower";
            public override string Description => "Damage, range, and attack speed is increased. \"Archers Ready!\"";
            public override int Cost => 1100;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.14f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.14f;
                attackModel.weapons[0].Rate = 0.6f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 8.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "MarksmenTower_Portrait";
        }
        public class SharpshooterTower : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Sharpshooter Tower";
            public override string Description => "Range and attack speed are slightly increased. Damage is greatly increased. \"Bulls-eye!\"";
            public override int Cost => 1600;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

                towerModel.range *= 1.125f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.125f;
                attackModel.weapons[0].Rate = 0.5f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 13.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "SharpshooterTower_Portrait";
        }
        public class RangersHideout : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Ranger's Hideout";
            public override string Description => "Damage, Range, and attack speed are slightly increased. \"Like a whisper!\"";
            public override int Cost => 2300;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.11f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.11f;
                attackModel.weapons[0].Rate = 0.4f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 16.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "RangersHideout_Portrait";
        }
        public class PoisonArrows : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Poison Arrows";
            public override string Description => "Adds DOT effect to arrows. \"It won't hurt... for long.\"";
            public override int Cost => 2500;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-300").GetAttackModel().weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().Duplicate());
                var addbeh = attackModel.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>();
                addbeh.glueLevel = -10;
                addbeh.lifespan = 3.0f;
                addbeh.GetBehavior<DamageOverTimeModel>().damage = 5;
                attackModel.weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
                attackModel.weapons[0].projectile.ApplyDisplay<PoisonArrow>();
            }
            public override string Icon => "PoisonArrows_Icon";
            public override string Portrait => "RangersHideout_Portrait";
        }
        public class WrathOfTheForest : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Wrath Of The Forest";
            public override string Description => "Rangers can now summon vines, instantly killing the Bloons targeted. DOT is increased. \"Its a trap!\"";
            public override int Cost => 7000;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetBehavior<AddBehaviorToBloonModel>().GetBehavior<DamageOverTimeModel>().damage = 10;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Druid-030").GetAttackModel(1).Duplicate());
                towerModel.GetAttackModel(1).weapons[0].Rate = 8.0f;
            }
            public override string Icon => "WrathOfTheForest_Icon";
            public override string Portrait => "RangersHideout_Portrait";
        }

        public class PoisonArrow : ModDisplay
        {
            public override string BaseDisplay => "0ddd8752be0d3554cb0db6abe6686e8e";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "PoisonArrow");
            }
        }
        public class MusketeerGarrison : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Musketeer Garrison";
            public override string Description => "Damage and range is increased greatly, but attack speed is decreased. \"Locked and Loaded!\"";
            public override int Cost => 2300;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.31f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.31f;
                attackModel.weapons[0].Rate = 1.5f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 50.0f;
                attackModel.weapons[0].projectile.display = "16b6c4224d027c0458d937e69cdc8b6f";
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 1.5f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "MusketeerGarrison_Portrait";
        }
        public class SniperShot : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Sniper Shot";
            public override string Description => "Has a chance to one shot any Bloon. \"One shot...one kill..\"";
            public override int Cost => 3000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].GetBehavior<CritMultiplierModel>().Duplicate());
                attackModel.weapons[0].GetBehavior<CritMultiplierModel>().damage = 99999;
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey-004").GetAttackModel().weapons[0].projectile.GetBehavior<ShowTextOnHitModel>().Duplicate());
            }
            public override string Icon => "SniperShot_Icon";
            public override string Portrait => "MusketeerGarrison_Portrait";
        }
        public class ShrapnelShot : ModUpgrade<ArcherTower>
        {
            public override string Name => "ShrapnelShot_ ";
            public override string DisplayName => "Shrapnel Shot";
            public override string Description => "Adds another weapon that explodes on contact and fires 6 grapes on contact. Has shorter range and very slow attack speed. \"Eat Grapeshot!\"";
            public override int Cost => 6000;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel(1);
                attackModel.range *= 0.5f;
                attackModel.weapons[0].Rate = 9.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 10.0f;
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetDamageModel().damage = 40.0f;
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", towerModel.GetAttackModel().weapons[0].projectile.Duplicate(), new ArcEmissionModel("ArcEmissionModel_", 6, 0.0f, 360.0f, null, false, false), false, false, false));
                attackModel.weapons[0].projectile.GetBehaviors<CreateProjectileOnContactModel>()[1].projectile.GetDamageModel().damage = 70.0f;
            }
            public override string Icon => "ShrapnelShot_Icon";
            public override string Portrait => "MusketeerGarrison_Portrait";
        }


        public class DummyUpgrade1 : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Buy me for Ranger's Hideout.";
            public override string Description => "Buy this after you get tier 2 middle path. Ranger's Hideout: Damage, Range, and attack speed are slightly increased. \"Like a whisper!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade2 : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Buy me for Ranger's Hideout";
            public override string Description => "Buy this after you get tier 2 middle path. Ranger's Hideout: Damage, Range, and attack speed are slightly increased. \"Like a whisper!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade3 : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Buy me for Musketeer Garrison.";
            public override string Description => "Buy this after you get tier 2 middle path. Musketeer Garrison: Damage and range is increased greatly, but attack speed is decreased. \"Locked and Loaded!\"";
            public override int Cost => 0;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade4 : ModUpgrade<ArcherTower>
        {
            public override string DisplayName => "Buy me for Musketeer Garrison.";
            public override string Description => "Buy this after you get tier 2 middle path. Musketeer Garrison: Damage and range is increased greatly, but attack speed is decreased. \"Locked and Loaded!\"";
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