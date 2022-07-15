using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendlyBuild : MonoBehaviour
{
    public FriendlyData data;
    public Text friendName;
    public Text friendManual;
    public Text cost;
    public Button buyButton;

    private void Awake()
    {
        friendName.text = data.friendName;
        friendManual.text = data.friendManual;
    }
    private void Start()
    {
        cost.text = data.cost.ToString();
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
