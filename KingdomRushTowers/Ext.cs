using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity;
using UnityEngine;
namespace KingdomRushTowers
{
    public static class Ext
    {
        public static void Clear(this GameObject go)
        {
            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject);
            }
        }
        public static int GetTowerSetIndex(this TowerModel towerModel)
        {
            foreach (var tower in Game.instance.model.towerSet)
            {
                if (tower.towerId == towerModel.baseId)
                {
                    return tower.towerIndex;
                }
            }
            return 0;
        }
    }
}