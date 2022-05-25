using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameData.asset", menuName = "Flame/FlameData")]
public class FlameData : ScriptableObject
{
    public int damage;

    public Flame prefab;

    public float speed;

    public float atRange;
    public float atExtent;
}
