using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    public GameObject soundSource;
    public AudioSource[] prefab;

    int soundNum = 0;
    int maxSoundPrefab = 100;
    private void Start()
    {
        for(int i = 0; i < maxSoundPrefab; i++) {
            prefab[i] =  Instantiate(prefab[i]);
            prefab[i].transform.SetParent(soundSource.transform, false);
            prefab[i].enabled = false;
        }
    }
    public void SetSound(AudioClip _clip, Transform _position, float audioEnd)
    {
        prefab[soundNum].enabled = true;
        prefab[soundNum].clip = _clip;
        prefab[soundNum].transform.position = _position.position;
        
        if(prefab[soundNum].time>= audioEnd)
        {
            prefab[soundNum].enabled = false;
        }

        soundNum++;
        if(soundNum >= maxSoundPrefab)
        {
            soundNum = 0;
        }
    }
}
