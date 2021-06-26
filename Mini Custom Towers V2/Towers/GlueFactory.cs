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
    public class GlueFactoryTower
    {
        public class GlueFactory : ModTower
        {
            public override string BaseTower => "SpikeFactory";
            public override string Name => "GlueFactory";
            public override int Cost => 1200;
            public override string DisplayName => "Glue Factory";
            public override string Description => "Leaves a bunch of glue on the track";
            public override string TowerSet => "Support";
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 0;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.ApplyDisplay<GlueFactoryDisplay>();
                attackModel.weapons[0].projectile.collisionPasses = new int[] { -1, 0, 1 };
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate());
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate());
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModifierForTagModel>().Duplicate());
                foreach (var beh in Game.instance.model.GetTowerFromId("GlueGunner-200").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
                {
                    attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
                }
                attackModel.weapons[0].projectile.RemoveBehavior<DamageModel>();
                attackModel.weapons[0].projectile.RemoveBehavior<SetSpriteFromPierceModel>();
                attackModel.weapons[0].projectile.ApplyDisplay<GlueBlobYellow>();
                //attackModel.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-202").GetAttackModel().GetBehavior<AttackFilterModel>().Duplicate());
            }
            public override string Icon => "GlueFactory_Icon";
            public override string Portrait => "GlueFactory_Icon";
        }
        public class GlueFactoryDisplay : ModDisplay
        {
            public override string BaseDisplay => "b6602452ae95a5c46a343caa5460a9c1";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "GlueFactoryDisplay");
            }
        }
        public class GlueBlobYellow : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "GlueBlobYellow");
            }
        }
        public class StickyGlue : ModUpgrade<GlueFactory>
        {
            public override string Name => "StickyGlue";
            public override string DisplayName => "Sticky Glue";
            public override string Description => "Glue slows down bloons more.";
            public override int Cost => 750;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override string Icon => "StickyGlue_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.RemoveBehavior<SlowModel>();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-202").GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>().Duplicate());
            }
        }
        public class SharpGlue : ModUpgrade<GlueFactory>
        {
            public override string Name => "SharpGlue";
            public override string DisplayName => "Sharp Glue";
            public override string Description => "Glue deals damage to bloons on contact, as well as over time.";
            public override int Cost => 1500;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override string Icon => "SharpGlue_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().weapons[0].projectile.GetDamageModel().Duplicate());
            }
        }
        public class MOABGlue : ModUpgrade<GlueFactory>
        {
            public override string Name => "MOABGlue ";
            public override string DisplayName => "MOAB Glue ";
            public override string Description => "Glue can affect and slow moabs.";
            public override int Cost => 3200;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override string Icon => "MOABGlue_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.RemoveBehavior<SlowModifierForTagModel>();
            }
        }
        public class FasterGlueProduction : ModUpgrade<GlueFactory>
        {
            public override string Name => "FasterGlueProduction";
            public override string DisplayName => "Faster Glue Production";
            public override string Description => "Glue is spit out of the factory way faster.";
            public override int Cost => 7000;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override string Icon => "FasterGlueProduction_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].Rate *= 0.3f;
            }
        }
        public class GlueBlobGreen : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "GlueBlobGreen");
            }
        }
        public class LiquifyingGlue : ModUpgrade<GlueFactory>
        {
            public override string Name => "LiquifyingGlue";
            public override string DisplayName => "Liquifying Glue";
            public override string Description => "Glue does way more damage over time, and glue deals more damage when it hits a bloon.";
            public override int Cost => 32000;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override string Icon => "LiquifyingGlue_Icon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().damage += 2.0f;
                attackModel.weapons[0].projectile.RemoveBehavior<AddBehaviorToBloonModel>();
                attackModel.weapons[0].projectile.RemoveBehavior<AddBehaviorToBloonModel>();
                foreach (var beh in Game.instance.model.GetTowerFromId("GlueGunner-402").GetAttackModel().weapons[0].projectile.GetBehaviors<AddBehaviorToBloonModel>())
                {
                    attackModel.weapons[0].projectile.AddBehavior(beh.Duplicate());
                }
                //attackModel.weapons[0].projectile.display = "3ca745f21516ce341b1db741bfe019fb";
                attackModel.weapons[0].projectile.ApplyDisplay<GlueBlobGreen>();
                attackModel.weapons[0].projectile.RemoveBehavior<SlowModel>();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-402").GetWeapon().projectile.GetBehavior<SlowModel>().Duplicate());
            }
        }
    }

}