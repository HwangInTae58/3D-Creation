using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamaged
{
    public EnemyData data;

    Animator anime;
    Collider enemyCollider;
    Transform enermyTargetPos;

    public GameObject effect;
    public Transform effectPos;

    public GameObject damageEffect;
    public Transform bloodPos;

    int HP;
    float Speed;

    float ranged;
    float attackranged;

    float attackDelay = 0f;
    bool attacked = true;


    bool isDie = false;
    private float moveDelay = 0f;
    private bool move;

    private void Awake()
    {
        HP = data.hp;
        ranged = data.range;
        attackranged = data.attackRange;
        Speed = data.speed;

        enemyCollider = GetComponent<Collider>();
        anime = GetComponent<Animator>();
    }
    private void Update()
    {
        FindTarget();
    }
    private void FindTarget()
    {
       
        Collider[] findTarget = Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly", "Town"));
        Collider[] attackedTarget = Physics.OverlapSphere(transform.position, attackranged, LayerMask.GetMask("Friendly", "Town"));


        anime.SetFloat("IsSpeed", Speed);

        if (isDie)
            return;
        if (findTarget.Length <= 0)
        {
            return;
        }
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
            Vector3 dir = findTarget[index].transform.position - transform.position;
            Quaternion q = Quaternion.LookRotation(dir.normalized);
            transform.rotation = q;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);

            if (attackedTarget.Length > 0)
            {
                Speed = 0;
                Attack(attackedTarget);
                FireDelay();
            }
            else
            {
                move = true;
                MoveDelay();
                if (moveDelay >= 3f && move) { 
                    Speed = data.speed;
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
        if(null != effect)
       anime.SetTrigger("IsAttack");
       IDamaged damaged = target[0].GetComponent<IDamaged>();
       attacked = true;
       damaged?.Damaged(data.damage);
        enermyTargetPos = target[0].transform;
    }
    private void HitEffact()
    {
        GameObject saveEffact;
        if (effectPos == null)
            saveEffact = Instantiate(effect, enermyTargetPos.position, Quaternion.identity);
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
    public void Damaged(int attck)
    {
        HP -= attck;
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            Speed = 0;
            ranged = 0;
            attackranged = 0;
            isDie = true;
            enemyCollider.enabled = false;
            //TODO : 죽으면서 콜라이더 꺼서 한번만 죽게 만들기
            Die();
        }
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
    private void Die()
    {
        WaveManager.instance.monsterCount += -1;
        anime.SetTrigger("IsDie");
        Destroy(gameObject, 1.3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
    
    
}
