using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChoose : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Choose();
        }
    }
    void Choose()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit ChooseUnit;
        if (Physics.Raycast(ray, out ChooseUnit))
        {
            PlayerManager.CompareState(ChooseUnit);
        }
    }
}
