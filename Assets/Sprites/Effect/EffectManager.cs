using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public string EffectPath = "effect/";
    public static EffectManager instance;
    private static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                return instance = new EffectManager();
            }
            else
            {
                return instance;
            }
        }
    }
    public Dictionary<string, Queue<EffectControl>> EffectObjectsPool = new Dictionary<string, Queue<EffectControl>>();

    /// <summary>
    /// 註冊
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="name"></param>
    public void Register(int amount, string name)
    {
        for (int i = 0; i < amount; i++)
        {
            Object obj = Resources.Load(EffectPath + name);
            if (obj == null)
            {
                Debug.LogError("註冊特效路徑錯誤->" + EffectPath + name);
                return;
            }
            GameObject go = Instantiate(obj, gameObject.transform) as GameObject;
            go.transform.position = Vector3.zero;
            go.name = name;

            if (!EffectObjectsPool.ContainsKey(name))
            {
                Queue<EffectControl> newStack = new Queue<EffectControl>();
                EffectObjectsPool.Add(name, newStack);
                newStack.Enqueue(go.GetComponent<EffectControl>());
            }
            else
            {
                Queue<EffectControl> objPool = EffectObjectsPool[name];
                objPool.Enqueue(go.GetComponent<EffectControl>());
            }
            go.SetActive(false);
        }
    }
    /// <summary>
    /// 檢查是否註冊
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckisRegister(string name)
    {
        if (EffectObjectsPool.ContainsKey(name))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 取得特效，回傳如果為Null請LogError
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public EffectControl GetUseEffect(string name)
    {
        if (!CheckisRegister(name)) return null;
        var EffectPool = EffectObjectsPool[name];
        if (EffectPool.Count == 0)
        {
            Register(1, name);
        }
        var ec = EffectPool.Dequeue();
        return ec;
    }

    public void RecycleEffect(EffectControl RecycleGo)
    {
        if (!CheckisRegister(RecycleGo.gameObject.name)) return;
        var EffectPool = EffectObjectsPool[RecycleGo.gameObject.name];
        EffectPool.Enqueue(RecycleGo);
    }
}
