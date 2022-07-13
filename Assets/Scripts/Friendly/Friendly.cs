using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Friendly : Character , IDamaged
{
    public FriendlyData data;
    public Transform face;
    public Vector3 originalPos;
    private void Awake()
    {
        HP = data.hp;
        attackDelay = data.fireDelay;
        ranged = data.range;
        attackranged = data.attackRange;
        moveDelay = 0f;
        attacked = true;
        move = true;
        isDie = false;
        
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();
        charCollider = GetComponent<Collider>();
        agent.speed = data.speed;
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
        anime.SetFloat("IsSpeed", agent.speed);
        if (isDie)
            return;
        if (Time.timeScale <= 0)
            return;
        if (findTarget.Length > 0) // 사거리에 타겟이 있으면
        {
            float min = int.MaxValue;
            int index = 0;
            // for문을 통해 가장 가까이 있는 적을 찾아낸다.
            for (int i = 0; i < findTarget.Length; i++) 
            {
                float distance = Vector3.Distance(transform.position, findTarget[i].transform.position);
                if (min > distance)
                {
                    index = i;
                    min = distance;
                }
            }
            agent.SetDestination(findTarget[index].transform.position);
            //공격 사거리에 적이 있다면 attack을 실행한다.
            if (attackedTarget.Length > 0)
            {
                agent.speed = 0;
                Attack(attackedTarget);
                FireDelay();
                Vector3 dird = attackedTarget[0].transform.position - transform.position;
                Quaternion qq = Quaternion.LookRotation(dird.normalized * Time.deltaTime);
                agent.transform.rotation = qq;
                move = false;
            }
            else
            {
                if (!move)
                    MoveDelay();
                else
                    agent.speed = data.speed;
            }
            
        }
        else//사거리에 타겟이 없으면
        {
            if (!move) {
                MoveDelay();
                return;
            }
            // 자신의 원래 자리와 지금 자리의 차이가 크면 돌아간다
            if (Vector3.Distance(originalPos, transform.position) >= 0.3f) 
            {
                agent.SetDestination(originalPos);
            }
            else if (Vector3.Distance(originalPos, transform.position) < 0.3f)
                agent.speed = 0;
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
                agent.speed = data.speed;
                move = true;
                moveDelay = 0f;
            }
        }
    }
    private void Attack(Collider[] target)//콜라이더로 데미지 받는 객체 저장
    {
        if (isDie)
            return;
        if (attacked)
            return;
        if (target[0] == null)
            return;
        anime.SetTrigger("IsAttack");
        enermyTargetPos = target[0].transform;
        //IDamaged인터페이스를 갖는 객체를 갖는다.
        IDamaged damaged = target[0].GetComponent<IDamaged>();
        damaged?.Damaged(data.damage);//객체에게 데미지를 준다.
        attacked = true;
    }
    //이 클래스가 인터페이스를 상속받게 해서 사용하게 만든다.
    public void Damaged(int attck)
    {
        HP -= attck;
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            anime.SetTrigger("IsDie");
            Die(1.3f, 1, 0.8f, false);
        }
    }
    private void HitEffact()
    {
        SoundPool.instance.SetSound(audioClip[0], gameObject.transform, 0.5f);
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
