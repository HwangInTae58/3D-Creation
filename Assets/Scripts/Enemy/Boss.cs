using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour, IDamaged
{
    public BossData data;

    Animator anime;
    Collider enemyCollider;

    public Flame flame;
    public Transform flamePos;

    int HP;
    float Speed;
    float ranged;

    float attackFarRange;
    float attackCloseRange;

    float attackDelay = 0f;
    bool meleeAttacked = true;
    float flameDelay = 0f;
    bool flameAttack = false;

    bool isDie = false;

    private float moveDelay = 0f;
    private bool move;

    public bool isStart = false;
    public bool Victory = false;

    Collider[] findTarget;
    Collider[] attackedCloseTarget;
    public Collider[] attackedFarTarget;
    Collider[] saveFarTarget;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider>();
        anime = GetComponent<Animator>();
    }
    private void Start()
    {
        HP = data.hp;
        ranged = data.range;
        attackFarRange = data.attackFarRange;
        attackCloseRange = data.attackCloseRange;
    }
    private void Update()
    {
        FindTarget();
    }
    private void FindTarget()
    {

        findTarget         =  Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly", "Town"));
        attackedCloseTarget = Physics.OverlapSphere(transform.position, attackCloseRange, LayerMask.GetMask("Friendly", "Town"));
        attackedFarTarget   = Physics.OverlapSphere(transform.position, attackFarRange, LayerMask.GetMask("Friendly", "Town"));

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
            //바라보게 하기 코드
            Vector3 dir = findTarget[index].transform.position - transform.position;
            Quaternion q = Quaternion.LookRotation(dir.normalized);
            transform.rotation = q;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);

            if (attackedFarTarget.Length > 0 && attackedCloseTarget.Length <= 0)
            {
                Speed = 0;
                //TODO : 원거리 공격 오브젝트 날리기
                FlameAttack(attackedFarTarget);
                FlameDelay();
            }
            else if (attackedCloseTarget.Length > 0)
            {
                Speed = 0;
                MeleeAttack(attackedCloseTarget);
                meleeDelay();
            }
            else
            {
                move = true;
                MoveDelay();
            }

        }
    }
    private void MoveDelay()
    {
        if (move)
        {
            moveDelay += Time.deltaTime;
            if (moveDelay >= 5f)
            {
                Speed = data.speed;
                moveDelay = 0;
                move = false;
            }
        }
        if(!isStart)
        {
            isStart = true;
            anime.SetTrigger("IsStart");
        }
    }
    private void FlameAttack(Collider[] target)
    {
        if (isDie)
            return;
        if (flameAttack)
            return;
        anime.SetTrigger("IsFlame");
        flame.OnFlame(flamePos);
        flameAttack = true;
    }
    private void MeleeAttack(Collider[] target)
    {
        if (isDie)
            return;
        if (meleeAttacked)
            return;
       
        if (target.Length > 0)
        {
            anime.SetTrigger("IsCloseAttack");
            
            for (int i = 0; i < target.Length; i++) {
                Vector3 dir = target[0].transform.position - transform.position;
                Quaternion q = Quaternion.LookRotation(dir.normalized);
                transform.rotation = q;

                IDamaged damaged = target[i].GetComponent<IDamaged>();
                damaged?.Damaged(data.damage);
            }
            meleeAttacked = true;
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
    private void meleeDelay()
    {
        if (meleeAttacked)
        {
            attackDelay += Time.deltaTime;
            if (attackDelay >= data.fireDelay)
            {
                meleeAttacked = false;
                attackDelay = 0f;
            }
        }
    }
    private void FlameDelay()
    {
        if (flameAttack)
        {
            flameDelay += Time.deltaTime;
            Speed = data.speed;
            if (flameDelay >= data.flameDelay)
            {
                flameAttack = false;
                flameDelay = 0f;
            }
        }
    }
    private void Die()
    {
        //TODO : 승리
        Victory = data.victory;
        if (Victory)
            OnVictory();
        WaveManager.instance.monsterCount += -1;
        anime.SetTrigger("IsDie");
        Destroy(gameObject, 3f);
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
        Debug.Log("승리");
    }
}
