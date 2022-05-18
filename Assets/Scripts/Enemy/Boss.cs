using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour, IDamaged
{
    public BossData data;

    Animator anime;
    Collider enemyCollider;

    int HP;
    float Speed;
    float ranged;

    float attackFarRange;
    float attackCloseRange;
    float attackDelay = 0f;
    bool attacked = true;

    bool isDie = false;

    private float moveDelay = 0f;
    private bool move = true;

    public bool isStart = false;
    public bool Victory = false;

    List<IDamaged> damaged;

    private void Awake()
    {
        HP = data.hp;
        ranged = data.range;
        attackFarRange = data.attackFarRange;
        attackCloseRange = data.attackCloseRange;

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
        Collider[] attackedCloseTarget = Physics.OverlapSphere(transform.position, attackCloseRange, LayerMask.GetMask("Friendly", "Town"));
        Collider[] attackedFarTarget = Physics.OverlapSphere(transform.position, attackFarRange, LayerMask.GetMask("Friendly", "Town"));

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
                    //TODO : 보스가 어느정도 거리면 날 수 있게
                }
            }
            Vector3 dir = findTarget[index].transform.position - transform.position;

            //바라보게 하기 코드
            Quaternion q = Quaternion.LookRotation(dir.normalized);
            transform.rotation = q;

            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
            if (attackedCloseTarget.Length > 0)
            {
                Speed = 0;
                Attack(attackedCloseTarget);
                FireDelay();
            }
            else if (attackedFarTarget.Length > 0 && attackedCloseTarget.Length < 0)
            {
                //TODO : 원거리 공격 오브젝트 날리기
            }
            else
            {
                move = true;
                MoveDelay();
                if (moveDelay >= 3f && move)
                {
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
        if (move)
        {
            moveDelay += Time.deltaTime;
        }
        if(!isStart)
        {
            isStart = true;
            anime.SetTrigger("IsStart");
        }
    }
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;
        if (attacked)
            return;
        anime.SetTrigger("IsCloseAttack");
       // yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < data.attacker; i++)
        {
            if (target[i] != null)
            {
                damaged = new List<IDamaged>();
                damaged.Add(target[i].GetComponent<IDamaged>());
                attacked = true;

            }
            damaged[i]?.Damaged(data.damage);
        }
    }
    
    public void Damaged(int attck)
    {
        HP -= attck;
        if (HP <= 0)
        {
            Speed = 0;
            ranged = 0;
            attackFarRange = 0;
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
        Victory = data.victory;
        if (Victory)
            OnVictory();
        WaveManager.instance.monsterCount += -1;
        anime.SetTrigger("IsDie");
        Destroy(gameObject, 1.3f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
        Gizmos.DrawWireSphere(transform.position, data.attackFarRange);
        Gizmos.DrawWireSphere(transform.position, data.attackCloseRange);

    }
    public void OnVictory()
    {

    }
}
