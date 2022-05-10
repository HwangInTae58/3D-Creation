using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData.asset", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    /*
     몬스터 설명
    몬스터 아이콘
    */
    
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
    public Enemy prefab;

}
