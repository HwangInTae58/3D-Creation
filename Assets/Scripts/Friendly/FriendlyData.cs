using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FriendlyData.asset", menuName = "Friendly/FriendlyData")]
public class FriendlyData : ScriptableObject
{
    
    //사거리
    public float range;
    public float attackRange;
    //HP
    public int hp;
    //데미지
    public int damage;
    //딜 딜레이
    public float fireDelay;
    //스피드
    public float speed;
    //prefab
    public Friendly prefab;
    //Cost
    public int cost;
}
