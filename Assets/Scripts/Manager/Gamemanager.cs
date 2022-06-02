using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    static Gamemanager _instance;
    public static Gamemanager instance { get { return _instance; } }

    public Text speedText;
    int speed;
    public bool load;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        load = false;
        speed = 1;
    }
    private void Start()
    {
        if(null != speedText)
            speedText.text = speed.ToString();
    }
    public void ChangeScene(string sceneName)
    {
        if (Time.timeScale <= 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void Load()
    {
        ChangeScene("GameScene");
        load = true;
        WaveManager.instance.LoadGame();
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
    public void GameSpeed()
    {
        if (Time.timeScale < 1)
            return;
        if (Time.timeScale == 1)
        {
            speed = 2;
            Time.timeScale = speed;
        }
        else { 
            speed = 1;
            Time.timeScale = speed;
        }
        speedText.text = speed.ToString();
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
