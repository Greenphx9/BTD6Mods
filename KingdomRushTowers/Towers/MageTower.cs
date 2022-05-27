using HarmonyLib;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using System.Collections.Generic;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Api.Display;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Models.GenericBehaviors;
namespace KingdomRushTowers
{
    public static class MageTower_
    {
        public class MageTower : ModTower
        {
            public override string BaseTower => "WizardMonkey";
            public override string Name => "MageTower";
            public override int Cost => 1000;
            public override string DisplayName => "Mage Tower";
            public override string Description => "Throws balls of magic at Bloons. \"I put a spell on you!\"";
            public override string TowerSet => "Primary";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 2;
            public override int BottomPathUpgrades => 5;
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.radius = 10.0f;
                towerModel.doesntRotate = true;
                towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                towerModel.range *= 1.1f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.1f;
                attackModel.weapons[0].Rate = 1.5f;
                attackModel.weapons[0].projectile.pierce = 1.0f;
                attackModel.weapons[0].projectile.maxPierce = 1.0f;
                attackModel.weapons[0].projectile.CapPierce(1.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 14.0f;
            }
            public override string Icon => "MageTower_Icon";
            public override string Portrait => "MageTower_Icon";
            public override bool Use2DModel => true;
            public override string Get2DTexture(int[] tiers)
            {
                if (tiers[2] == 3 || tiers[2] == 4 || tiers[2] == 5)
                {
                    return "SorcererMageDisplay";
                }
                if (tiers[0] == 3 || tiers[0] == 4 || tiers[0] == 5)
                {
                    return "ArcaneWizardDisplay";
                }
                if (tiers[1] == 1)
                {
                    return "AdeptTowerDisplay";
                }
                if (tiers[1] == 2)
                {
                    return "WizardTowerDisplay";
                }
                return "MageTowerDisplay";
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                return Game.instance.model.GetTowerFromId("EngineerMonkey").GetTowerSetIndex() + 1;
            }
        }
        public class AdeptTower : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Adept Tower";
            public override string Description => "Range is slightly increased and Mage's magic deals more than double the damage! \"Like a charm!\"";
            public override int Cost => 1600;
            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.14f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.14f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 31.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "AdeptTower_Portrait";
        }
        public class WizardTower : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Wizard Tower";
            public override string Description => "Range is slightly increased again and Mage's magic is greatly increased! \"Might and Magic!\"";
            public override int Cost => 2400;
            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.12f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.12f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 62.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "WizardTower_Portrait";
        }
        public class ArcaneWizard : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Arcane Wizard";
            public override string Description => "Attack speed is decreased, but damage is massively increased. Range is also increased. \"Klaatu Barada Nikto!\"";
            public override int Cost => 3000;
            public override int Path => TOP;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.11f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.11f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 116.0f;
                attackModel.weapons[0].Rate = 2.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "ArcaneWizard_Portrait";
        }
        public class DeathRay : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Death Ray";
            public override string Description => "Every 18 seconds, fires a massive ball of magic that will insta-kill anything Bloon, and take a layer of MOABs/BFBs. \"Avada Kedavra!\"";
            public override int Cost => 3500;
            public override int Path => TOP;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel(1);
                attackModel.weapons[0].Rate = 18.0f;
                attackModel.range = 1000;
                attackModel.weapons[0].projectile.GetBehavior<TravelStraitModel>().Lifespan *= 3.5f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 999999.0f;
                attackModel.weapons[0].projectile.radius *= 3.0f;
                attackModel.weapons[0].projectile.scale *= 3.0f;
                attackModel.weapons[0].projectile.GetDamageModel().distributeToChildren = true;
                var filters = new FilterModel[]
                {
                    new FilterOutTagModel("FilterOutTagModel_", "Zomg", null),
                    new FilterOutTagModel("FilterOutTagModel_", "Ddt", null),
                    new FilterOutTagModel("FilterOutTagModel_", "Bad", null),
                };
                foreach(var filter in attackModel.weapons[0].projectile.filters)
                {
                    filters.AddItem(filter);
                }
                attackModel.weapons[0].projectile.filters = filters;
                attackModel.weapons[0].projectile.GetBehavior<ProjectileFilterModel>().filters = filters;
            }
            public override string Icon => "DeathRay_Icon";
            public override string Portrait => "ArcaneWizard_Portrait";
        }
        public class Teleport : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Teleport";
            public override string Description => "Every 9 seconds, fires a ball of magic that blows Bloons back. \"Space is merely a perception, a concern for mortal men.\"";
            public override int Cost => 4000;
            public override int Path => TOP;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel(2);
                attackModel.weapons[0].Rate = 9.0f;
                attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("Druid-300").GetAttackModel().weapons[1].projectile.Duplicate();
                attackModel.weapons[0].projectile.display = "54bcc286971344146a1cec38858b6b16";
                attackModel.weapons[0].projectile.GetBehavior<WindModel>().distanceMax *= 4.0f;
                attackModel.weapons[0].projectile.GetBehavior<WindModel>().distanceMin *= 4.0f;

            }
            public override string Icon => "Teleport_Icon";
            public override string Portrait => "ArcaneWizard_Portrait";
        }
        public class SorcererMage : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Sorcerer Mage";
            public override string Description => "Damage and Range is increased. \"Ashes to ashes!\"";
            public override int Cost => 3000;
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.range *= 1.11f;
                var attackModel = towerModel.GetAttackModel();
                attackModel.range *= 1.11f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 66.0f;
            }
            public override string Icon => "UpgradeIconKR";
            public override string Portrait => "SorcererMage_Portrait";
        }
        public class Polymorph : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Polymorph";
            public override string Description => "Every 14 seconds, transforms a enemy to a sheep (no display yet), slowing them down and allowing projectiles to deal more damage. \"Meeeh!\"";
            public override int Cost => 4000;
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(towerModel.GetAttackModel().Duplicate());
                var attackModel = towerModel.GetAttackModel(1);
                attackModel.weapons[0].Rate = 14.0f;
                attackModel.weapons[0].projectile.GetDamageModel().damage = 1f;
                attackModel.weapons[0].projectile.name = "test";
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-002").GetWeapon().projectile.GetBehavior<SlowModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<SlowModel>().overlayType="";
                attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("GlueGunner-050").GetAbilites()[0].GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().Duplicate());
                attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition = 3.0f;
                //var asset = new Il2CppSystem.Collections.Generic.Dictionary<string, Assets.Scripts.Models.Effects.AssetPathModel>();
                //for(int i = 0; i < Game.instance.model.bloons.Length; i++)
                //{
                //    addb.overlays.Add(Game.instance.model.bloons[i].baseId, new Assets.Scripts.Models.Effects.AssetPathModel(GetDisplayGUID<Sheep>(), GetDisplayGUID<Sheep>())) ;
                //}

            }
            public override string Icon => "Polymorph_Icon";
            public override string Portrait => "SorcererMage_Portrait";
        }
        public class Sheep : ModDisplay
        {
            public override string BaseDisplay => "9d3c0064c3ace7448bf8fefa4a97a70f";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "SheepBloon");
            }
        }
        public class SummonElemental : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Summon Elemental";
            public override string Description => "Every 8 seconds, summons an elemental which blocks and damages Bloons. \"Rock is Eternal.\"";
            public override int Cost => 6000;
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("ObynGreenfoot 3").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
                var attackModel = towerModel.GetAttackModel(2);
                attackModel.weapons[0].Rate = 8.0f;
                attackModel.weapons[0].projectile.GetBehavior<AgeModel>().Lifespan = 8.0f;
                attackModel.weapons[0].projectile.ApplyDisplay<Elemental>();
                attackModel.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                attackModel.weapons[0].projectile.pierce = 20.0f;
                attackModel.weapons[0].projectile.maxPierce = 20.0f;
                attackModel.weapons[0].projectile.CapPierce(20.0f);
                attackModel.weapons[0].projectile.GetDamageModel().damage = 30.0f;
                attackModel.weapons[0].projectile.radius *= 1.25f;
                attackModel.range = towerModel.range;

            }
            public override string Icon => "SummonElemental_Icon";
            public override string Portrait => "SorcererMage_Portrait";
        }
        public class Elemental : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, "Elemental");
            }
            public override float PixelsPerUnit => 2.4f;
        }




        public class DummyUpgrade1__ : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Buy me for Arcane Wizard.";
            public override string Description => "Buy this after you get tier 2 middle path. Arcane Wizard: Attack speed is decreased, but damage is massively increased. Range is also increased. \"Klaatu Barada Nikto!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade2__ : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Buy me for Arcane Wizard.";
            public override string Description => "Buy this after you get tier 2 middle path. Arcane Wizard: Attack speed is decreased, but damage is massively increased. Range is also increased. \"Klaatu Barada Nikto!\"";
            public override int Cost => 0;
            public override int Path => TOP;
            public override int Tier => 2;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade3__ : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Buy me for Sorcerer Mage.";
            public override string Description => "Buy this after you get tier 2 middle path. Sorcerer Mage: Damage and Range is increased. \"Ashes to ashes!\"";
            public override int Cost => 0;
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override void ApplyUpgrade(TowerModel towerModel)
            {

            }
            public override string Icon => "UpgradeIconKR";
        }
        public class DummyUpgrade4__ : ModUpgrade<MageTower>
        {
            public override string DisplayName => "Buy me for Sorcerer Mage.";
            public override string Description => "Buy this after you get tier 2 middle path. Sorcerer Mage: Damage and Range is increased. \"Ashes to ashes!\"";
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