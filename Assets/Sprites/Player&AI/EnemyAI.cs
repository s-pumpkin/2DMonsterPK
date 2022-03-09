using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : CreateBase
{
   public float speed = 3, Vision = 5, AttackRange = 2;
   public Transform goal;
   public int ID = 0;
   float Length, IsMonster_F = 1f;
   GameObject ShortEnemy;
   Vector3 TargetVec;
   public bool Already = false, IsForward = false;
    public override void Start()
    {
        ID = Random.Range(0,3);
        goal = goal.GetChild(ID);
        BioManager.Add(this.gameObject, Camp);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Camp == EnumManager.camp.Player)
        {
            //找最近的單位
            BioManager.GetShortDistanceAndGameObject(this.gameObject, EnumManager.camp.Enemy, ref ShortEnemy, ref Length);
            //按下J鍵(要改)向前
            if (Input.GetKeyDown(KeyCode.J))    
            {
                IsForward = true;
                Already = true;   
            }
        }
        else if (Camp == EnumManager.camp.Enemy)
        {
            IsMonster_F = -1f;
            //找最近的單位
            BioManager.GetShortDistanceAndGameObject(this.gameObject, EnumManager.camp.Player, ref ShortEnemy, ref Length);
            IsForward = Level.EmenyAttack;
        }
        //計算前進方向
        TargetVec = ForwardDirection();

        //如果是怪物向左，反之向右    
        Anim.gameObject.transform.localScale = new Vector3(IsMonster_F, 1, 1); 

        //在到達定位前，不能停止
        if (Already)
        {   
            //按下K鍵(要改)停止
            if (Input.GetKeyDown(KeyCode.K))    
            {
                IsForward = false;             
                Anim.SetAnim("idle");
            }
            //如果在攻擊範圍內停止移動並且攻擊
            if (Length <= AttackRange)              
            {
                Trun();
                Anim.SetAnim("attack");
            }
            //如果在視野範圍內，往他移動
            else if (Length <= Vision)              
            {
                Trun();
                //往目標移動
                this.transform.Translate(TargetVec * speed * Time.deltaTime);
                Anim.SetAnim("run");
            }
            //如果有向前的指令，往前移動
            else if (IsForward)                     
            {
                //如果是怪物向左，反之向右
                Anim.gameObject.transform.localScale = new Vector3(IsMonster_F, 1, 1);        
                //往目標移動
                this.transform.Translate(new Vector3(-1,0,0) * speed * Time.deltaTime * IsMonster_F * -1f);
                Anim.SetAnim("run");
            } 
        }
        else
        {
            Anim.SetAnim("run");
            MoveToAims(goal);
        }  
    }
    
    //判斷目標位置的左右以及轉向
    void Trun()  
    {
        float angle = Vector3.Angle(Vector3.left,TargetVec);
        //右邊
        if (angle > 90)     
        {
            Anim.gameObject.transform.localScale = new Vector3(1, 1, 1);        
        }
        //左邊
        else               
        {
            Anim.gameObject.transform.localScale = new Vector3(-1, 1, 1);        
        }
    }
    //計算前進方向
    Vector3 ForwardDirection()
    {
        Vector3 Forward;
        Forward = ShortEnemy.transform.position - this.transform.position;
        Forward.y = 0;
        Forward = Forward.normalized;
        return Forward; 
    }
    void MoveToAims(Transform goal)
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, goal.position, 0.01f);
        if (this.transform.position == goal.position)
        {
            Already = true;
            Anim.SetAnim("idle");
        }
    }
    public virtual void useMethodModes()
    {

    }
}
