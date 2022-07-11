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
    public Text max;

    public Friendly selectedFriendly;
    public List<Friendly> friendlies;
    public Town selectedTown;
    public List<Town> towns;
    public Transform castlePos;

    public int startCost;
    public int Cost;
    public int maxCost;

    private void Awake()
    {
        if (null == _instance)
            _instance = this;

        startCost = 35;
        maxCost = 50;
        Cost = startCost;
    }
    private void Start()
    {
        changeCost += OnCost;
        changeCost?.Invoke();
        max.text = maxCost.ToString();
    }
    public void GetCost(int getCost)
    {
        if (Cost < maxCost)
        {
            Cost += getCost;
            changeCost?.Invoke();
        }
        else if (Cost > maxCost)
        {
            Cost = maxCost;
            changeCost?.Invoke();
        }
        else
            return;
    }
    public void plusMaxCost(int get)
    {
        if(maxCost >= 250)
            return;
        maxCost += get;
        max.text = maxCost.ToString();
    }
    public void MinusMaxCost(int minus)
    {
        maxCost -= minus;
        max.text = maxCost.ToString();
    }
    private void OnCost()
    {
        costCount.text = Cost.ToString();
    }
    public void OnTownBuild(RaycastHit pos)
    {
        if (selectedTown == null)
            return;
        if (Time.timeScale <= 0)
            return;
        if (selectedTown.data.cost > Cost)
            return;
        else 
        { 
        Town town = Instantiate(selectedTown, pos.point, Quaternion.identity);
            towns.Add(town);
        Cost -= selectedTown.data.cost;
            changeCost?.Invoke();
        }
    }
    public void OnFriendlyBuild(RaycastHit pos)
    {
        if (selectedFriendly == null)
            return;
        if (Time.timeScale <= 0)
            return;
        if (selectedFriendly.data.cost > Cost)
            return;
        else { 
            Cost -= selectedFriendly.data.cost;
            Friendly friendly = Instantiate(selectedFriendly, pos.point,Quaternion.identity);
            friendlies.Add(friendly);
            changeCost?.Invoke();
        }
    }
}
