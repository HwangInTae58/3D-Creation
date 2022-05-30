using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    static WaveManager _instance;
    public static WaveManager instance { get { return _instance; } }

    public GameObject[] monsterPotal;
    public GameObject bossPotal;
    public Enemy[] enemies;
    public Boss[] bosses;

    public float    waveTime;
    public bool     waveStart;

    public bool     boss;

    public int      monsterCount;
    int monCount;
    public int      monsterMaxCount;
    public float    monsterDelay;
    bool            monsterSpawn;
    int             mons;

    public int      stage;
    public int      dragonSpawn;
    public int      bossStage;
    int             hard;
    int             collecter;

    int             spawnOke;
    int             spawnBadWizard;
    int             spawnGolem;

    private void Awake()
    {
        if (null == _instance)
            _instance = this;

        waveTime = 10f;
        waveStart = false;
        boss = false;
        monsterCount = 0;
        monsterMaxCount = 15;
        monsterDelay = 0f;
        monsterSpawn = false;
        mons = 0;
        stage = 0;
        dragonSpawn = 0;
        bossStage = 0;
        hard = 0;
        collecter = 1;
        spawnOke = 1000;
        spawnBadWizard = 1000;
        spawnGolem = 1000;
    }

    private void Start()
    {
        //int random = Random.Range(0, enemies.Length);
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
            waveStart = false;
    }
    IEnumerator OnMonster()
    {
        float ranTime;
        ranTime = Random.Range(0.2f, 0.5f);
        int randomPotal;
        
        yield return new WaitForSeconds(1f);
        while (monsterCount < monsterMaxCount && !monsterSpawn)
        {
            randomPotal = Random.Range(0, monsterPotal.Length);
            Instantiate(enemies[RandomMonster()].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
            monsterCount++;
            monCount++;
            yield return new WaitForSeconds(ranTime);
        }
        if (dragonSpawn == 8)
        {
            randomPotal = Random.Range(0, monsterPotal.Length);
            Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
            dragonSpawn = 7;
            monsterCount++;
            monCount++;
            switch (stage) {
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    for (int i = 0; i <= 1; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                    for (int i = 0; i <= 2; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                    for (int i = 0; i <= 5; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
            }
        }
        yield return new WaitForSeconds(0.4f);
        if (bossStage == 10)
        {
            UIManager.instance.bossName.text = bosses[collecter].data.monsterName;
            UIManager.instance.bossAppear.SetActive(true);
            if (collecter == 4)
                collecter = 1;
            Instantiate(bosses[collecter].data.prefab, bossPotal.transform.position, Quaternion.identity);
            boss = true;
            bossStage = 3;
            monsterCount++;
            monCount++;
            collecter++;
            yield return new WaitForSeconds(1.5f);
            UIManager.instance.bossAppear.SetActive(false);
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

            if (spawnBadWizard >= 500)
                spawnBadWizard -= 80;

            if (spawnGolem >= 750)
                spawnGolem -= 20;

            if(hard == 5)
            {
                monsterMaxCount += 20;
                if (spawnOke >= 0)
                    spawnOke -= 200;
                if (spawnBadWizard >= 500) 
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
            StartCoroutine(WaveStartUI());
            waveTime = 10f;
            stage++;
            bossStage++;
            dragonSpawn++;
            DifficultyUP();
            UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
            UIManager.instance.stageNumber.text = stage.ToString();
            waveStart = true;
            monsterSpawn = false;
        }
    }
    IEnumerator WaveStartUI()
    {
        UIManager.instance.waveStart.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIManager.instance.waveStart.SetActive(false);
    }
}
