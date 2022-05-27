using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour, IDamaged
{
    public BossData data;

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
    float Speed;
    float ranged;

    float attackFarRange;
    float attackCloseRange;

    float attackDelay = 0f;
    bool meleeAttacked = true;
    float flameDelay = 0f;
    bool flameAttack = false;
    float flameFire = 0f;

    bool isDie = false;

    private float moveDelay = 0f;
    private bool move;

    public bool isStart = false;
    bool Victory = false;

    Collider[] findTarget;
    Collider[] attackedCloseTarget;
    public Collider[] attackedFarTarget;
    float distance;
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
        distance = 0;

    }
    private void Update()
    {
        FindTarget();
    }
    
    private void FindTarget()
    {

        findTarget         =  Physics.OverlapSphere(transform.position, ranged, LayerMask.GetMask("Friendly", "Town"));
        attackedCloseTarget = Physics.OverlapBox(transform.position,new Vector3(5,10, attackCloseRange), Quaternion.identity,LayerMask.GetMask("Friendly", "Town"));
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
                distance = Vector3.Distance(transform.position, findTarget[i].transform.position);
                if (min > distance)
                {
                    index = i;
                    min = distance;
                }
            }

            Vector3 dir = findTarget[index].transform.position - transform.position;
            Quaternion q = Quaternion.LookRotation(dir.normalized * Time.deltaTime);
            transform.rotation = q;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
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
                Speed = 0;
                //TODO : 원거리 공격 오브젝트 날리기
                anime.SetTrigger("IsFlame");
                
                flameAttack = true;
            }
            else if (attackedCloseTarget.Length > 0)
            {
                Vector3 dird = attackedCloseTarget[0].transform.position - transform.position;
                Speed = 0;
                MeleeAttack(attackedCloseTarget);
                meleeDelay();
                if (!meleeAttacked) {
                    Quaternion qq = Quaternion.LookRotation(dird.normalized * Time.deltaTime);
                    transform.rotation = qq;
                }
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
                IDamaged damaged = target[i].GetComponent<IDamaged>();
                damaged?.Damaged(data.damage);
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
            flameFire += Time.deltaTime;
            if(flameFire >= data.flameFire)
            {
                Speed = data.speed;
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
        Gizmos.DrawWireCube(closeAttackPos.position, new Vector3(5,10 ,data.attackCloseRange));

    }
    public void OnVictory()
    {
        Debug.Log("승리");
    }
}
