using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    static MouseManager _instance;
    public static MouseManager instance { get { return _instance; } }
    private void Awake()
    {
        if (null == _instance)
            _instance = this;
    }
    private void OnMouseDown()
    {
        UIManager.instance.friendly.onClick.AddListener(() => { Debug.Log(1); });
        UIManager.instance.OnTownBuildUI(true);
        UIManager.instance.OnFriendlyBuildUI(true);
    }
}
