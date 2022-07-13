using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character, IDamaged
{
    public EnemyData data;

    private void Awake()
    {
        HP = data.hp;
        ranged = data.range;
        attackranged = data.attackRange;
       
        attackDelay = data.fireDelay;
        attacked = true;
        isDie = false;
        moveDelay = 0f;
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        charCollider = GetComponent<Collider>();
        anime = GetComponent<Animator>();
        agent.speed = data.speed;
    }
    private void Update()
    {
        FindTarget();
    }
    private void FindTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly", "Town"));
        Collider[] attackedTarget = Physics.OverlapSphere(transform.position, attackranged, LayerMask.GetMask("Friendly", "Town"));
        anime.SetFloat("IsSpeed", agent.speed);
        if (isDie)
            return;
        if (Time.timeScale <= 0)
            return;
        if (findTarget.Length <= 0)
            return;
        else
        {
            float min = int.MaxValue;
            int index = 0;
            for (int i = 0; i < findTarget.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, findTarget[i].transform.position);
                if (min > distance)
                {
                    index = i;
                    min = distance;
                }
            }

            //바라보게 하기 코드
            agent.SetDestination(findTarget[index].transform.position);

            if (attackedTarget.Length > 0)
            {

                agent.speed = 0;
                Attack(attackedTarget);
                FireDelay();
                Vector3 dird = findTarget[0].transform.position - transform.position;
                Quaternion qq = Quaternion.LookRotation(dird.normalized * Time.deltaTime);
                agent.transform.rotation = qq;
            }
            else
            {
                move = true;
                MoveDelay();
                if (moveDelay >= 3f && move) {
                    agent.speed = data.speed;
                    moveDelay = 0;
                    move = false;
                    return;
                }
            }

        }
    }
    private void MoveDelay()
    {
        if(move)
        {
            moveDelay += Time.deltaTime;
        }
    }
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;
        if (attacked)
            return;
        if (target[0] == null)
            return;
        
        anime.SetTrigger("IsAttack");
        enermyTargetPos = target[0].transform;
        IDamaged damaged = target[0].GetComponent<IDamaged>();
        attacked = true;
        damaged?.Damaged(data.damage);
    }
    public void Damaged(int attck)
    {
        HP -= attck;
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            anime.SetTrigger("IsDie");
            Die(1.3f, 1, 0.8f, true);
        }
    }
    private void HitEffact()
    {
        GameObject saveEffact;

        //audiosource.clip = audioClip[0];
        //audiosource.Play();
        SoundPool.instance.SetSound(audioClip[0], gameObject.transform, 0.5f);
        if (effectPos == null) {
            if (enermyTargetPos == null)
                return;
            saveEffact = Instantiate(effect, enermyTargetPos.position, Quaternion.identity);
        }
        else
            saveEffact = Instantiate(effect, effectPos.position, Quaternion.identity);
        Destroy(saveEffact, 1f);
    }
    private void DamageEffect()
    {
        if (null == damageEffect)
            return;
       GameObject saveEffact = Instantiate(damageEffect, bloodPos.position, Quaternion.identity);
       Destroy(saveEffact, 0.8f);
    }

    private void FireDelay()
    {
        if (attacked)
        {
            attackDelay += Time.deltaTime;
            if (attackDelay >= data.fireDelay)
            {
                attacked = false;
                attackDelay = 0f;
            }
        }
    }
}
