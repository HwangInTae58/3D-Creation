using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    static SoundManager _instance;
    public static SoundManager instance { get { return _instance; } }

    AudioSource audiosorce;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public void AudioOption()
    {
    }
}
