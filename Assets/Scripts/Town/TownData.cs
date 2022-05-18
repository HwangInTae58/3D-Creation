using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TownData.asset", menuName = "Town/TownData")]
public class TownData : ScriptableObject
{
    //이름
    public string townName;
    //설명
    public string townManual;
    //HP
    public int hp;
    //cost 수급량
    public int getCost;
    public int maxCostUp;
    //수급 딜레이
    public float getDelay;
    //Cost
    public int cost;
    //prefab
    public Town prefab;
    //이게 0이면 게임오버
    public int Life;

}
