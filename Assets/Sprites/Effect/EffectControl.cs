using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EffectInfo
{
    public bool OnshowEvent;
    public string EffectName;
    public float EffectEndTime;
    public float speed;
    public string FollowObject;
    public Vector3 Rotation;
}
public class EffectControl : MonoBehaviour
{
    public EffectInfo Info = new EffectInfo();
    public int Towards = 1;
    public GameObject AnimationGO;
    public Timer effectTime = new Timer();

    // Start is called before the first frame update
    private void Awake()
    {
        AnimationGO = gameObject.transform.Find("Animation").gameObject;
    }

    private void Update()
    {
        AnimationGO.transform.localScale = new Vector3(Towards, 1, 1);
        gameObject.transform.Translate(Info.speed * Vector3.forward* Towards);
        effectTime.Update();
    }

    void CloseEffect()
    {
        EffectManager.instance.RecycleEffect(this);
        gameObject.SetActive(false);
        Init();
    }

    void Init()
    {
        Info = new EffectInfo();
        AnimationGO = null;
    }

    public void SetPRS(GameObject go, EffectInfo Eventinfo)
    {
        Info = Eventinfo;
        effectTime.SetTimer(Info.EffectEndTime, CloseEffect, TimeState.Updata);
        Towards = (int)Mathf.Sign(AnimationGO.transform.localScale.x);
        gameObject.transform.SetPositionAndRotation(go.transform.position, Quaternion.Euler(Info.Rotation));
        gameObject.SetActive(true);
    }
}
