using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public int HpMax;
    public int Hp;
    public Image HpSliderBar;
    public float Speed;


    private void Awake()
    {
        Hp = transform.parent.parent.GetComponent<CreateBase>().Hp;
        HpMax = Hp;
    }

    // Update is called once per frame
    void Update()
    {
        float toFillAmoValue = FillAmountValueCompute(Hp);
        HpSliderBar.fillAmount = Mathf.Lerp(HpSliderBar.fillAmount, toFillAmoValue, Speed * Time.deltaTime);
    }

    float FillAmountValueCompute(int hp)
    {
        return hp / (float)HpMax;
    }

    public void Init()
    {

    }
}
