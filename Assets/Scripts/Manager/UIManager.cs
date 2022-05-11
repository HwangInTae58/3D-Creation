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


    private void Awake()
    {
        if (null == _instance)
            _instance = this;
    }
  
    public void OnTownBuildUI(bool town)
    {
        if(town)
        {
            townBuild.SetActive(true);
            if (friendlyBuild.activeSelf == true)
            {
                friendlyBuild.SetActive(false);
            }
            else
                return;
        }
    }
    public void OnFriendlyBuildUI(bool friend)
    {
        if (friend)
        {
            friendlyBuild.SetActive(true);
            if (townBuild.activeSelf == true)
            {
                townBuild.SetActive(false);
            }
            else
                return;
        }
    }

}
