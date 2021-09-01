using MelonLoader;
using HarmonyLib;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;

namespace SeaMonkeyGreenphx
{
    public static class Ext
    {
        public static int GetTowerSetIndex(this TowerModel towerModel)
        {
            foreach(var tower in Game.instance.model.towerSet)
            {
                if(tower.towerId == towerModel.baseId)
                {
                    return tower.towerIndex;
                }
            }
            return 0;
        }
    }
}