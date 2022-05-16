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
        changeCost += OnCost;
        changeCost?.Invoke();
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


    public void OnTownBuild(RaycastHit pos)
    {
        if (selectedTown == null)
            return;
        if (selectedTown.data.cost > Cost)
            return;
        else 
        { 
        Town town = Instantiate(selectedTown, pos.point, Quaternion.identity);
        Cost -= selectedTown.data.cost;
            changeCost?.Invoke();
        }
    }
    public void OnFriendlyBuild(RaycastHit pos)
    {
        if (selectedFriendly == null)
            return;
        if (selectedFriendly.data.cost > Cost)
            return;
        else { 
            Cost -= selectedFriendly.data.cost;
            Friendly friendly = Instantiate(selectedFriendly, pos.point, Quaternion.identity);
            friendly.originalPos = pos.point;
            changeCost?.Invoke();
        }
    }


}
