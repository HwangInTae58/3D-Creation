using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownBuild : MonoBehaviour
{
    public TownData data;

    public Text townName;
    public Text townManual;
    public Text cost;
    public Button buyButton;

    private void Awake()
    {
        townName.text = data.townName;
        cost.text = data.cost.ToString();
        townManual.text = data.townManual;
    }
    public void OnClick()
    {
        if(BuildManager.instance.selectedFriendly == null)
            BuildManager.instance.selectedTown = data.prefab;
        else
        {
            BuildManager.instance.selectedFriendly = null;
            BuildManager.instance.selectedTown = data.prefab;
        }
    }

}
