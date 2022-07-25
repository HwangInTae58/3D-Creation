using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    static SoundPool _instance;
    public static SoundPool instance { get { return _instance; } }
    public GameObject prefab;
    AudioSource[] pool;
    int plus = 0;
    int maxSoundPrefab = 50;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
       
    }
    private void Start()
    {
        pool = new AudioSource[maxSoundPrefab];
        //Start지점에서 오브젝트풀을 생성
       PoolCreat();
    }
    private void PoolCreat()
    {
        for (int i = 0; i < maxSoundPrefab; i++)
        {
            //List로 pool에 Soundprefab을 저장
            pool[plus] = CreatObject();
            plus++;
        }
        plus = 0;
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
    public void SetSound(AudioClip _clip, Transform _position, float _time , bool loop, bool sound3d)
    {
        if (plus == maxSoundPrefab) { 
            plus = 0;
            SetSound(_clip, _position, _time, loop, sound3d);
        }
        if (!loop)
            StartCoroutine(ReturnSoundPool(pool[plus], _time));
        pool[plus].gameObject.SetActive(true);
        //pool[i].transform.SetParent(null);
        pool[plus].clip = _clip;
        pool[plus].transform.position = _position.position;
        pool[plus].loop = loop;
        if (sound3d)
            pool[plus].spatialBlend = 1;
        else
            pool[plus].spatialBlend = 0;
        
            pool[plus].PlayOneShot(_clip);
        plus++;
                //코루틴을 돌려 사용이 끝나면 다시 돌려준다.
                
       //전부 사용중이라면 새로 만들어주고 다시 재귀해준다.
    }
    public IEnumerator ReturnSoundPool(AudioSource source, float _time)
    {
        yield return new WaitForSeconds(_time);
        source.Stop();
        source.gameObject.SetActive(false);
        source.transform.SetParent(instance.transform);
    }
}
