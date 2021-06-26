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
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;

namespace minicustomtowersv2
{
    public class UnloaderDartlingGunnerTower
    {
        public class UnloaderDartlingGunner : ModTower
        {
            public override string BaseTower => "DartlingGunner";
            public override string Name => "UnloaderDartlingGunner";
            public override int Cost => 4300;
            public override string DisplayName => "Unloader Dartling Gunner";
            public override string Description => "Hurls a spiked ball that pops everything that it touches.";
            public override string TowerSet => "Military";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate = 1.25f;
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 2, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[0].projectile.GetDamageModel().damage += 1.0f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[1].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[1].startInCooldown = true;
                attackModel.weapons[1].customStartCooldown = 0.075f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[2].emission = new ArcEmissionModel("ArcEmissionModel_", 4, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[2].startInCooldown = true;
                attackModel.weapons[2].customStartCooldown = 0.150f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[3].emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[3].startInCooldown = true;
                attackModel.weapons[3].customStartCooldown = 0.225f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[4].emission = new ArcEmissionModel("ArcEmissionModel_", 6, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[4].startInCooldown = true;
                attackModel.weapons[4].customStartCooldown = 0.300f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[5].emission = new ArcEmissionModel("ArcEmissionModel_", 7, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[5].startInCooldown = true;
                attackModel.weapons[5].customStartCooldown = 0.375f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[6].emission = new ArcEmissionModel("ArcEmissionModel_", 8, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[6].startInCooldown = true;
                attackModel.weapons[6].customStartCooldown = 0.450f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[7].emission = new ArcEmissionModel("ArcEmissionModel_", 9, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[7].startInCooldown = true;
                attackModel.weapons[7].customStartCooldown = 0.525f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[8].emission = new ArcEmissionModel("ArcEmissionModel_", 9, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[8].startInCooldown = true;
                attackModel.weapons[8].customStartCooldown = 0.600f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[9].emission = new ArcEmissionModel("ArcEmissionModel_", 10, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[9].startInCooldown = true;
                attackModel.weapons[9].customStartCooldown = 0.675f;
                attackModel.AddWeapon(attackModel.weapons[0].Duplicate());
                attackModel.weapons[10].emission = new ArcEmissionModel("ArcEmissionModel_", 11, 0.0f, 30.0f, null, false, false);
                attackModel.weapons[10].startInCooldown = true;
                attackModel.weapons[10].customStartCooldown = 0.750f;
                towerModel.ApplyDisplay<UnloaderDisplay>();
            }
            public override string Icon => "UnloaderDartlingGunner_Icon";
            public override string Portrait => "UnloaderDartlingGunner_Icon";
        }
        public class UnloaderDisplay : ModDisplay
        {
            public override string BaseDisplay => "f8d6088e1ff66c248a5e23c6851ba27a";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach(Renderer renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture(mod, "UnloaderDartlingGunner");
                }
            }
        }
        public class FlechetteDarts : ModUpgrade<UnloaderDartlingGunner>
        {
            public override string Name => "FlechetteDarts";
            public override string DisplayName => "Flechette Darts";
            public override string Description => "Darts split into smaller darts on contact.";
            public override int Cost => 12500;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "FlechetteDarts_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
               foreach(var proj in towerModel.GetAllProjectiles())
                {
                    proj.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-002").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate());
                    proj.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile = proj.Duplicate();
                    proj.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetDamageModel().damage -= 1.0f;
                    proj.GetBehavior<CreateProjectileOnExhaustFractionModel>().emission.Cast<ArcEmissionModel>().count = 1;
                }
            }
        }
        public class GoldenDart : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "GoldenDart");
            }
            public override float PixelsPerUnit => 4.3f;
        }
        public class GoldenBarrage : ModUpgrade<UnloaderDartlingGunner>
        {
            public override string Name => "GoldenBarrage";
            public override string DisplayName => "Golden Barrage";
            public override string Description => "Golden Darts do extra damage.";
            public override int Cost => 34000;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "GoldenBarrage_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                foreach (var proj in towerModel.GetAllProjectiles())
                {
                    proj.GetDamageModel().damage += 2.0f;
                    proj.ApplyDisplay<GoldenDart>();
                    proj.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                }
            }
        }
    }

}