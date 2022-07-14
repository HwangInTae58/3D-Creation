using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public AudioClip[] audioClip;

    protected Collider charCollider;
    protected NavMeshAgent agent;
    protected Animator anime;

    public GameObject damageEffect;
    public GameObject effect;

    public Transform effectPos;
    public Transform bloodPos;
    protected Transform enermyTargetPos;

   
    protected int HP;
    protected float attackDelay;
    protected bool attacked;

    protected float ranged;
    protected float attackranged;

    protected float moveDelay;
    protected bool move;

    protected bool isDie;
    protected void Die(float time , int clip , float soundTime, bool monCount)//사망처리
    {
        if(agent != null)
        agent.speed = 0;
        charCollider.enabled = false;
        ranged = 0;
        attackranged = 0;
        isDie = true;
        SoundPool.instance.SetSound(audioClip[clip], gameObject.transform, soundTime, false);
        if (monCount)
            WaveManager.instance.monsterCount -= 1;
        Destroy(gameObject, time);
    }
}
