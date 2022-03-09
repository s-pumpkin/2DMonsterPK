using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchControlsKit;

public class PlayerControl2D : EnemyAI
{

    public enum state { Player, AI };
    public state State = state.AI;

    //move
    private Vector3 movePos;
    // Start is called before the first frame update
    public override void Start()
    {
        PlayerManager.Add(this.gameObject, State);
        BioManager.Add(this.gameObject,Camp);
        ID = Random.Range(0,3);
        goal = goal.GetChild(ID);
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!isDeath)
            PlayCtr();

    }


    public void PlayCtr()
    {
        if (State == state.AI)
        {
            base.Update();
        }
        else
        {
            Already = true;
            #region ANDROID
#if UNITY_ANDROID
            if (TCKInput.m_Instance != null)
            {
                Vector2 CtrPos = TCKInput.GetAxis("Joystick");
                movePos = new Vector3(CtrPos.x, 0, CtrPos.y);
            }
#endif
            #endregion

            #region Windows
#if UNITY_STANDALONE_WIN
            movePos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Debug.Log("我是從Windows的電腦上執行的");
#endif
            #endregion
            if (movePos != Vector3.zero)
            {
                Anim.SetAnim("run");
                transform.Translate(movePos * Speed * Time.deltaTime);
                Turn(movePos.x);
            }
            else
            {
                Anim.SetAnim("idle");
            }
        }
    }
    public void Turn(float Horizontal)
    {
        if (Horizontal < 0)
        {
            Anim.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Anim.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void OnDeath()
    {
        Debug.Log("player");
    }
}
