using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
   public List<GameObject> Enmey;
   public float Speed = 3, Vision = 100, AttackRange = 1;
   public SpriteRenderer AISr;
   float Length, IsMonster_F = -1, Length_Min = 100000;
   int Id = 0;
   Vector3 TargetVec;
   bool CanAttack = false, IsRun = false, IsForward = false, IsMonster = false;
   Animator Anim;
   private void Start()
   {
       //如果是友軍
       if (this.gameObject.CompareTag("Player"))
       {
           Enmey = BioManager.GetAllEnemyCamp(EnumManager.camp.Enemy);
           IsMonster = false;
       }
       //不是友軍
       else if(this.gameObject.CompareTag("Enemy"))
       {
            Enmey = BioManager.GetAllEnemyCamp(EnumManager.camp.Player);
            IsMonster = true;
            IsMonster_F = 1f;
       }
       Anim = this.gameObject.GetComponent<Animator>();
   }
    private void Update()
    {
        //找最近的單位
        Length_Min = 100000;
        for (int i = 0; i < Enmey.Count; i++)          
        {
            //計算距離^2
            TargetVec = Enmey[i].transform.position - this.transform.position;
            TargetVec.y = 0;
            Length = TargetVec.sqrMagnitude;

            if (Length < Length_Min)
            {
                Length_Min = Length;
                Id = i;
            }
        }
        //計算距離^2
        TargetVec = Enmey[Id].transform.position - this.transform.position;
        TargetVec.y = 0;
        Length = TargetVec.sqrMagnitude;
        //print(TargetVec);
        //向量序列化
        TargetVec = TargetVec.normalized;
        //按下J鍵(要改)發布指令
        if (Input.GetKeyDown(KeyCode.J))    
        {
            IsForward = !IsForward;
            IsRun = !IsRun;
            //如果是怪物向左，反之向右
            AISr.flipX = IsMonster;         
            Anim.SetBool("IsRun",IsRun);
        }
        //如果在攻擊範圍內停止移動並且攻擊
        if (Length <= AttackRange)              
        {
            IsRun = false;
            CanAttack = true;
            Trun();
            Anim.SetBool("IsRun",IsRun);
            Anim.SetBool("CanAttack",CanAttack);
        }
        //如果在視野範圍內，往他移動
        else if (Length <= Vision)              
        {
            IsRun = true;
            CanAttack = false;
            Trun();
            //往目標移動
            this.transform.Translate(TargetVec * Speed * Time.deltaTime);
            Anim.SetBool("IsRun",IsRun);
            Anim.SetBool("CanAttack",CanAttack);
        }
        //如果有向前的指令，往前移動
        else if (IsForward)                     
        {
            IsRun = true;
            CanAttack = false;
            //如果是怪物向左，反之向右
            AISr.flipX = IsMonster;
            //往目標移動
            this.transform.Translate(new Vector3(-1,0,0) * Speed * Time.deltaTime * IsMonster_F);
            Anim.SetBool("IsRun",IsRun);
            Anim.SetBool("CanAttack",CanAttack);
        }
        //啥事都沒有，服從指令
        else                                   
        {
            IsRun = IsForward;
            AISr.flipX = !IsForward && IsMonster;
            CanAttack = false;
            Anim.SetBool("IsRun",IsRun);
            Anim.SetBool("CanAttack",CanAttack);
        }
    }
    void Trun()  //判斷目標位置的左右以及轉向
    {
        float angle = Vector3.Angle(Vector3.left,TargetVec);
        if (angle > 90)     //右邊
        {
            AISr.flipX = false;
        }
        else                //左邊
        {
            AISr.flipX = true;
        }
    }
}
