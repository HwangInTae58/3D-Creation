using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData.asset", menuName = "Boss/BossData")]
public class BossData : ScriptableObject
{
    public string monsterName;
    //사거리
    public float range;
    public float attackFarRange;
    public float attackCloseRange;
    public int attacker;
    //HP
    public int hp;
    //데미지
    public int damage;
    //딜 딜레이
    public float fireDelay;
    public float flameDelay;
    public float flameFire;
    //스피드
    public float speed;
    //prefab
    public Boss prefab;
    //victory
    public bool victory;
}
