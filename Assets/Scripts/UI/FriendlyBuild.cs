using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyBuild : MonoBehaviour
{
    public FriendlyData data;

    public Text friendName;
    public Text cost;
    public Button buyButton;

    private void Awake()
    {
        friendName.text = data.friendName;
        cost.text = data.cost.ToString();
    }
    private void OnEnable()
    {
        
    }
    public void OnClick()
    {
        if (BuildManager.instance.selectedTown == null)
            BuildManager.instance.selectedFriendly = data.prefab;
        else
        {
            BuildManager.instance.selectedTown = null;
            BuildManager.instance.selectedFriendly = data.prefab;
        }
        
    }
}
