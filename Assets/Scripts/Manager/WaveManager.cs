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
    private void Awake()
    {
        if (null == _instance)
            _instance = this;

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

        //랜덤사용을 위한 변수
        spawnOke        = 1000;
        spawnBadWizard  = 1000;
        spawnGolem      = 1000;
    }
    private void Start()
    {
        UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
    }
    private void FixedUpdate()
    {
        SpawnMonster();
        WaveWait();
        ShowMeTheMoney();
    }
    public void SpawnMonster()
    {
        if (!waveStart) // 웨이브 시간이 0초가 되어서 true가 되면
            return;
       if(mons == 0) { 
        StartCoroutine(OnMonster());//몬스터 소환
            mons++;
        }
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
            yield return new WaitForSeconds(ranTime); // 몬스터가 겹쳐서 스폰하는 것을 방지하기 위해 랜덤으로 함
        }
        if(spawnDragon)
            switch (stage) {
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
        }//일반 드래곤은 어느정도 강력하게 설정한만큼 Switch를 이용하여 생성되는 것을 직접 컨트롤함 
        yield return new WaitForSeconds(0.4f);
        if (bossStage == 5) // 보스 소환
        {
            if (collecter == 4)
                collecter = 1;
            UIManager.instance.bossName.text = bosses[collecter].data.monsterName;
            UIManager.instance.bossAppear.SetActive(true);
            Instantiate(bosses[collecter].data.prefab, bossPotal.transform.position, Quaternion.identity);
            boss = true;
            bossStage = 0;
            monsterCount++;
            monCount++;
            collecter++;
            yield return new WaitForSeconds(1.5f);
            UIManager.instance.bossAppear.SetActive(false);
        }
    }
    private int RandomMonster()
    {
        //int로 랜덤 함수를 받은후
        int random = Random.Range(0, 1000);
        int slime = 0;
        int oke = 1;
        int badWizard = 2;
        int golem = 3;
        //조건에 맞는 몬스터를 소환하는 방식으로 하였습니다.
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
        //int형 변수를 하나 둬서 웨이브마다의 난이도를 바꾸도록 하였습니다.
        hard++;
        if(hard >= 2)
        {
            //웨이브에 따른 강한 몬스터 소환될 확률 상승
            if (spawnOke > 0)
                spawnOke -= 200;

            if (spawnBadWizard > 580)
                spawnBadWizard -= 80;

            if (spawnGolem > 750)
                spawnGolem -= 20;

            if(hard == 5)
            {
                monsterMaxCount += 35; // 몬스터 수 증가
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
       //wave 시간을 줄여주며 플레이어가 초단위로 볼 수 있게 하였습니다.
        waveTime -= Time.deltaTime;
        UIManager.instance.waveWaitTime.text = Mathf.Floor(waveTime).ToString();
        //웨이브 시간이 0초로 줄어들면 스테이지가 시작되게 하였다.
        if (waveTime <= 0)
        {
            StartCoroutine(WaveStartUI());
            waveTime = 15f;
            stage++;
            bossStage++;
            DifficultyUP(); // 난이도 업
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
    private void ShowMeTheMoney()
    {
        BuildManager.instance.MoneyCheat();
    }
}
