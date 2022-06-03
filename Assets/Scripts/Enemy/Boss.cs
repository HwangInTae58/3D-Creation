using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour, IDamaged
{
    public BossData data;
    NavMeshAgent agent;

    Animator anime;
    Collider enemyCollider;

    public Transform closeAttackPos;
    public Flame flame;
    public Transform flamePos;

    public GameObject effect;
    public Transform effectPos;
    public GameObject damageEffect;
    public Transform bloodPos;

    int HP;
    float ranged;

    float attackFarRange;
    float attackCloseRange;

    float   attackDelay;
    bool    meleeAttacked;
    float   flameDelay;
    bool    flameAttack;
    float   flameFire;

    bool isDie;

    private float moveDelay = 0f;
    private bool move;

    public bool isStart;
    public bool Victory;

    Collider[] findTarget;
    Collider[] attackedCloseTarget;
    public Collider[] attackedFarTarget;
    float distance;
    private void Awake()
    {
        HP = data.hp;
        ranged = data.range;
        attackFarRange = data.attackFarRange;
        attackCloseRange = data.attackCloseRange;
        attackDelay = data.fireDelay;
        flameDelay = data.flameDelay;
        meleeAttacked = true;
        flameAttack = false;
        isDie = false;
        Victory = false;
        isStart = false;
        flameFire = 0f;

        distance = 0;
    }
    private void Start()
    {
        enemyCollider = GetComponent<Collider>();
        anime = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = data.speed;
    }
    private void Update()
    {
        if (Victory)
            StartCoroutine(OnVictory());
        FindTarget();
    }
    
    private void FindTarget()
    {
        findTarget         =  Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly", "Town"));
        attackedCloseTarget = Physics.OverlapBox(closeAttackPos.position,new Vector3(5,10, attackCloseRange), transform.rotation,LayerMask.GetMask("Friendly", "Town"));
        attackedFarTarget   = Physics.OverlapSphere(transform.position, attackFarRange, LayerMask.GetMask("Friendly", "Town"));

        anime.SetFloat("IsSpeed", agent.speed);
        if (Time.timeScale <= 0)
            return;
        if (isDie)
            return;
        if (findTarget.Length <= 0)
            return;
        else
        {
            float min = int.MaxValue;
            int index = 0;
            for (int i = 0; i < findTarget.Length; i++)
            {
                distance = Vector3.Distance(transform.position, findTarget[i].transform.position);
                if (min > distance)
                {
                    index = i;
                    min = distance;
                }
            }
            //Vector3 dir = findTarget[index].transform.position - transform.position;
            //Quaternion q = Quaternion.LookRotation(dir.normalized * Time.deltaTime);
            //transform.rotation = q;
            //transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
            agent.SetDestination(findTarget[index].transform.position);
            //바라보게 하기 코드
            //transform.rotation = q;
            //transform.rotation = Quaternion.LookRotation(dir.normalized * Time.deltaTime);
            if (attackedFarTarget.Length > 0 && attackedCloseTarget.Length <= 0)
            {
                if (isDie)
                    return;
                if (flameAttack) {
                    FlameDelay();
                    return;
                }
                agent.speed = 0;
                //TODO : 원거리 공격 오브젝트 날리기
                anime.SetTrigger("IsFlame");
                
                flameAttack = true;
            }
            else if (attackedCloseTarget.Length > 0)
            {
                Vector3 dird = attackedCloseTarget[0].transform.position - transform.position;
                agent.speed = 0;
                MeleeAttack(attackedCloseTarget);
                meleeDelay();
                if (!meleeAttacked) {
                    Quaternion qq = Quaternion.LookRotation(dird.normalized * Time.deltaTime);
                    agent.transform.rotation = qq;
                }
            }
            else
                MoveDelay();
        }
    }
    private void MoveDelay()
    {
        move = true;
        if (move)
        {
            moveDelay += Time.deltaTime;
            if (moveDelay >= 5f)
            {
                agent.speed = data.speed;
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
    private void FlameAttack()
    {
        flame.OnFlame(flamePos);
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
                if (target[i] != null)
                {
                    IDamaged damaged = target[i].GetComponent<IDamaged>();
                    damaged?.Damaged(data.damage);
                }
                else
                    return;
            }
            meleeAttacked = true;
        }
    }
    private void HitEffact()
    {
        GameObject saveEffact;
            saveEffact = Instantiate(effect, effectPos.position, Quaternion.identity);
        Destroy(saveEffact, 1f);
    }
    private void DamageEffect()
    {
        if (null == damageEffect)
            return;
        GameObject saveEffact = Instantiate(damageEffect, bloodPos.position, Quaternion.identity);
        Destroy(saveEffact, 1.5f);
    }
    public void Damaged(int attck)
    {
        HP -= attck;
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            agent.speed = 0;
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
            flameFire += Time.deltaTime;
            if(flameFire >= data.flameFire)
            {
                agent.speed = data.speed;
                flameFire = 0f;
            }
            
            if (flameDelay >= data.flameDelay)
            {
                flameAttack = false;
                flameDelay = 0f;
            }
        }
    }
    private void Die()
    {
        Victory = data.victory;
        WaveManager.instance.monsterCount += -1;
        anime.SetTrigger("IsDie");
        Destroy(gameObject, 3f);
    }
    private void OnDrawGizmos()
    {
        //TODO : 사정거리를 눈으로 확인하기 위한 작업 제작완료시 삭제
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.range);
        Gizmos.DrawWireSphere(transform.position, data.attackFarRange);
        Gizmos.DrawWireCube(closeAttackPos.position, new Vector3(5,10 ,data.attackCloseRange));
    }
    public IEnumerator OnVictory()
    {
        yield return new WaitForSeconds(2.9f);
        UIManager.instance.GameVictory();
        yield return new WaitForSecondsRealtime(3f);
        Gamemanager.instance.ChangeScene("TitleScene");
    }
}
