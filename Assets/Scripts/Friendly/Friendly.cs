using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour, IDamaged
{
    public FriendlyData data;

    Rigidbody rigid;

    int HP;
    float Speed;

    float attackDelay = 0f;
    bool attacked = false;

    private void Awake()
    {
        HP = data.hp;
        Speed = data.speed;

        rigid = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        FireDelay();
    }
    private void Update()
    {

        FindTarget();
    }

    private void FindTarget()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, data.range, LayerMask.GetMask("Enemy"));
        Collider[] attackedTarget = Physics.OverlapSphere(transform.position, data.attackRange, LayerMask.GetMask("Enemy"));

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
                float distance = Vector3.Distance(transform.position,findTarget[i].transform.position);
                if (min > distance) {
                    index = i;
                    min = distance;
                }
            }
            Vector3 dir = findTarget[index].transform.position - transform.position;
            transform.LookAt(dir);
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
            if(attackedTarget.Length > 0)
            {
                Attack(attackedTarget);
                
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
        IDamaged damaged = target[0].GetComponent<IDamaged>();
        if (!attacked) {
            Speed = 0;
            attacked = true;
            damaged?.Damaged(data.damage);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.range);
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }

    public void Damaged(int attck)
    {
        HP -= attck;
        if(0 >= HP)
        {
            Die();
        }
    }
    private void Die()
    {
        // TODO : 아군 죽음
        Destroy(gameObject);
    }
    private void FireDelay()
    {
        if(attacked)
        {
            attackDelay += Time.deltaTime;
            if(attackDelay >= data.fireDelay)
            {
                attacked = false;
                attackDelay = 0f;
            }
        }
    }
}
