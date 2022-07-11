using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Character, IDamaged
{
    public TownData data;
    int     MaxHP;
    float   costDelay;
    bool    isLowHP;
    bool    costCount;
    bool    Life;
    private void Awake()
    {
        Life = false;
        MaxHP = data.hp;
        HP = data.hp;
        isLowHP = false;
        costDelay = 0f;
        costCount = true;
    }
    private void Start()
    {
        anime = GetComponent<Animator>();
        charCollider = GetComponent<Collider>();
        GetMaxCost();
    }
    private void Update()
    {
        if (Life)
            StartCoroutine(OnLose());
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
            anime.SetBool("IsLowHP", isLowHP);
        }
        if (0 >= HP) {
            anime.SetBool("IsDie", true);
            Die(0.6f,0,0.4f);
        }
    }
    public IEnumerator OnLose()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.instance.GameLose();
        yield return new WaitForSecondsRealtime(3f);
        Gamemanager.instance.ChangeScene("TitleScene");
    }
}

