using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBase : MonoBehaviour
{
    public EnumManager.camp Camp = EnumManager.camp.None;
    //Hp
    public int MaxHp;
    public int HpMagnification = 1;
    public int Hp;

    public float Speed;
    public Timer timer = new Timer();
    public bool isDeath = false;
    public PlayAnimation Anim;
    private void Awake()
    {
        Anim = gameObject.GetComponentInChildren<PlayAnimation>();
    }
    public virtual void Start()
    {
        Hp = MaxHp;
    }

    void Init()
    {
        Hp = MaxHp;
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public virtual void OnEnable()
    {
        Init();
    }

}
