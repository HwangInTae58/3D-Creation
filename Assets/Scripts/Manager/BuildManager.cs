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

    public Friendly selectedFriendly;
    public Town selectedTown;
    
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
    }
    private void OnCost()
    {
        costCount.text = Cost.ToString();
    }


    public void OnTownBuild(Vector3 vec)
    {
        if (selectedTown.data.cost > Cost)
            return;
        if (selectedTown == null)
            return;
        changeCost?.Invoke();
        Cost -= selectedTown.data.cost;
        Town town = Instantiate(selectedTown, vec, Quaternion.identity);
    }
    public void OnFriendlyBuild(Vector3 vec)
    {
        if (selectedFriendly.data.cost > Cost)
            return;
        if (selectedFriendly == null)
            return;
        changeCost?.Invoke();
        Cost -= selectedFriendly.data.cost;
       Friendly friendly = Instantiate(selectedFriendly, vec, Quaternion.identity);
    }


}
