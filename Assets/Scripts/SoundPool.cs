using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    static SoundPool _instance;
    public static SoundPool instance { get { return _instance; } }
    public GameObject prefab;
    List<AudioSource> pool;
    int maxSoundPrefab = 50;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
       
    }
    private void Start()
    {
        pool = new List<AudioSource>();
        //Start지점에서 오브젝트풀을 생성
       PoolCreat();
    }
    private void PoolCreat()
    {
        for (int i = 0; i < maxSoundPrefab; i++)
        {
            //List로 pool에 Soundprefab을 저장
            pool.Add(CreatObject());
        }
    }
    private AudioSource CreatObject()
    {
        //오브젝트풀링 기법으로 사용하기 위해 생성즉시 gameObject를 false해줌
        var obj = Instantiate(prefab).GetComponent<AudioSource>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    //싱글톤으로 클래스를 만들었기에 어디서든 자신이 원하는 클립과 위치를 정해주고 플레이 타임을 정해주는 식으로 만듬
    public void SetSound(AudioClip _clip, Transform _position, float _time)
    {
        //for문을 돌려 사용중이면 넘어가고 아니라면 사용해준다.
       for(int i = 0; i <pool.Count; i++) {
            if (pool[i].gameObject.activeSelf)
                continue;
            else { 
                pool[i].gameObject.SetActive(true);
                //pool[i].transform.SetParent(null);
                pool[i].clip = _clip;
                pool[i].transform.position = _position.position;
                pool[i].Play();
                //코루틴을 돌려 사용이 끝나면 다시 돌려준다.
                StartCoroutine(ReturnSoundPool(pool[i], _time));
                return;
            }
        }
       //전부 사용중이라면 새로 만들어주고 다시 재귀해준다.
        pool.Add(CreatObject());
        SetSound(_clip, _position, _time);
    }
    public IEnumerator ReturnSoundPool(AudioSource source, float _time)
    {
        yield return new WaitForSeconds(_time);
        source.Stop();
        source.gameObject.SetActive(false);
        source.transform.SetParent(instance.transform);
    }
}
