using MelonLoader;
using HarmonyLib;
using Assets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using UnityEngine.UI;
[assembly: MelonInfo(typeof(KingdomRushTowers.Main), "Kingdom Rush Towers", "1.0.0", "Greenphx, Kingdom Rush Devs")]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace KingdomRushTowers
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Kingdom Rush Towers Loaded!");
        }
        [HarmonyPatch(typeof(StandardTowerPurchaseButton), nameof(StandardTowerPurchaseButton.UpdateIcon))]
        public class SetTower
        {
            [HarmonyPrefix]
            internal static bool Prefix(ref StandardTowerPurchaseButton __instance)
            {
                __instance.bg = __instance.gameObject.GetComponent<Image>();
                var towerModel = __instance.towerModel;
                if (towerModel.baseId.Contains("KingdomRushTowers"))
                {
                    __instance.SetBackground(ModContent.GetTexture<Main>("TowerContainerKingdomRush"));
                }
                return true;
            }
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if(InGame.instance != null && InGame.instance.bridge != null)
            {
                foreach(var tower in InGame.instance.GetAllTowerToSim())
                {

                }
            }
        }
    }
}