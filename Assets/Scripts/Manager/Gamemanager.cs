using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    static Gamemanager _instance;
    public static Gamemanager instance { get { return _instance; } }

    public GameObject optionWindow;
    public Slider slider;
    public AudioSource sources; // 0 병사, 1 건물 , 2 일반 적 병사 , 3 보스 , 4 플레임
    public AudioClip audioClip;
    public Text speedText;
    int speed;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        speed = 1;
    }
    private void Start()
    {
        if(null != speedText)
            speedText.text = speed.ToString();
        sources = Instantiate(sources);
        if (audioClip != null) { 
            sources.clip = audioClip;
            sources.loop = true;
            sources.Play();
        }
    }
    public void ChangeScene(string sceneName)
    {
        if (Time.timeScale <= 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    
    public void ContinueGame()
    {
            Time.timeScale = speed;
        //if (speed == 2)
        //    Time.timeScale = 2;
        if (UIManager.instance.pausdWindow.activeSelf == true) { 
            UIManager.instance.pausdWindow.SetActive(false);
            UIManager.instance.pausd = false;
        }
    }
    public void ActiveOptionWindow()
    {
        if (!optionWindow.activeSelf)
            optionWindow.SetActive(true);
        else
            optionWindow.SetActive(false);
    }
    public void SoundOption()
    {
        if (sources != null)
            return;
        sources.volume = slider.value;
        slider.value = sources.volume;
    }
    public void GameSpeed()
    {
        if (Time.timeScale < 1)
            return;
        if (Time.timeScale == 1)
        {
            speed = 2;
            Time.timeScale = speed;
        }
        else if(Time.timeScale == 2){ 
            speed = 3;
            Time.timeScale = speed;
        }
        else
        {
            speed = 1;
            Time.timeScale = 1;
        }
        speedText.text = speed.ToString();
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
