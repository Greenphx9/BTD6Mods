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
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Weapons;

namespace minicustomtowersv2
{
    public class BatMonkeyTower
    {
        public class BatMonkey : ModTower
        {
            public override string BaseTower => "SuperMonkey-003";
            public override string Name => "BatMonkey";
            public override int Cost => 1900;
            public override string DisplayName => "Bat Monkey";
            public override string Description => "Throws sharp batarangs at bloons.";
            public override string TowerSet => "Magic";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = Game.instance.model.GetTowerFromId("SuperMonkey-003").portrait;
                towerModel.portrait = Game.instance.model.GetTowerFromId("SuperMonkey-003").portrait;
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 0.1f;
                attackModel.weapons[0].projectile.RemoveBehavior<KnockbackModel>();
                attackModel.weapons[0].projectile.pierce = 2.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 1.0f;
                towerModel.RemoveBehavior<AbilityModel>();
            }
        }
        public class SharperBatarangs : ModUpgrade<BatMonkey>
        {
            public override string Name => "SharperBatarangs";
            public override string DisplayName => "Sharper Batarangs";
            public override string Description => "Sharper Batarangs can pop through 3 bloons and deal 2 damage.";
            public override int Cost => 1105;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "SharpBatarangs_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.pierce += 1.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage += 1.0f;
            }
        }
        public class DoubleBatarang : ModUpgrade<BatMonkey>
        {
            public override string Name => "DoubleBatarang";
            public override string DisplayName => "Double Batarang";
            public override string Description => "Throws 2 batarangs at once! Range is also increased.";
            public override int Cost => 2900;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "DoubleBatarang_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.display = "aaf638baa0f066a40a91bfbf51f085c7";
                var attackModel = towerModel.GetAttackModel();
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[0].ejectX += 10f;
                attackModel.weapons[1].ejectX -= 10f;
                towerModel.range += 10f;
                attackModel.range = towerModel.range;
            }
        }
        public class LaserBatarangs : ModUpgrade<BatMonkey>
        {
            public override string Name => "LaserBatarangs";
            public override string DisplayName => "Laser Batarangs";
            public override string Description => "Laser Batarangs have more pierce, deal more damage, can pop leads, and excel at popping cermaic bloons!";
            public override int Cost => 7900;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "LaserBatarangs_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.display = "1ce5742aa11421441a85fb244bb451ad";
                var attackModel = towerModel.GetAttackModel();
                foreach(WeaponModel weaponModel in attackModel.weapons)
                {
                    weaponModel.projectile.display = "5d88a6eeaf733324ea8fcfc9d19013b3";
                    weaponModel.projectile.pierce += 2.0f;
                    weaponModel.projectile.GetDamageModel().damage += 1.0f;
                    weaponModel.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Purple;
                    weaponModel.projectile.AddBehavior(new DamageModifierForTagModel("Ceramic", "Ceramic", 1.0f, 5.0f, false, true));
                }
            }
        }
        public class EnergyBatarangs : ModUpgrade<BatMonkey>
        {
            public override string Name => "EnergyBatarangs";
            public override string DisplayName => "Energy Batarangs";
            public override string Description => "Energy Batarangs have even more pierce, and deal even more damage!";
            public override int Cost => 15000;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "EnergyBatarangs_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<BatMonkey4Display>();
                var attackModel = towerModel.GetAttackModel();
                foreach (WeaponModel weaponModel in attackModel.weapons)
                {
                    weaponModel.projectile.pierce += 2.0f;
                    weaponModel.projectile.GetDamageModel().damage += 3.0f;
                    weaponModel.projectile.ApplyDisplay<EnergyBatarangDisplay>();
                }
            }
        }
        public class BatMonkey4Display : ModDisplay
        {
            public override string BaseDisplay => "1ce5742aa11421441a85fb244bb451ad";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "BatMonkey4Display");
            }
        }
        public class EnergyBatarangDisplay : ModDisplay
        {
            public override string BaseDisplay => "5d88a6eeaf733324ea8fcfc9d19013b3";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "EnergyBatarangDisplay");
            }
            public override float PixelsPerUnit => 7.4f;
        }
        public class ExplosiveBatarangs : ModUpgrade<BatMonkey>
        {
            public override string Name => "ExplosiveBatarangs";
            public override string DisplayName => "Explosive Batarangs";
            public override string Description => "Batarangs explode when hitting a bloon!";
            public override int Cost => 56000;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override string Icon => "SharpBatarangs_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<BatMonkey4Display>();
                var attackModel = towerModel.GetAttackModel();
                foreach (WeaponModel weaponModel in attackModel.weapons)
                {
                    weaponModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                    weaponModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
                    weaponModel.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                    weaponModel.projectile.GetBehavior<CreateEffectOnContactModel>().effectModel.scale *= 0.5f;
                }
            }
        }
    }

}