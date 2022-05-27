﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour, IDamaged
{
    public TownData data;
    Animator anim;
    Collider coll;
    int MaxHP;
    int HP;
    bool isLowHP = false;
    bool isBroken =false;

    float costDelay = 0f;
    bool costCount = true;

    int Life = 1;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        MaxHP = data.hp;
        HP = data.hp;
    }
    private void Start()
    {
        GetMaxCost();
    }
    private void Update()
    {
        GetDelay();
        OnCost();
    }
    public void OnCost()
    {
        if (!costCount)
        {
            // TODO : 빌드매니저를 만들어서 코스트 관리하기 
            BuildManager.instance.GetCost(data.getCost);
            costCount = true;
        }
    }
    public void GetMaxCost()
    {
        BuildManager.instance.plusMaxCost(data.maxCostUp);
    }
   
    private void GetDelay()
    {
        if (costCount)
        {
            costDelay += Time.deltaTime;
            if(costDelay >= data.getDelay)
            {
                costCount = false;
                costDelay = 0f;
            }
        }
    }

    public void Damaged(int attck)
    {
        HP -= attck;
        if(HP < (MaxHP / 3))
        {
            isLowHP = true;
            anim.SetBool("IsLowHP", isLowHP);
        }
        if (0 >= HP)
        {
            isBroken = true;
            coll.enabled = false;
            Die();
        }
    }
private void Die()
    {
        anim.SetBool("IsBroken", isBroken);
        Destroy(gameObject , 0.6f);
        Life -= data.Life;
        BuildManager.instance.MinusMaxCost(data.maxCostUp);
        if(Life <= 0)
        {
            //Gamemanager.instuns.GameOver();
        }
    }
}

