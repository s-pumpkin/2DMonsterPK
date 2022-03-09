using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    public Timer SummonTime = new Timer();
    public event Action<GameObject> Summon;
    public GameObject SummonUnit;
    int Difficulty = 1, cnt = 0;
    public static bool EmenyAttack = false;
    void Start()
    {
        SummonTime.SetTimer(2f,SummonZombie);   
    }
    
    private void Update()
    {
        SummonTime.Update();
    }

    void SummonZombie()
    {
        if (cnt < 3)
        {
            for(int i = 0; i < Difficulty + 2; i++)
            {
                Instantiate(SummonUnit,this.transform.position,Quaternion.identity);
            }
            SummonTime.Reset();
            cnt += 1;
        }
        else
        {
            EmenyAttack = true;
        }
    }

}
