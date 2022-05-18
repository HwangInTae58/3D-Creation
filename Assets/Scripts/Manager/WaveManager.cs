using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static WaveManager _instance;
    public static WaveManager instance { get { return _instance; } }

    public GameObject[] monsterPotal;
    public Enemy[] enemies;

    public float waveTime = 10f;
    public bool waveStart = false;

    public int monsterCount = 0;
    int monCount;
    public int monsterMaxCount = 10;
    public float monsterDelay = 0f;
    bool monsterSpawn = false;
    int mons = 0;

    int stage = 0;
    int hard = 0;

    int spawnOke = 1000;
    int spawnBadWizard = 1000;
    int spawnGolem = 1000;
    //int spawnDragon = 1000;


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
        ranTime = Random.Range(0.8f, 2f);
        int randomPotal;
        int randomEnemy;

        yield return new WaitForSeconds(1f);
        while (monsterCount < monsterMaxCount && !monsterSpawn)
        {
            randomPotal = Random.Range(0, monsterPotal.Length);
            randomEnemy = Random.Range(0, enemies.Length);

            Instantiate(enemies[RandomMonster()].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);

            monsterCount++;
            monCount++;

            yield return new WaitForSeconds(ranTime);
        }
        mons = 0;
    }
    private int RandomMonster()
    {
        int random = Random.Range(0, 1000);

        int slime = 0;

        int oke = 1;

        int badWizard = 2;

        int golem = 3;

        //int dragon;
    
        Debug.Log(random);
        if(random > spawnGolem && spawnBadWizard < spawnGolem)
            return golem;
        if (random > spawnBadWizard && spawnOke <spawnBadWizard)
            return badWizard;
        if (random > spawnOke)
            return oke;
        
        return slime;
    }
    private void DifficultyUP()
    {
        hard++;
        if(hard >= 2)
        {
            //TODO : 강한몬스터 확률로 더 높은 확률로 나오게 만들기
            if (spawnOke >= 0)
                spawnOke -= 200;

            if (spawnBadWizard >= 400)
                spawnBadWizard -= 80;

            if (spawnGolem >= 700)
                spawnGolem -= 20;

            if(hard == 5)
            {
                monsterMaxCount += 20;
                if (spawnOke >= 0)
                    spawnOke -= 200;
                if (spawnBadWizard >= 400) 
                    spawnBadWizard -= 100;
                hard = 0;
            }
           
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
            waveTime = 15f;
            stage++;
            DifficultyUP();
            UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
            UIManager.instance.stageNumber.text = stage.ToString();
            waveStart = true;
            monsterSpawn = false;
        }

            
    }
}
