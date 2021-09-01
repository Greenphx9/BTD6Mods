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

namespace SeaMonkeyGreenphx
{
    public class Main : BloonsTD6Mod
    {
       // public static AssetBundle assetBundle = null;
        public override void OnApplicationStart()
        {
            //assetBundle = AssetBundle.LoadFromMemory(Resource1.shaders);
            MelonLogger.Msg("Sea Monkey loaded!");
        }
        public class SeaMonkey : ModTower
        {
            public override string Name => "SeaMonkeyGreenphx";
            public override string DisplayName => "Sea Monkey";
            public override string Description => "Launches balls of water at the bloons to pop them.";
            public override string BaseTower => "DartMonkey-002";
            public override int Cost => 550;
            public override string TowerSet => MAGIC;
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 5;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.ApplyDisplay<SeaMonkeyDisplay>();
                attackModel.weapons[0].projectile.ApplyDisplay<BallOfWater>();
                attackModel.weapons[0].projectile.radius = 20.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 2.0f;
                UnhollowerBaseLib.Il2CppStructArray<AreaType> areaTypes = new UnhollowerBaseLib.Il2CppStructArray<AreaType>(2);
                towerModel.areaTypes = areaTypes;
                towerModel.areaTypes[0] = AreaType.land;
                towerModel.areaTypes[1] = AreaType.water;
                
                
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                return Game.instance.model.GetTowerFromId("Druid").GetTowerSetIndex() + 1;
            }
            public override string Icon => "SeaMonkey_Icon";
            public override string Portrait => "SeaMonkey_Icon";
        }
        public class SeaMonkeyDisplay : ModDisplay
        {
            public override string BaseDisplay => "cbac06a37a38a0746a4593de4a9b6296";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                SetMeshTexture(node, "SeaMonkeyDisplay");
            }
        }
        public class BallOfWater : ModDisplay
        {
            public override string BaseDisplay => "c73fd08146403e14fbcebd3cbf600b88";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "BallOfWater");
            }
        }
        public class Soak : ModUpgrade<SeaMonkey>
        {
            public override string Name => "Soak";
            public override string DisplayName => "Soak";
            public override string Description => "Pops 3 layers of bloon.";
            public override int Cost => 600;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().damage += 1.0f;
            }
            public override string Icon => Name + "_Icon";
        }
        public class SeepingShot : ModUpgrade<SeaMonkey>
        {
            public override string Name => "SeepingShot";
            public override string DisplayName => "Seeping Shot";
            public override string Description => "Can now pop lead bloons and does extra damage to ceramics.";
            public override int Cost => 1100;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_", "Ceramic", 1.0f, 3.0f, false, true));
            }
            public override string Icon => Name + "_Icon";
        }
        public class Diver : ModUpgrade<SeaMonkey>
        {
            public override string Name => "Diver";
            public override string DisplayName => "Diver";
            public override string Description => "Soaks the bloons in it's own diving spot (attacks similarly to an ice tower).";
            public override int Cost => 2500;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var attack = Game.instance.model.GetTowerFromId("TackShooter-400").GetAttackModel().Duplicate();
                attack.name = "Diver_Weapon";
                towerModel.AddBehavior(attack);
                foreach(var attacks in towerModel.GetAttackModels())
                {
                    if(attacks.name.Contains("Diver_Weapon"))
                    {
                        attacks.weapons[0].projectile.GetDamageModel().damage = 2.0f;
                        attacks.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                        attacks.weapons[0].projectile.pierce = 40.0f;
                        attacks.weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId = "f73f2e12a1827cd40b99cf65312a3a2f";
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class DeepDive : ModUpgrade<SeaMonkey>
        {
            public override string Name => "DeepDive";
            public override string DisplayName => "Deep Dive";
            public override string Description => "Larger radius, faster attack, dampens surrounding land areas and allows you to place water towers on them.";
            public override int Cost => 5900;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range *= 1.5f;
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    attacks.range *= 1.5f;
                    attacks.weapons[0].Rate *= 0.75f;
                }
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Pontoon").GetBehavior<AddMakeshiftAreaModel>().Duplicate());
                var makeshift = towerModel.GetBehaviors<AddMakeshiftAreaModel>()[0];
                makeshift.points[0] = new Assets.Scripts.Simulation.SMath.Vector3(-13.6f * 4f, -20.366888f * 4f, 12.5000057f * 4f);
                makeshift.points[1] = new Assets.Scripts.Simulation.SMath.Vector3(13.54f * 4f, -20.366888f * 4f, 12.5000057f * 4f);
                makeshift.points[2] = new Assets.Scripts.Simulation.SMath.Vector3(13.48f * 4f, 7.29312563f * 4f, 12.5000057f * 4f);
                makeshift.points[3] = new Assets.Scripts.Simulation.SMath.Vector3(-13.74f * 4f, 7.353125f * 4f, 12.5000057f * 4f);
                makeshift.points[4] = new Assets.Scripts.Simulation.SMath.Vector3(-13.6f * 4f, -20.366888f * 4f, 12.5000057f * 4f);
                makeshift.newAreaType = AreaType.water;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("IceMonkey-040").GetBehavior<FreezeNearbyWaterModel>().Duplicate());
                towerModel.GetBehavior<FreezeNearbyWaterModel>().radius = towerModel.range;
            }
            public override string Icon => Name + "_Icon";
        }
        public class Aquanaut : ModUpgrade<SeaMonkey>
        {
            public override string Name => "Aquanaut";
            public override string DisplayName => "Aquanaut";
            public override string Description => "Bloon popper of the deep.";
            public override int Cost => 36000;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    if (attacks.name.Contains("Diver_Weapon"))
                    {
                        attacks.weapons[0].projectile.GetDamageModel().damage += 20.0f;
                        attacks.weapons[0].projectile.pierce += 20.0f;
                        attacks.weapons[0].Rate = 0.1f;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
            public override string Portrait => Name + "_Portrait";
        }
        public class LandSight : ModUpgrade<SeaMonkey>
        {
            public override string Name => "LandSight";
            public override string DisplayName => "Land Sight";
            public override string Description => "Gives the sea monkey larger radius.";
            public override int Cost => 200;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.range *= 1.15f;
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    attacks.range *= 1.15f;
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class Corals : ModUpgrade<SeaMonkey>
        {
            public override string Name => "Corals";
            public override string DisplayName => "Corals";
            public override string Description => "Lays down sharp corals when idle.";
            public override int Cost => 500;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var caltrop = Game.instance.model.GetTowerFromId("NinjaMonkey-002").GetAttackModel(1).Duplicate();
                caltrop.range = towerModel.range;
                caltrop.name = "Corals_Weapon";
                towerModel.AddBehavior(caltrop);
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    if(attacks.name.Contains("Corals"))
                    {
                        attacks.weapons[0].projectile.ApplyDisplay<Coral>();
                        attacks.weapons[0].animateOnMainAttack = false;
                        attacks.weapons[0].animation = 0;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }

        public class Coral : ModDisplay
        {
            public override string BaseDisplay => "a6d991e94ab3af1438e751afde2d12fa";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Coral");
            }
        }
        public class RidgeShooter : ModUpgrade<SeaMonkey>
        {
            public override string Name => "RidgeShooter";
            public override string DisplayName => "Ridge Shooter";
            public override string Description => "Equipped with a canon that it uses to launch sharp corals at the bloons.";
            public override int Cost => 1100;
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                var ridge = Game.instance.model.GetTowerFromId("DartMonkey-023").GetAttackModel().Duplicate();
                ridge.range = towerModel.range;
                ridge.name = "RidgeShooter_Weapon";
                towerModel.AddBehavior(ridge);
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    if (attacks.name.Contains("RidgeShooter"))
                    {
                        attacks.weapons[0].projectile.ApplyDisplay<Coral>();
                        attacks.weapons[0].projectile.radius = 4.0f;
                        attacks.weapons[0].projectile.pierce += 2.0f;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class CoralWall : ModUpgrade<SeaMonkey>
        {
            public override string Name => "CoralWall";
            public override string DisplayName => "Coral Wall";
            public override string Description => "Ability: Creates a coral reef wall that blocks any bloons from moving past it. (lasts to 15 secs, does not stop moabs, wall does not do damage)";
            public override int Cost => 5500;
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("ObynGreenfoot 3").GetAbility().Duplicate());
                towerModel.GetAbility().icon = GetSpriteReference(mod, Name + "_Icon");
                towerModel.GetAbility().Cooldown = 50.0f;
                var activate = towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0];
                activate.weapons[0].projectile.display = "90d3b0ad8d5b2874eb1f09d90c715f4f";
                activate.weapons[0].projectile.RemoveBehavior<DamageModel>();
                activate.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 15.0f;
                activate.weapons[0].projectile.pierce = 99999999.0f;
                activate.weapons[0].projectile.AddBehavior(new KnockbackModel("KnockbackModel_", 0.0f, 1.0f, 1.0f, 0.1f, "Knockback"));
            }
            public override string Icon => Name + "_Icon";
        }
        public class AtollArchitect : ModUpgrade<SeaMonkey>
        {
            public override string Name => "AtollArchitect";
            public override string DisplayName => "Atoll Architect";
            public override string Description => "Ability: Creates a sharp coral reef wall that blocks even the biggest of bloons! (lasts up to 30 secs, stops everything below a zomg, does damage)";
            public override int Cost => 35600;
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.GetAbility().icon = GetSpriteReference(mod, Name + "_Icon");
                var activate = towerModel.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0];
                activate.weapons[0].projectile.AddBehavior(new DamageModel("DamageModel_", 2.0f, 2.0f, false, false, false, BloonProperties.None));
                activate.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 30.0f;
                activate.weapons[0].projectile.GetBehavior<KnockbackModel>().moabMultiplier = 1.0f;
            }
            public override string Icon => Name + "_Icon";
            public override string Portrait => Name + "_Portrait";
        }
        public class HeavyWaves : ModUpgrade<SeaMonkey>
        {
            public override string Name => "HeavyWaves";
            public override string DisplayName => "Heavy Waves";
            public override string Description => "Can now pop more bloons per shot.";
            public override int Cost => 450;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach(var attack in towerModel.GetAttackModels())
                {
                    foreach(var weapon in attack.weapons)
                    {
                        weapon.projectile.pierce += 3.0f;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class FasterSpewing : ModUpgrade<SeaMonkey>
        {
            public override string Name => "FasterSpewing";
            public override string DisplayName => "Faster Spewing";
            public override string Description => "Faster attack speed.";
            public override int Cost => 800;
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach (var attack in towerModel.GetAttackModels())
                {
                    foreach (var weapon in attack.weapons)
                    {
                        weapon.Rate *= 0.7f;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class DeEvolution : ModUpgrade<SeaMonkey>
        {
            public override string Name => "DeEvolution";
            public override string DisplayName => "De-evolution";
            public override string Description => "Looses it's legs, but gains a boost in attack (and attack speed).";
            public override int Cost => 1350;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                foreach (var attack in towerModel.GetAttackModels())
                {
                    foreach (var weapon in attack.weapons)
                    {
                        weapon.Rate *= 0.8f;
                        weapon.projectile.GetDamageModel().damage += 2.0f;
                    }
                }
            }
            public override string Icon => Name + "_Icon";
        }
        public class Siren : ModUpgrade<SeaMonkey>
        {
            public override string Name => "Siren";
            public override string DisplayName => "Siren";
            public override string Description => "Boosts attack speed and pierce for all Sea Monkeys in the radius. Can stack up to 5 times on a single Sea Monkey.";
            public override int Cost => 2800;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Druid-004").GetBehavior<PoplustSupportModel>().Duplicate());
            }
            public override string Icon => Name + "_Icon";
        }
        public class OceanicCommander : ModUpgrade<SeaMonkey>
        {
            public override string Name => "OceanicCommander";
            public override string DisplayName => "Oceanic Commander";
            public override string Description => "The more bloons there are, the more damage it does!";
            public override int Cost => 25400;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("Druid-005").GetAttackModel().weapons[0].projectile.GetBehavior<DamageModifierWrathModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<DamageModifierWrathModel>().damage += 2;
            }
            public override string Icon => Name + "_Icon";
            public override string Portrait => Name + "_Portrait";
        }
        public static Assets.Scripts.Simulation.SMath.Vector2 pos;
        public override string IDPrefix => "SeaMonkey_";
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(InGame.instance != null && InGame.instance.bridge != null)
            {
            }
        }
        public static Texture2D textureFromSprite(Sprite sprite)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        }

    }
}