using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamaged
{
    public EnemyData data;

    Animator anime;
    Collider enemyCollider;

    int HP;
    float Speed;

    float ranged;
    float attackranged;

    float attackDelay = 0f;
    bool attacked = true;


    bool isDie = false;
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
       
        Collider[] findTarget = Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly"));
        Collider[] attackedTarget = Physics.OverlapSphere(transform.position, attackranged, LayerMask.GetMask("Friendly"));
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
            Vector3 dir = findTarget[index].transform.position - transform.position;

            //바라보게 하기 코드
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
                //어택사거리에 적이 없으면 다시 속도가 생기게 하기
                Speed = data.speed;
                return;
            }

        }
    }
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;

        IDamaged damaged = target[0].GetComponent<IDamaged>();
        if (!attacked)
        {
            attacked = true;
            damaged?.Damaged(data.damage);
            anime.SetTrigger("IsAttack");
        }
    }
    public void Damaged(int attck)
    {
        HP -= attck; 

        if(HP <= 0)
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
