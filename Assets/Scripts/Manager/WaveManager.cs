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
    int             monCount;
    public int      monsterMaxCount;
    public float    monsterDelay;
    bool            monsterSpawn;
    int             mons;

    public int      stage;
    public int      bossStage;
    int             hard;
    public int      collecter;

    public int             spawnOke;
    public int             spawnBadWizard;
    public int             spawnGolem;
    public bool            spawnDragon;

    bool            save;
    private void Awake()
    {
        if (null == _instance)
            _instance = this;

        save = true;
        waveTime = 10f;
        waveStart = false;
        boss = false;
        monsterCount = 0;
        monsterMaxCount = 20;
        monsterDelay = 0f;
        monsterSpawn = false;
        mons = 0;
        stage = 0;
        bossStage = 0;
        hard = 0;
        collecter = 1;
        spawnOke = 1000;
        spawnBadWizard = 1000;
        spawnGolem = 1000;
    }
    private void Start()
    {
        
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
        if (stage >= 10)
            spawnDragon = true;
        yield return new WaitForSeconds(1f);
        while (monsterCount < monsterMaxCount && !monsterSpawn)
        {
            randomPotal = Random.Range(0, monsterPotal.Length);
            Instantiate(enemies[RandomMonster()].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
            monsterCount++;
            monCount++;
            yield return new WaitForSeconds(ranTime);
        }
        if(spawnDragon)
            switch (stage) {
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    for (int i = 0; i <= 2; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    spawnDragon = false;
                    break;
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                    for (int i = 0; i <= 4; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    spawnDragon = false;
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                    for (int i = 0; i <= 7; i++)
                    {
                        yield return new WaitForSeconds(0.5f);
                        randomPotal = Random.Range(0, monsterPotal.Length);
                        Instantiate(bosses[0].data.prefab, monsterPotal[randomPotal].transform.position, Quaternion.identity);
                        monsterCount++;
                        monCount++;
                    }
                    spawnDragon = false;
                    break;
        }
        yield return new WaitForSeconds(0.4f);
        if (bossStage == 5)
        {
            if (collecter == 4)
                collecter = 1;
            UIManager.instance.bossName.text = bosses[collecter].data.monsterName;
            UIManager.instance.bossAppear.SetActive(true);
            Instantiate(bosses[collecter].data.prefab, bossPotal.transform.position, Quaternion.identity);
            boss = true;
            bossStage = 1;
            monsterCount++;
            monCount++;
            collecter++;
            yield return new WaitForSeconds(1.5f);
            UIManager.instance.bossAppear.SetActive(false);
        }
       
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
            if (spawnOke > 0)
                spawnOke -= 200;

            if (spawnBadWizard > 580)
                spawnBadWizard -= 80;

            if (spawnGolem > 750)
                spawnGolem -= 20;

            if(hard == 5)
            {
                monsterMaxCount += 35;
                if (spawnOke > 0)
                    spawnOke -= 200;
                if (spawnBadWizard > 580) 
                    spawnBadWizard -= 100;
                hard = 0;
            }
        }
    }
    public void WaveWait()
    {
        if (waveStart)
            return;
        mons = 0;
        if (!save) { 
            int gold = BuildManager.instance.Cost;
            SaveData saveData = new SaveData(stage, gold, BuildManager.instance.towns, BuildManager.instance.friendlies, monsterMaxCount, bossStage, collecter, spawnOke, spawnBadWizard, spawnGolem);
            SaveLoad.Save(saveData, "save_001");
            save = true;
        }
        waveTime -= Time.deltaTime;
        UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
        if (waveTime <= 0)
        {
            StartCoroutine(WaveStartUI());
            waveTime = 15f;
            stage++;
            bossStage++;
            DifficultyUP();
            UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
            UIManager.instance.stageNumber.text = stage.ToString();
            waveStart = true;
            monsterSpawn = false;
            save = false;
        }
    }
    IEnumerator WaveStartUI()
    {
        UIManager.instance.waveStart.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIManager.instance.waveStart.SetActive(false);
    }
    public void LoadGame()
    {
        SaveData loadData = SaveLoad.Load("save_001");
        Debug.Log(loadData.stage);
        Debug.Log(loadData.saveGold);
        Debug.Log(loadData.bossStage);
        Debug.Log(loadData.maxCount);
        Debug.Log(loadData.collecter);
        Debug.Log(loadData.Oke);
       
        stage = loadData.stage;
        BuildManager.instance.Cost = loadData.saveGold;
        bossStage = loadData.bossStage;
        monsterMaxCount = loadData.maxCount;
        collecter = loadData.collecter;
        spawnOke = loadData.Oke;
        spawnBadWizard = loadData.wizard;
        spawnGolem = loadData.Golem;
        for (int i = 0; i <= loadData.townPrefab.Count; i++)
        {
            if (null != loadData.townPrefab[i])
                Instantiate(loadData.townPrefab[i], loadData.townPrefab[i].transform.position, loadData.townPrefab[i].transform.rotation);
        }
        for (int j = 0; j < loadData.friendPrefab.Count; j++)
        {
            if (null != loadData.friendPrefab[j])
                Instantiate(loadData.friendPrefab[j], loadData.friendPrefab[j].transform.position, loadData.friendPrefab[j].transform.rotation);
        }
        Gamemanager.instance.load = false;
        Debug.Log("불러오기");
    }
}
