using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TownData.asset", menuName = "Town/TownData")]
public class TownData : ScriptableObject
{
    //HP
    public int hp;
    //cost 수급량
    public int getCost;
    //수급 딜레이
    public float getDelay;
}
