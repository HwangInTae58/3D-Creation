using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Option : MonoBehaviour
{
    static Option _instance;
    public static Option instance { get { return _instance; } }
    public AudioMixer audioMixer;
    public Slider slider;
    public float volume;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
    }
    private void Update()
    {
        SetVolume();
    }
    public void OptionWindowCreat()
    {
        slider.value = volume;
        gameObject.SetActive(false);
    }
    public void ActiveOptionWindow()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    public void SetVolume()
    {
        volume = slider.value;
        if (volume == -40f)
            audioMixer.SetFloat("Master", -80);
        else
            audioMixer.SetFloat("Master", volume);

        PlayerPrefs.SetFloat("Volume", volume);
    }
}
