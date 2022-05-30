using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager instance { get { return _instance; } }

    public Button town;
    public Button friendly;

    public GameObject friendlyBuild;
    public GameObject townBuild;

    public GameObject pausdWindow;

    public Text stageNumber;
    public Text waveWaitTime;
    public GameObject waveStart;

    public GameObject bossAppear;
    public Text bossName;

    public GameObject victory;
    public GameObject lose;

    bool        curtownUI;
    bool        curFrienUI;
    public bool pausd;

    private void Awake()
    {
        if (null == _instance)
            _instance = this;

        curtownUI = false;
        curFrienUI = false;
        pausd = false;
    }
   
    public void OpenPausdWindow()
    {
        if (pausd)
        {
            pausd = false;
            Gamemanager.instance.ContinueGame();
        }
        else
        {
            pausd = true;
            pausdWindow.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void GameVictory()
    {
        victory.SetActive(true);
       Time.timeScale = 0;
    }
    public void GameLose()
    {
        lose.SetActive(true);
        Time.timeScale = 0;
    }

    public void OnBuildUI(int build)
    {
        if (Time.timeScale <= 0)
            return;
        if(build == 1 && !curtownUI)
        {
            if (friendlyBuild.activeSelf)
            {
                friendlyBuild.SetActive(false);
                if (BuildManager.instance.selectedFriendly != null)
                    BuildManager.instance.selectedFriendly = null;
            }
            townBuild.SetActive(true);
            curtownUI = true;
            if (curFrienUI)
                curFrienUI = false;
        }
        else if(build == 1 && curtownUI)
        {
            if (BuildManager.instance.selectedTown != null)
                   BuildManager.instance.selectedTown = null;
                townBuild.SetActive(false);
            curtownUI = false;
        }
        if (build == 2 && !curFrienUI)
        {
            if (townBuild.activeSelf)
            {
                townBuild.SetActive(false);
                if (BuildManager.instance.selectedTown != null)
                    BuildManager.instance.selectedTown = null;
            }
            friendlyBuild.SetActive(true);
            curFrienUI = true;
            if (curtownUI)
                curtownUI = false;
        }
        else if(build == 2 && curFrienUI)
        {
            if (BuildManager.instance.selectedFriendly != null)
                BuildManager.instance.selectedFriendly = null;
            friendlyBuild.SetActive(false);
            curFrienUI = false;
        }

    }
}
