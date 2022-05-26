using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    static Gamemanager _instance;
    public static Gamemanager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }
    public void ChangeScene(string sceneName)
    {
        if (Time.timeScale <= 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    public void ContinueGame()
    {
        if (Time.timeScale <= 0)
            Time.timeScale = 1;
        if (UIManager.instance.pausdWindow.activeSelf == true) { 
            UIManager.instance.pausdWindow.SetActive(false);
            UIManager.instance.pausd = false;
        }
    }
    public void GameOver()
    {
        //SceneManager.LoadScene(GameOverScene);
    }
    public void GameExit()
    {
        Application.Quit();
    }
}
