using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetCompoment : MonoBehaviour
{   
    public GameObject Actor;
    public void SpwanActor()
    {
        Instantiate(Actor,new Vector3(-29,0,-0),new Quaternion(0,0,0,0));
        //AIMove.Enmey = GameObject.FindGameObjectsWithTag("Player");
    }
}
