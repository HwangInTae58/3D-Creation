using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour, IDamaged
{
    public FriendlyData data;

    Animator anime;
    Rigidbody rigid;
    Collider fricollider;
    public Transform face;

    int HP;
    float Speed;

    float attackDelay = 0f;
    bool attacked = false;

    float ranged;
    float attackranged;

    bool isDie = false;

    private void Awake()
    {
        HP = data.hp;
        Speed = data.speed;
        ranged = data.range;
        attackranged = data.attackRange;

        
        rigid = GetComponent<Rigidbody>();
        anime = GetComponent<Animator>();
        fricollider = GetComponent<Collider>();
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
        Collider[] findTarget = Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Enemy"));
        Collider[] attackedTarget = Physics.OverlapSphere(transform.position, attackranged, LayerMask.GetMask("Enemy"));
        anime.SetFloat("IsSpeed", Speed);
        if (isDie)
            return;

        if (findTarget.Length > 0)
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
                Attack(attackedTarget);
                Speed = 0;
            }
            else
            {
                //어택사거리에 적이 없으면 다시 속도가 생기게 하기
                Speed = data.speed;
                return;
            }
            
        }
        else
        {
            Speed = 0;
            return;

        }
    }
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;
        IDamaged damaged = target[0].GetComponent<IDamaged>();
        if (!attacked) {
            attacked = true;
            anime.SetTrigger("IsAttack");
            damaged?.Damaged(data.damage);

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ranged);
        Gizmos.DrawWireSphere(transform.position, attackranged);
    }

    public void Damaged(int attck)
    {
        HP -= attck;
        if(0 >= HP)
        {
            Speed = 0;
            ranged = 0;
            attackranged = 0;
            isDie = true;
            fricollider.enabled = false;
            Die();
        }
    }
    private void Die()
    {
        // TODO : 아군 죽음
        anime.SetTrigger("IsDie");
        Destroy(gameObject, 1.3f);
    }
    private void FireDelay()
    {
        if(attacked && !isDie)
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
