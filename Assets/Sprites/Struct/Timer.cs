using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimeState { Updata, FixedUpdate };
public class Timer
{
    
    public TimeState timeState;
    static float TimeEndSec;
    public float time;
    public static bool isTimeEnd = false;
    public System.Action timerCallMethod;


    /// <summary>
    /// <param name="time">time-結束時間</param>
    /// <param name="Event">Event-委託事件</param>
    /// <param name="state">state-模式Updata, FixedUpdate</param>
    /// </summary>
    public void SetTimer(float time, System.Action Event = null, TimeState state = TimeState.Updata)
    {
        TimeEndSec = time;
        timerCallMethod = Event;
        timeState = state;
    }

    public void Update()
    {
        if (isTimeEnd)
            return;

        if (time < TimeEndSec)
            CheckTime();

        if (time >= TimeEndSec)
            End();
    }

    void End()
    {
        isTimeEnd = true;
        if (timerCallMethod != null)
        {
            timerCallMethod.Invoke();
        }
    }
    void CheckTime()
    {
        if (timeState == TimeState.Updata) time += Time.deltaTime;
        if (timeState == TimeState.FixedUpdate) time += Time.fixedDeltaTime;
    }

    /// <summary>
    /// 不重製EventTarget
    /// </summary>
    public void Reset()
    {
        isTimeEnd = false;
        time = 0;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        timerCallMethod = null;
        isTimeEnd = false;
        time = 0;
    }

}
