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
       PoolCreat();
    }
    private void PoolCreat()
    {
        for (int i = 0; i < maxSoundPrefab; i++)
        {
            pool.Add(CreatObject());
        }
    }
    private AudioSource CreatObject()
    {
        var obj = Instantiate(prefab).GetComponent<AudioSource>();
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        return obj;
    }
    public void SetSound(AudioClip _clip, Transform _position, float _time)
    {
       for(int i = 0; i <pool.Count; i++) {
            if (pool[i].gameObject.activeSelf)
                continue;
            else { 
        pool[i].gameObject.SetActive(true);
        //pool[i].transform.SetParent(null);
        pool[i].clip = _clip;
        pool[i].transform.position = _position.position;
        pool[i].Play();
        StartCoroutine(ReturnSoundPool(pool[i], _time));
                return;
            }
        }
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
/*
 1. 풀링 리스트에 담음
 2. for문안에 if(만약 풀링리스트[i]가 setActive(true)면 continue
    아니면 풀링리스트[i]를 setActive(true)로)
3. 끝나면 풀링리스트[i]를 setActive(false)로
4.만약 전부 다 쓰고있으면 풀링리스트.Add(풀링할거)그 다음 다시 실행
 */