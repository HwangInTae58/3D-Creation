using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{
    public TownData townData;

    int HP;

    float costDelay = 0f;
    bool costCount = false;

    private void Awake()
    {
        HP = townData.hp;
    }
    private void FixedUpdate()
    {
        GetCost();
    }
    private void GetCost()
    {

    }
    private void GetDelay()
    {

    }

}
