using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    static WaveManager _instance;
    public static WaveManager instance { get { return _instance; } }

    public GameObject[] monsterPotal;
    public GameObject bossPotal;
    public Enemy[] enemies;
    public Boss[] bosses;

    public float waveTime = 10f;
    public bool waveStart = false;

    public int monsterCount = 0;
    int monCount;
    public int monsterMaxCount = 10;
    public float monsterDelay = 0f;
    bool monsterSpawn = false;
    int mons = 0;

    public int stage = 0;
    public int dragonSpawn = 0;
    public int bossStage = 0;
    int hard = 0;
    int collecter = 1;

    int spawnOke = 1000;
    int spawnBadWizard = 1000;
    int spawnGolem = 1000;

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
        ranTime = Random.Range(0.3f, 1f);
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
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                    for (int i = 0; i <= 1; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    for (int i = 0; i <= 2; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                    for (int i = 0; i <= 3; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    break;
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
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
        if (bossStage == 20)
        {
           
            if (collecter == 3)
            {
                collecter = 1;
            }
            Instantiate(bosses[collecter].data.prefab, bossPotal.transform.position, Quaternion.identity);
            bossStage = 10;
            monsterCount++;
            monCount++;
            collecter++;
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
            bossStage++;
            dragonSpawn++;
            DifficultyUP();
            UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
            UIManager.instance.stageNumber.text = stage.ToString();
            waveStart = true;
            monsterSpawn = false;
        }
    }
}
