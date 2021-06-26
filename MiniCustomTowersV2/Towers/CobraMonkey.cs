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
    public class CobraMonkeyTower
    {
        public class CobraMonkey : ModTower
        {
            public override string BaseTower => "SniperMonkey";
            public override string Name => "CobraMonkey";
            public override int Cost => 425;
            public override string DisplayName => "Cobra Monkey";
            public override string Description => "Pops bloons using his small gun.";
            public override bool Use2DModel => true;
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 4;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 4;
            public override float PixelsPerUnit => 16f;
            public override string Icon => "CobraMonkey_Icon";
            public override string Portrait => "CobraMonkey_Icon";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.isGlobalRange = false;
                towerModel.range = 40f;
                towerModel.GetAttackModel().range = towerModel.range;
                towerModel.GetAttackModel().weapons[0].Rate = 1.2f;
            }
            public override string Get2DTexture(int[] tiers)
            {
                return "Cobra";
            }
        }
        public class WiredFunds : ModUpgrade<CobraMonkey>
        {
            public override string Name => "WiredFunds";
            public override string DisplayName => "Wired Funds";
            public override string Description => "Gains 80 cash per round.";
            public override int Cost => 400;
            public override int Path => TOP;
            public override int Tier => 1;
            public override string Icon => "WiredFunds_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior<PerRoundCashBonusTowerModel>(new PerRoundCashBonusTowerModel("wiredfunds", 80.0f, 0.0f, 1.0f, "80178409df24b3b479342ed73cffb63d", false));
            }
        }
        public class BloonAdjustment : ModUpgrade<CobraMonkey>
        {
            public override string Name => "BloonAdjustment";
            public override string DisplayName => "Bloon Adjustment";
            public override string Description => "Every 15 seconds, takes 5 layers of a bloon. Also removes, camo, regrow, and fortified modifiers from the bloon.";
            public override int Cost => 650;
            public override int Path => TOP;
            public override int Tier => 2;
            public override string Icon => "BloonAdjustment_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var newAttack = towerModel.GetAttackModel().Duplicate();
                newAttack.name = "Bloon_Adjustment_AttackModel";
                towerModel.AddBehavior(newAttack);
                foreach(AttackModel attackModel in towerModel.GetAttackModels())
                {
                    if(attackModel.name == "Bloon_Adjustment_AttackModel")
                    {
                        attackModel.range = 500f;
                        attackModel.weapons[0].Rate = 15f;
                        attackModel.weapons[0].projectile.GetDamageModel().damage = 5f;
                        attackModel.weapons[0].projectile.AddBehavior(new RemoveBloonModifiersModel("bloonadjust", true, true, false, true, false, new Il2CppStringArray(0)));
                        towerModel.AddBehavior(new OverrideCamoDetectionModel("bloonadjust1", true));

                    }
                }
            }
        }
        public class MonkeyStim : ModUpgrade<CobraMonkey>
        {
            public override string Name => "MonkeyStim";
            public override string DisplayName => "Monkey Stim";
            public override string Description => "Increased attack speed. Unlocks the Tower Boost ability: increases all towers' attack speed by 30% for 10 seconds.";
            public override int Cost => 2750;
            public override int Path => TOP;
            public override int Tier => 3;
            public override string Icon => "MonkeyStim_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeyVillage-050").GetBehavior<AbilityModel>().Duplicate());
                var c2a = towerModel.GetBehavior<AbilityModel>().GetBehavior<CallToArmsModel>();
                c2a.multiplier = 1.3f;
                c2a.Lifespan /= 2;
                c2a.lifespanFrames /= 2;
                c2a.lifespan /= 2;
                towerModel.GetBehavior<AbilityModel>().icon = GetSpriteReference(mod, "MonkeyStim_Icon");
                towerModel.GetBehavior<AbilityModel>().RemoveBehavior<CreateEffectOnAbilityModel>();
                towerModel.GetBehavior<AbilityModel>().RemoveBehavior<CreateSoundOnAbilityModel>();
                towerModel.GetAttackModel().weapons[0].Rate *= 0.75f;
            }
        }
        public class OffensivePush : ModUpgrade<CobraMonkey>
        {
            public override string Name => "OffensivePush";
            public override string DisplayName => "Offensive Push";
            public override string Description => "Every second, all bloons in radius are damaged for 5 damage.";
            public override int Cost => 5000;
            public override int Path => TOP;
            public override int Tier => 4;
            public override string Icon => "OffensivePush_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var newWep = Game.instance.model.GetTowerFromId("TackShooter-402").GetAttackModel().weapons[0].Duplicate();
                newWep.name = "Offensive_Push_WeaponModel";
                towerModel.GetAttackModel().AddWeapon(newWep);
                foreach(WeaponModel weaponModel in towerModel.GetWeapons())
                {
                    if(weaponModel.name == "Offensive_Push_WeaponModel")
                    {
                        weaponModel.Rate = 1.0f;
                        weaponModel.projectile.display = "a";
                        weaponModel.projectile.pierce = 9999f;
                        weaponModel.projectile.GetBehavior<DisplayModel>().display = "a";
                        weaponModel.GetBehavior<EjectEffectModel>().effectModel.assetId = "a";
                        weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    }
                }
            }
        }
        public class DoubleTap : ModUpgrade<CobraMonkey>
        {
            public override string Name => "DoubleTap";
            public override string DisplayName => "Double Tap";
            public override string Description => "Dual wield pistols for double attack speed.";
            public override int Cost => 595;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override string Icon => "DoubleTap_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].Rate *= 0.5f;
            }
        }
        public class Attrition : ModUpgrade<CobraMonkey>
        {
            public override string Name => "Attrition";
            public override string DisplayName => "Attrition";
            public override string Description => "Adds 1 live after every round.";
            public override int Cost => 1200;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override string Icon => "Attrition_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(new BonusLivesPerRoundModel("attrition", 1, 1.0f, "eb70b6823aec0644c81f873e94cb26cc"));
            }
        }
        public class FinishHim : ModUpgrade<CobraMonkey>
        {
            public override string Name => "FinishHim";
            public override string DisplayName => "Finish Him!";
            public override string Description => "Increased damage. Unlocks the Bloon Bash ability: dazes all bloons on screen for 5 seconds.";
            public override int Cost => 5600;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override string Icon => "FinishHim_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3f;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MortarMonkey-050").GetBehavior<AbilityModel>().Duplicate());
                towerModel.GetBehavior<AbilityModel>().name = "BloonBash";
                towerModel.GetBehavior<AbilityModel>().displayName = "Bloon Bash";
                towerModel.GetBehavior<AbilityModel>().description = "Dazes all bloons on screen for 5 seconds.";
                towerModel.GetBehavior<AbilityModel>().icon = GetSpriteReference(mod, "FinishHim_Icon");
                towerModel.GetBehavior<AbilityModel>().RemoveBehavior<CreateSoundOnAbilityModel>();
                towerModel.GetBehavior<AbilityModel>().RemoveBehavior<CreateEffectOnAbilityModel>();
                var attackAbil = towerModel.GetBehavior<AbilityModel>().GetBehavior<ActivateAttackModel>();
                attackAbil.attacks[0].weapons[0].projectile.GetDamageModel().damage = 0f;
                attackAbil.attacks[0].weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 1.0f;
                attackAbil.attacks[0].weapons[0].projectile.GetBehavior<SlowModel>().Lifespan = 5.0f;
            }
        }
        public class Misdirection : ModUpgrade<CobraMonkey>
        {
            public override string Name => "Misdirection";
            public override string DisplayName => "Misdirection";
            public override string Description => "Ability: Blows back a bloon on the screen.";
            public override int Cost => 12000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override string Icon => "Misdirection_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var ability2 = Game.instance.model.GetTowerFromId("MortarMonkey-050").GetBehavior<AbilityModel>().Duplicate();
                ability2.name = "Misdirection";
                towerModel.AddBehavior(ability2);
                foreach(AbilityModel abilityModel in towerModel.GetAbilites())
                {
                    if (abilityModel.name == "Misdirection")
                    {
                        abilityModel.icon = GetSpriteReference(mod, "Misdirection_Icon");
                        abilityModel.RemoveBehavior<CreateSoundOnAbilityModel>();
                        abilityModel.RemoveBehavior<CreateEffectOnAbilityModel>();
                        var attackAbil = abilityModel.GetBehavior<ActivateAttackModel>();
                        attackAbil.attacks[0].range = 1000f;
                        attackAbil.attacks[0].weapons[0] = Game.instance.model.GetTowerFromId("DartMonkey").GetWeapon().Duplicate();
                        attackAbil.attacks[0].weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate());
                        var wind = attackAbil.attacks[0].weapons[0].projectile.GetBehavior<WindModel>();
                        wind.affectMoab = true;
                        wind.chance = 100f;
                        wind.distanceMax = 100000f;
                        wind.distanceMin = 100000f;
                        attackAbil.attacks[0].weapons[0].projectile.pierce = 1;
                        attackAbil.attacks[0].weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan = 4f;
                        attackAbil.attacks[0].weapons[0].projectile.GetBehavior<TravelStraitModel>().Speed = 1000f;
                        attackAbil.attacks[0].weapons[0].projectile.radius = 10000f;
                        attackAbil.attacks[0].RemoveBehavior<TargetFirstModel>();
                        attackAbil.attacks[0].RemoveBehavior<TargetLastModel>();
                        attackAbil.attacks[0].RemoveBehavior<TargetCloseModel>();
                    }
                }
            }
        }
    }

}