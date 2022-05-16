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

    public Text stageNumber;
    public Text monsterCount;
    public Text waveWaitTime;

    bool curtownUI = false;
    bool curFrienUI = false;

    private void Awake()
    {
        if (null == _instance)
            _instance = this;
    }
  
    public void OnBuildUI(int build)
    {
        if(build == 1 && !curtownUI)
        {
            curtownUI = true;
            townBuild.SetActive(true);
            if (friendlyBuild.activeSelf == true)
            {
                friendlyBuild.SetActive(false);
                if (BuildManager.instance.selectedFriendly != null)
                {
                    BuildManager.instance.selectedFriendly = null;
                }
            }
           
            else
                return;
        }
        else if(build == 1 && curtownUI)
        {
            curtownUI = false;
            if (townBuild.activeSelf == true)
            {
                if(BuildManager.instance.selectedTown != null) 
                { 
                    BuildManager.instance.selectedTown = null;
                }
                townBuild.SetActive(false);
            }
        }
        if (build == 2 && !curFrienUI)
        {
            curFrienUI = true;
            friendlyBuild.SetActive(true);
            if (townBuild.activeSelf == true)
            {
                townBuild.SetActive(false);
                if (BuildManager.instance.selectedTown != null)
                {
                    BuildManager.instance.selectedTown = null;
                }
            }
            else
                return;
        }
        else if(build == 2 && curFrienUI)
        {
            curFrienUI = false;
            if (friendlyBuild.activeSelf == true)
            {
                if (BuildManager.instance.selectedFriendly != null)
                {
                    BuildManager.instance.selectedFriendly = null;
                }
                friendlyBuild.SetActive(false);
            }
        }

    }
}
