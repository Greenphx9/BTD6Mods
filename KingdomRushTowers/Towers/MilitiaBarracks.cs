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
    public static class MilitiaBarracks_
    {
        public class MilitiaBarracks : ModTower
        {
            public override string BaseTower => "DartMonkey-002";
            public override string Name => "MilitiaBarracks";
            public override int Cost => 700;
            public override string DisplayName => "Militia Barracks";
            public override string Description => "Spawns helpful knights that fight Bloons. \"For honor and glory!\"";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 5;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.radius = 9.0f;
                towerModel.doesntRotate = true;
                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.RemoveBehavior<AttackModel>();
               
                //attackModel2.RemoveBehavior<TargetTrackOrDefaultModel>();
                //attackModel2.AddBehavior(new PursuitSettingModel("PursuitSettingModel_", true, 32.0f, true));
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("ObynGreenfoot 3").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 3.34f;
                attackModel.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 10.0f;
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender0>();
                attackModel.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                attackModel.weapons[0].projectile.pierce = 5.0f;
                attackModel.weapons[0].projectile.maxPierce = 5.0f;
                attackModel.weapons[0].projectile.CapPierce(5.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 2.0f;
                attackModel.weapons[0].projectile.radius *= 0.75f;
                attackModel.range = towerModel.range;
            }
            public override string Icon => "MilitiaBarracks_Icon";
            public override string Portrait => "MilitiaBarracks_Icon";
            public override bool Use2DModel => true;
            public override string Get2DTexture(int[] tiers)
            {
                if (tiers[2] == 3 || tiers[2] == 4 || tiers[2] == 5)
                {
                    return "BarbarianMeadHallDisplay";
                }
                if (tiers[0] == 3 || tiers[0] == 4 || tiers[0] == 5)
                {
                    return "HolyOrderDisplay";
                }
                if (tiers[1] == 1)
                {
                    return "FootmenBarracksDisplay";
                }
                if (tiers[1] == 2)
                {
                    return "KnightsBarracksDisplay";
                }
                return "MilitiaBarracksDisplay";
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                return Game.instance.model.GetTowerFromId("EngineerMonkey").GetTowerSetIndex() + 1;
            }
        }
        public class BarrackDefender0 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender0");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class FootmenBarracks : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Footmen Barracks";
            public override string Description => "Knights deal more damage and can pop more Bloons before disappearing. \"For the king!\"";
            public override int Cost => 1100;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce = 10.0f;
                attackModel.weapons[0].projectile.maxPierce = 10.0f;
                attackModel.weapons[0].projectile.CapPierce(10.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 3.0f;
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender1>();
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "FootmenBarracks_Portrait";
        }
        public class BarrackDefender1 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender1");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class KnightsBarracks : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Knights Barracks";
            public override string Description => "Knights deal even more damage, and have can pop more Bloons before disappearing. \"Have at thee!\"";
            public override int Cost => 1600;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce = 15.0f;
                attackModel.weapons[0].projectile.maxPierce = 15.0f;
                attackModel.weapons[0].projectile.CapPierce(15.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 8.0f;
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender2>();
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "KnightsBarracks_Portrait";
        }
        public class BarrackDefender2 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender2");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class HolyOrder : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Holy Order";
            public override string Description => "Knights deal more damage and they can pop more Bloon before disappearing. \"Justice Served!\"";
            public override int Cost => 2300;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce = 20.0f;
                attackModel.weapons[0].projectile.maxPierce = 20.0f;
                attackModel.weapons[0].projectile.CapPierce(20.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 16.0f;
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender3>();
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "HolyOrder_Portrait";
        }
        public class BarrackDefender3 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender3");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class ShieldOfValor : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Shield of Valor";
            public override string Description => "Enhances knight's protection, allowing them to pop 50% more Bloons. \"No retreat, No surrender!\"";
            public override int Cost => 2500;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce = 30.0f;
                attackModel.weapons[0].projectile.maxPierce = 30.0f;
                attackModel.weapons[0].projectile.CapPierce(30.0f);

            }
            public override string Icon => "ShieldOfValor_Icon";
            public override string Portrait => "HolyOrder_Portrait";
        }
        public class HolyStrike : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Holy Strike";
            public override string Description => "1 in 3 Knights absorb holy power, allowing them to deal 2x more damage to bloons. They can also pop lead Bloons, but cannot pop purple Bloons. \"By Holy fire be purged!\"";
            public override int Cost => 3000;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].AddBehavior(new AlternateProjectileModel("AlternateProjectileModel_", attackModel.weapons[0].projectile.Duplicate(), new SingleEmissionModel("SEM_", null), 3, -1));
                var proj = attackModel.weapons[0].GetBehavior<AlternateProjectileModel>().projectile;
                proj.ApplyDisplay<BarrackDefender4>();
                proj.GetDamageModel().damage *= 2.0f;
                proj.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                //proj.name = "test";
            }
            public override string Icon => "HolyStrike_Icon";
            public override string Portrait => "HolyOrder_Portrait";
        }
        public class BarrackDefender4 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender4");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class BarbarianMeadHall : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Barbarian Mead Hall";
            public override string Description => "Damage is greatly increased and Knights can pop more Bloons before disappearing. \"To the death!\"";
            public override int Cost => 5500;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender5>();
                attackModel.weapons[0].projectile.pierce = 25.0f;
                attackModel.weapons[0].projectile.maxPierce = 25.0f;
                attackModel.weapons[0].projectile.CapPierce(25.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 20.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "BarbarianMeadHall_Portrait";
        }
        public class BarrackDefender5 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender5");
            }
            public override float PixelsPerUnit => 2.4f;
        }
        public class WhirlwindAttack : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Whirlwind Attack";
            public override string Description => "After disappearing, Barbarians will damage Bloons around them. \"I got something to axe you...\"";
            public override int Cost => 3000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(new CreateProjectileOnExpireModel("CreateProjectileOnExpireModel_", Game.instance.model.GetTowerFromId("BombShooter-300").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.Duplicate(), new SingleEmissionModel("SEM", null), false));
                var proj = attackModel.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile;
                proj.pierce = 999999.0f;
                proj.maxPierce = 999999.0f;
                proj.CapPierce(999999.0f);
                proj.display = "efc680bebf80d1e4584e9548ddfbff34";
                proj.GetDamageModel().damage = 20.0f;
                proj.AddBehavior(new AgeModel("AgeModel_", 100, 0, false, new EndOfRoundClearBypassModel("EORCBM", "")));
                proj.GetBehavior<AgeModel>().Lifespan = 0.5f;
            }
            public override string Icon => "WhirlwindAttack_Icon";
            public override string Portrait => "BarbarianMeadHall_Portrait";
        }
        public class MoreAxes : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "More Axes";
            public override string Description => "Deals double damage using 2 axes, however, Barbarians pop less Bloons. \"Double the Axes. Double the fun!\"";
            public override int Cost => 5000;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.ApplyDisplay<BarrackDefender6>();
                attackModel.weapons[0].projectile.pierce = 10.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 40.0f;
            }
            public override string Icon => "MoreAxes_Icon";
            public override string Portrait => "BarbarianMeadHall_Portrait";
        }
        public class BarrackDefender6 : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BarrackDefender6");
            }
            public override float PixelsPerUnit => 2.4f;
        }


        public class DummyUpgrade1_ : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Buy me for Holy Order.";
            public override string Description => "Buy this after you get tier 2 middle path. Holy Order: Knights deal more damage and they can pop more Bloon before disappearing. \"Justice Served!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade2_ : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Buy me for Holy Order.";
            public override string Description => "Buy this after you get tier 2 middle path. Holy Order: Knights deal more damage and they can pop more Bloons before disappearing. \"Justice Served!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade3_ : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Buy me for Barbarian Mead Hall.";
            public override string Description => "Buy this after you get tier 2 middle path. Barbarian Mead Hall: Damage is greatly increased and they can pop more Bloons before disappearing. \"To the death!\"";
            public override int Cost => 0;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade4_ : ModUpgrade<MilitiaBarracks>
        {
            public override string DisplayName => "Buy me for Barbarian Mead Hall.";
            public override string Description => "Buy this after you get tier 2 middle path. Barbarian Mead Hall: Damage is greatly increased and they can pop more Bloons before disappearing. \"To the death!\"";
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