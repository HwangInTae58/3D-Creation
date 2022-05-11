using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildManager : MonoBehaviour
{
    static BuildManager _instance;
    public static BuildManager instance{ get { return _instance; } }
    public UnityAction changeCost;
    public Text costCount;

    Friendly friendly;
    Town town;

    public int startCost = 20;
    public int Cost;

    private void Awake()
    {
        if (null == _instance)
            _instance = this;
    }
   
    private void Start()
    {
        Cost = startCost;
        costCount.text = startCost.ToString();
        changeCost += OnCost;
    }
    public void GetCost(int getCost)
    {
        Cost += getCost;
        changeCost?.Invoke();
        Debug.Log("돈 : " + Cost);
    }
    private void OnCost()
    {
        costCount.text = Cost.ToString();
    }


    public void OnTownBuild()
    {
        if (town.data.cost > Cost)
            return;

    }
    public void OnFriendlyBuild()
    {
        if (friendly.data.cost > Cost)
            return;
    }


}
