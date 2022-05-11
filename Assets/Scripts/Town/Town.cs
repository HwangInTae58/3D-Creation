using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour, IDamaged
{
    public TownData data;

    int HP;

    float costDelay = 0f;
    bool costCount = true;

   
    private void Awake()
    {
        HP = data.hp;
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
        if (0 >= HP)
        {
            Die();
        }
    }
private void Die()
    {
        Destroy(gameObject);
    }

}
