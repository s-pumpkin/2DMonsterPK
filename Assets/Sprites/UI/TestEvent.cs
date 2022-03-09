using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEvent : MonoBehaviour
{
    public GameObject gb;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        UIEventListener BtnListener = btn.gameObject.AddComponent<UIEventListener>();

        BtnListener.OnClick += delegate()
        {
           Instantiate(gb,new Vector3(-28,1.2f,-1),Quaternion.identity);
        };
    }
}
