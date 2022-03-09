using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerManager
{
    public static Dictionary<GameObject, PlayerControl2D.state> PlayerPool = new Dictionary<GameObject, PlayerControl2D.state>();
    /// <summary>
    /// 註冊友軍
    /// </summary>
    /// <param name="go">友軍GameObject</param>
    /// <param name="Playerc">PlayerControl2D 腳本</param>
    public static void Add(GameObject go, PlayerControl2D.state PlayerC)
    {
        if (!PlayerPool.ContainsKey(go)) PlayerPool.Add(go,PlayerC);
    }

    /// <summary>
    /// 移除友軍
    /// </summary>
    /// <param name="go">友軍GameObject</param>
    public static void Remove(GameObject go)
    {
        if (PlayerPool.ContainsKey(go)) PlayerPool.Remove(go);
    }

    ///<summary>
    ///清除所有
    ///</summary>
    public static void Clear()
    {
        PlayerPool.Clear();
    }

    /// <summary>
    /// 判斷能否控制
    /// </summary>
    /// <param name="go">射線射中 GameObject</param>
    public static void CompareState(RaycastHit go)
    {
        foreach(var P in PlayerPool)
        {
            if (P.Key.transform.position == go.transform.position)
            {
                PlayerControl2D PS = P.Key.GetComponent<PlayerControl2D>();
                if (P.Value == PlayerControl2D.state.AI)
                {
                    PS.State = PlayerControl2D.state.Player;
                }          
            }
            else
            {
                PlayerControl2D PS = P.Key.GetComponent<PlayerControl2D>();
                PS.State = PlayerControl2D.state.AI;
                PS.Anim.gameObject.transform.localScale = new Vector3(1,1,1);
                PS.Anim.SetAnim("idle");
            }
        }
    }
}
