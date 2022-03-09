using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BioManager
{
    public static Dictionary<GameObject, EnumManager.camp> BioPool = new Dictionary<GameObject, EnumManager.camp>();

    /// <summary>
    /// 註冊生物
    /// </summary>
    /// <param name="go">生物GameObject</param>
    /// <param name="Camp">陣營</param>
    public static void Add(GameObject go, EnumManager.camp Camp)
    {
        if (!BioPool.ContainsKey(go)) BioPool.Add(go, Camp);
    }

    /// <summary>
    /// 移除註冊生物
    /// </summary>
    /// <param name="go">生物GameObject</param>
    public static void Remove(GameObject go)
    {
        if (BioPool.ContainsKey(go)) BioPool.Remove(go);
    }

    /// <summary>
    /// 清除所有
    /// </summary>
    public static void Clear()
    {
        BioPool.Clear();
    }

    /// <summary>
    ///獲取敵方物件
    /// </summary>
    /// <param name="enemyCamp">要獲取的敵方陣營</param>
    public static List<GameObject> GetAllEnemyCamp(EnumManager.camp enemyCamp)
    {
        var enemys = new List<GameObject>();
        foreach (var go in BioPool)
        {
            if (go.Value == enemyCamp) enemys.Add(go.Key);
        }
        return enemys;
    }

    /// <summary>
    /// 獲取與自己最近的敵人和距離
    /// </summary>
    /// <param name="go">自己</param>
    /// <param name="enemyCamp">敵方陣營</param>
    /// <param name="enemy">回傳敵方物件</param>
    /// <param name="enemydistance">回傳敵方最短距離</param>
    public static void GetShortDistanceAndGameObject(GameObject go, EnumManager.camp enemyCamp, ref GameObject enemy, ref float shortdistance)
    {
        var enemys = GetAllEnemyCamp(enemyCamp);
        bool first = true;
        foreach (var e in enemys)
        {
            var distance = Vector3.Distance(go.transform.position, e.transform.position);
            if (first)
            {
                first = false;
                shortdistance = distance;
                enemy = e;           
            }
            else if(distance < shortdistance)
            {
                shortdistance = distance;
                enemy = e;
            }
        }
    }
}
