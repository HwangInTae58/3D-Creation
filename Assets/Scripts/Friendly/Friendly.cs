using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friendly : MonoBehaviour, IDamaged
{
    public FriendlyData data;

    Animator anime;
    Collider fricollider;
    public Transform face;

    Transform enermyTargetPos;
    public GameObject effect;
    public Transform effectPos;

    public GameObject damageEffect;
    public Transform bloodPos;

    int HP;
    float Speed = 0;

    float attackDelay = 0f;
    bool attacked = true;

    float ranged;
    float attackranged;

    private float moveDelay = 0f;
    private bool move = true;
    public Vector3 originalPos;

    bool isDie = false;
   // bool isCall = true;

    private void Awake()
    {
        HP = data.hp;
        ranged = data.range;
        attackranged = data.attackRange;

        anime = GetComponent<Animator>();
        fricollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        originalPos = transform.position;
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
            //바라보게 하기 코드
            Vector3 dir = findTarget[index].transform.position - transform.position;
            Quaternion q = Quaternion.LookRotation(dir.normalized);
            transform.rotation = q;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);

            if (attackedTarget.Length > 0)
            {
                FireDelay();
                Attack(attackedTarget);
                Speed = 0;
                move = false;
            }
            else
            {
                if (!move)
                    MoveDelay();
                else
                    Speed = data.speed;
            }
            
        }
        else
        {
            if (!move) {
                MoveDelay();
                return;
            }
            if (Vector3.Distance(originalPos, transform.position) >= 0.04f)
            {
                Vector3 purdir = originalPos - transform.position;
                transform.Translate(purdir.normalized * Speed * Time.deltaTime, Space.World);
                Quaternion q = Quaternion.LookRotation(purdir.normalized);
                transform.rotation = q;
            }
            else if (Vector3.Distance(originalPos, transform.position) < 0.04f)
                Speed = 0;
            return;
        }
    }
    private void MoveDelay()
    {
        if (!move)
        {
            moveDelay += Time.deltaTime;
            if (moveDelay >= 1f)
            {
                Speed = data.speed;
                move = true;
                moveDelay = 0f;
            }
        }
    }
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;
        if (attacked)
            return;
            anime.SetTrigger("IsAttack");
            IDamaged damaged = target[0].GetComponent<IDamaged>();
            attacked = true;
            damaged?.Damaged(data.damage);
        enermyTargetPos = target[0].transform;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ranged);
        Gizmos.DrawWireSphere(transform.position, attackranged);
    }
    public void Damaged(int attck)
    {
        //TODO : 자신이 공격당하였을때
        //if(Vector3.Distance(target[0].transform.position, transform.position) > data.range)
        //{
        //    Vector3 dir = target[0].transform.position - transform.position;
        //    transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
        //    Quaternion q = Quaternion.LookRotation(dir.normalized);
        //    transform.rotation = q;
        //}
        HP -= attck;
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            Speed = 0;
            ranged = 0;
            attackranged = 0;
            isDie = true;
            fricollider.enabled = false;
            Die();
        }
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

    private void Die()
    {
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
