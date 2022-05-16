using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static WaveManager _instance;
    public static WaveManager instance { get { return _instance; } }

   

    public GameObject[] monsterPotal;

    public Enemy[] enemies;

    public float waveTime = 5f;
    bool waveStart = false;

    public int monsterCount = 0;
    int monCount;
    public int monsterMaxCount = 10;
    public float monsterDelay = 0f;
    bool monsterSpawn = false;
    int mons = 0;

    int stage = 0;
    int hard = 0;
    

    private void Awake()
    {
        if (null == _instance)
            _instance = this;
    }
    private void Start()
    {
        //int random = Random.Range(0, enemies.Length);
        UIManager.instance.monsterCount.text = monsterCount.ToString();
        UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();

        
    }
    private void FixedUpdate()
    {
        SpawnMonster();
        WaveWait();
    }
    public void SpawnMonster()
    {
        if (!waveStart)
            return;
        UIManager.instance.monsterCount.text = monsterCount.ToString();
       if(mons == 0) { 
        StartCoroutine(OnMonster());
            mons++;
        }


        //TODO : 몬스터 소환

        if (monCount == monsterMaxCount) { 
                monsterSpawn = true;
            monCount = 0;
        }

        if (monsterCount == 0 && monsterSpawn)
        {
            waveStart = false;
        }
    }
    IEnumerator OnMonster()
    {
        float ranTime;
        ranTime = Random.Range(0.1f, 1f);
        int i = 0;
        yield return new WaitForSeconds(1f);
        while (monsterCount < monsterMaxCount && !monsterSpawn)
        {
                i = Random.Range(0,monsterPotal.Length);
            Instantiate(enemies[0].data.prefab, monsterPotal[i].transform.position, Quaternion.identity);
                monsterCount++;
            monCount++;
                Debug.Log("몬스터 소환");
                yield return new WaitForSeconds(ranTime);
        }
        mons = 0;
    }
    private void DifficultyUP()
    {
        hard++;
        if(hard == 5)
        {
            //TODO : 강한몬스터 확률로 더 높은 확률로 나오게 만들기
            monsterMaxCount += 5;
            hard = 0;
        }
    }
    public void WaveWait()
    {
        if (waveStart)
            return;
       
        waveTime -= Time.deltaTime;
        UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();

        if (waveTime <= 0)
        {
            Debug.Log("Stagestart");
            waveTime = 20f;
            stage++;
            DifficultyUP();
            UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
            UIManager.instance.stageNumber.text = stage.ToString();
            waveStart = true;
            monsterSpawn = false;
        }

            
    }
}
