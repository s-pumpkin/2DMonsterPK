using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

[CreateAssetMenu(fileName = "newAnimEventDataBase", menuName = "新增動畫事件", order = 1)]
public class AnimEventDataBase : ScriptableObject
{
    public AnimatorController AnimatorController;
    public List<EventsInfo> eventInfo = new List<EventsInfo>();

    public void UseEvent(int animValue, int Value, GameObject go)
    {
        EventsInfo.IndividualEvent nowEvent = eventInfo[animValue].Event[Value];
        switch (nowEvent.Objtype)
        {
            case objtype.Effect:
                var effectGO = EffectManager.instance.GetUseEffect(nowEvent.effectinfo.EffectName);
                if(effectGO == null)
                {
                    Debug.LogError(nowEvent.effectinfo.EffectName + " 無法獲得此特效");
                    Debug.Break();
                }
                effectGO.SetPRS(go, nowEvent.effectinfo);

                break;

            case objtype.Audio:

                break;
        }
    }

    public void RegisterEvent()
    {
        foreach (var eventList in eventInfo)
        {
            foreach (var evens in eventList.Event)
            {
                switch (evens.Objtype)
                {
                    case objtype.Effect:
                        EffectManager.instance.Register(5,evens.effectinfo.EffectName);
                        break;

                    case objtype.Audio:

                        break;
                }
            }
        }
    }
}
[System.Serializable]
public class EventsInfo
{
    public int AnimStateValue = 0;
    //Anim
    public AnimationClip AnimClip;
    public float AnimTimeLength;
    public List<IndividualEvent> Event = new List<IndividualEvent>();
    [HideInInspector]
    public List<AnimationEvent> AnimEvent = new List<AnimationEvent>();
    //Editor
    public bool OnshowEvent = false;
    [System.Serializable]
    public class IndividualEvent
    {
        public bool OnshowEvent = false;

        public float AnimTimeline;

        public objtype Objtype;
        //effect
        public EffectInfo effectinfo;
    }
}
public enum objtype
{
    None,
    Effect,
    Audio
};

