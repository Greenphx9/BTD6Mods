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
using Assets.Scripts.Models.TowerSets;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using BTD_Mod_Helper.Api;
using UnityEngine.UI;
using Assets.Scripts.Unity.UI_New.InGame.RightMenu;
using Assets.Scripts.Unity.UI_New.Upgrade;
using UnhollowerBaseLib;
using NinjaKiwi.Common;
using BTD_Mod_Helper.Api.ModOptions;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Unity.Towers.Mods;

[assembly: MelonInfo(typeof(MiscTowersInShop.Main), "Misc Towers In Shop", "1.0.0", "Greenphx")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MiscTowersInShop
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            MelonLogger.Msg("Misc Towers In Shop Loaded");
        }
        public static ModSettingBool Sentries = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Sentries"
        };
        public static ModSettingBool MiniSunAvatarEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Mini Sun Avatar"
        };
        public static ModSettingBool MarineEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Marine"
        };
        public static ModSettingBool CaveMonkeyEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Cave Monkey"
        };
        public static ModSettingBool TransformedMonkeyEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Transformed Monkey"
        };
        public static ModSettingBool ApexPlasmaMasterEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Apex Plasma Master"
        };
        public static ModSettingBool GlaiveDominusEnabled = new ModSettingBool(true)
        {
            IsButton = false,
            displayName = "Glaive Dominus"
        };
        public class Sentry : ModTower
        {
            public override string BaseTower => "Sentry";
            public override string Name => "Sentry ";
            public override string DisplayName => "Sentry Gun";
            public override string Description => "Shoots little pins at bloons.";
            public override int Cost => 190;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("Sentry").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if(!Sentries)
                {
                    return -1;
                }
                return 100;
            }
        }
        public class BoomSentry : ModTower
        {
            public override string BaseTower => "SentryBoom";
            public override string Name => "BoomSentry";
            public override string DisplayName => "Boom Sentry";
            public override string Description => "Shoots small bombs at bloons.";
            public override int Cost => 500;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SentryBoom").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!Sentries)
                {
                    return -1;
                }
                return 101;
            }
        }
        public class ColdSentry : ModTower
        {
            public override string BaseTower => "SentryCold";
            public override string Name => "ColdSentry";
            public override string DisplayName => "Cold Sentry";
            public override string Description => "Shoots frozen balls at bloons, freezing them.";
            public override int Cost => 450;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SentryCold").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!Sentries)
                {
                    return -1;
                }
                return 102;
            }
        }
        public class CrushingSentry : ModTower
        {
            public override string BaseTower => "SentryCrushing";
            public override string Name => "CrushingSentry";
            public override string DisplayName => "Crushing Sentry";
            public override string Description => "Shoots small spiked balls.";
            public override int Cost => 475;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SentryCrushing").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!Sentries)
                {
                    return -1;
                }
                return 103;
            }
        }
        public class EnergySentry : ModTower
        {
            public override string BaseTower => "SentryEnergy";
            public override string Name => "EnergySentry";
            public override string DisplayName => "Energy Sentry";
            public override string Description => "Shoots small blasts of energy.";
            public override int Cost => 550;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SentryEnergy").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!Sentries)
                {
                    return -1;
                }
                return 104;
            }
        }
        public class ParagonSentry : ModTower
        {
            public override string BaseTower => "SentryParagon";
            public override string Name => "ParagonSentry";
            public override string DisplayName => "Paragon Sentry";
            public override string Description => "Shoots powerful blasts of purple energy.";
            public override int Cost => 5000;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SentryParagon").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.towerSize = TowerModel.TowerSize.small;
                towerModel.footprint = new CircleFootprintModel("CircleFootprintModel_", 3, false, false, false);
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!Sentries)
                {
                    return -1;
                }
                return 105;
            }
        }
        public class MiniSunAvatar : ModTower
        {
            public override string BaseTower => "SunAvatarMini";
            public override string Name => "MiniSunAvatar";
            public override string DisplayName => "Mini Sun Avatar";
            public override string Description => "Shoots powerful blasts of sun beams at bloons.";
            public override int Cost => 48600;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("SunAvatarMini").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!MiniSunAvatarEnabled)
                {
                    return -1;
                }
                return 106;
            }
        }
        public class Marine : ModTower
        {
            public override string BaseTower => "Marine";
            public override string Name => "Marine";
            public override string DisplayName => "Marine";
            public override string Description => "Rapidly fires piercing darts at bloons.";
            public override int Cost => 19000;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("Marine").portrait;
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!MarineEnabled)
                {
                    return -1;
                }
                return 107;
            }
        }
        public class CaveMonkey : ModTower
        {
            public override string BaseTower => "CaveMonkey";
            public override string Name => "CaveMonkey";
            public override string DisplayName => "Cave Monkey";
            public override string Description => "Slowly hits bloons with his club.";
            public override int Cost => 200;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("CaveMonkey").portrait;
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!CaveMonkeyEnabled)
                {
                    return -1;
                }
                return 108;
            }
        }
        public class TransformedMonkey : ModTower
        {
            public override string BaseTower => "TransformedBaseMonkey";
            public override string Name => "TransformedMonkey";
            public override string DisplayName => "Transformed Monkey";
            public override string Description => "Shoots blasts of lasers at bloons.";
            public override int Cost => 12000;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.RemoveBehavior<TowerExpireModel>();
                towerModel.dontDisplayUpgrades = true;
            }
            public override string Icon => "TransformedMonkeyIcon2";
            public override string Portrait => "TransformedMonkeyIcon2";
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!TransformedMonkeyEnabled)
                {
                    return -1;
                }
                return 109;
            }
        }
        public class ApexPlasmaMaster : ModTower
        {
            public override string BaseTower => "DartMonkey-Paragon";
            public override string Name => "ApexPlasmaMaster";
            public override string DisplayName => "Apex Plasma Master";
            public override string Description => "Fill the area with Bloon shredding plasma juggernaut balls, leaving nothing behind...";
            public override int Cost => 435000;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("DartMonkey-Paragon").portrait;
                towerModel.isParagon = false;
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!ApexPlasmaMasterEnabled)
                {
                    return -1;
                }
                return 110;
            }
        }
        public class GlaiveDominus : ModTower
        {
            public override string BaseTower => "BoomerangMonkey-Paragon";
            public override string Name => "GlaiveDominus";
            public override string DisplayName => "Glaive Dominus";
            public override string Description => "The Bloons will look upon my Glaives, and they will know fear.";
            public override int Cost => 527400;
            public override int TopPathUpgrades => 0;
            public override int MiddlePathUpgrades => 0;
            public override int BottomPathUpgrades => 0;
            public override string TowerSet => "Support";
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.icon = towerModel.portrait = Game.instance.model.GetTowerFromId("BoomerangMonkey-Paragon").portrait;
                towerModel.isParagon = false;
                towerModel.dontDisplayUpgrades = true;
            }
            public override int GetTowerIndex(List<TowerDetailsModel> towerSet)
            {
                if (!GlaiveDominusEnabled)
                {
                    return -1;
                }
                return 111;
            }
        }

        public override void OnGameModelLoaded(GameModel model)
        {
            base.OnGameModelLoaded(model);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton), nameof(StandardTowerPurchaseButton.UpdateIcon))]
        public class SetTower
        {
            [HarmonyPrefix]
            internal static bool Prefix(StandardTowerPurchaseButton __instance) //ref ShopMenu shopMenu, ref TowerModel towerModel
            {
                __instance.bg = __instance.gameObject.GetComponent<Image>();
                var towerModel = __instance.baseTowerModel;
                if(towerModel.baseId == "MiscTowersInShop-Sentry ")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerGreen"));
                }
                if (towerModel.baseId == "MiscTowersInShop-BoomSentry")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerBlack"));
                }
                if (towerModel.baseId == "MiscTowersInShop-ColdSentry")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerBlue"));
                }
                if (towerModel.baseId == "MiscTowersInShop-CrushingSentry")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerRed"));
                }
                if (towerModel.baseId == "MiscTowersInShop-EnergySentry")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerYellow"));
                }
                if (towerModel.baseId == "MiscTowersInShop-ParagonSentry")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerParagon"));
                }
                if (towerModel.baseId == "MiscTowersInShop-MiniSunAvatar")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerYellow"));
                }
                if (towerModel.baseId == "MiscTowersInShop-Marine")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerBlack"));
                }
                if (towerModel.baseId == "MiscTowersInShop-CaveMonkey")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerBrown"));
                }
                if (towerModel.baseId == "MiscTowersInShop-TransformedMonkey")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerBluePurple"));
                }
                if (towerModel.baseId == "MiscTowersInShop-ApexPlasmaMaster")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerParagon"));
                }
                if (towerModel.baseId == "MiscTowersInShop-GlaiveDominus")
                {
                    __instance.SetBG(ModContent.GetSprite<Main>("TowerContainerParagon"));
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(TowerImageLoader), nameof(TowerImageLoader.Load))]
        public class LoadTowerImageLoader
        {
            [HarmonyPrefix]
            internal static bool Prefix(TowerImageLoader __instance, ref string towerID)
            {
                __instance.bg.sprite = ModContent.GetSprite<Main>("TowerContainerBlack");
                return true;
            }
        }
        public static void TrySaveToPNG(Texture texture, string filePath)
        {
            try
            {
                RenderTexture tmp = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
                Graphics.Blit(texture, tmp);
                RenderTexture previous = RenderTexture.active;
                RenderTexture.active = tmp;
                Texture2D myTexture2D = new Texture2D(texture.width, texture.height);
                myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                myTexture2D.Apply();
                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(tmp);
                var bytes = ImageConversion.EncodeToPNG(myTexture2D);
                File.WriteAllBytes(filePath, bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }

}