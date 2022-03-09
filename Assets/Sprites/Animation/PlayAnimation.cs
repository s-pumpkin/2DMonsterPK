using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public AnimEventDataBase animeventData;
    private string nowAnim;
    private Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetAnim(string newAnim)
    {
        if (!CheckAnimation(newAnim)) return;
        nowAnim = newAnim;
        animator.Play(newAnim);
    }

    public bool CheckAnimation(string newAnim)
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        //不同動畫則切換
        if (!stateinfo.IsName(newAnim)) { return true; }
        //相同判斷動畫是否播完
        if (stateinfo.IsName(newAnim) && (stateinfo.normalizedTime >= 1.0f))
        {
            return true;
        }
        return false;
    }

    public void PlayEvent(int playevent)
    {
        var animEventValue = animeventData.eventInfo.FindIndex(x => x.AnimClip.name == nowAnim);
        animeventData.UseEvent(animEventValue,playevent,this.gameObject);
    }
}
