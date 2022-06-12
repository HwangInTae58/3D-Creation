using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Friendly : MonoBehaviour, IDamaged
{
    public FriendlyData data;
    NavMeshAgent agent;

    AudioSource audioSource;
    public AudioClip[] audioClip;

    Animator anime;
    Collider fricollider;
    public Transform face;

    Transform enermyTargetPos;
    public GameObject effect;
    public Transform effectPos;

    public GameObject damageEffect;
    public Transform bloodPos;

    int HP;
    float attackDelay;
    bool  attacked;

    float ranged;
    float attackranged;

    private float moveDelay;
    private bool  move;
    public Vector3 originalPos;

    bool isDie;
   // bool isCall = true;

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
        fricollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
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
            //Vector3 dir = findTarget[index].transform.position - transform.position;
            //Quaternion q = Quaternion.LookRotation(dir.normalized);
            //transform.rotation = q;
            //transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
            agent.SetDestination(findTarget[index].transform.position);

            if (attackedTarget.Length > 0)
            {
                Vector3 dird = attackedTarget[0].transform.position - transform.position;
                Quaternion qq = Quaternion.LookRotation(dird.normalized * Time.deltaTime);
                agent.transform.rotation = qq;
                FireDelay();
                Attack(attackedTarget);
                agent.speed = 0;
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
        else
        {
            if (!move) {
                MoveDelay();
                return;
            }
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
    private void Attack(Collider[] target)
    {
        if (isDie)
            return;
        if (attacked)
            return;
        if (target[0] == null)
            return;

        
        anime.SetTrigger("IsAttack");
        enermyTargetPos = target[0].transform;
        IDamaged damaged = target[0].GetComponent<IDamaged>();
        attacked = true;
        damaged?.Damaged(data.damage);
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
        Invoke("DamageEffect", 0.3f);
        if (HP <= 0)
        {
            agent.speed = 0;
            ranged = 0;
            attackranged = 0;
            isDie = true;
            fricollider.enabled = false;
            Die();
        }
    }
    private void HitEffact()
    {
        audioSource.clip = audioClip[0];
        audioSource.Play();
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
        audioSource.clip = audioClip[1];
        audioSource.Play();
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
