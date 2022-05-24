using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flame : MonoBehaviour
{
    public Boss boss;
    public FlameData data;

    public GameObject prefab;
    public GameObject endPrefab; 

    float speed;
    float range;
    float atRange;

    public bool hitFlame;

    private void Awake()
    {
        speed = data.speed;
        hitFlame = false;
    }
    private void Start()
    {
        range = boss.data.attackFarRange;
        atRange = 1f;
    }
    public void OnFlame(Transform FlamePos)
    {
        Instantiate(data.prefab, FlamePos.position, Quaternion.identity);
    }
    private void Update()
    {
        MoveFlame();
    }

    public void MoveFlame()
    {
        Collider[] findTarget = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Friendly", "Town"));
        Collider[] findDamageTarget = Physics.OverlapSphere(transform.position, atRange, LayerMask.GetMask("Friendly", "Town"));
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

            //바라보게 하기 코드
            Vector3 dir = findTarget[index].transform.position - transform.position;
            Quaternion q = Quaternion.LookRotation(dir.normalized);
            transform.rotation = q;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(findTarget[index].transform.position, transform.position) < 0.1f)
            {
                if(null != prefab)
                    prefab.SetActive(false);
                if(null != endPrefab)
                    endPrefab.SetActive(true);
                hitflame(findDamageTarget);
            }
            
        }
    }
    private void hitflame(Collider[] target)
    {
        if (target.Length > 0 && !hitFlame)
        {
            hitFlame = true;
            speed = 0;
            Destroy(gameObject, 1f);
            for (int i = 0; i < target.Length; i++)
            {
                IDamaged damaged = target[i].GetComponent<IDamaged>();
                damaged?.Damaged(data.damage);
                
            }
        }
       
    }
}
    //public void MoveFlame(Transform FlamePos, Collider[] target)
    //{
    //    Vector3 dir = target[0].transform.position - FlamePos.position;
    //    Vector3 targetPos = target[0].transform.position;
    //    if (Vector3.Distance(targetPos, transform.position) > 0.1f)
    //    {
    //        Debug.Log("이동");
    //        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    //    }
    //    if (Vector3.Distance(targetPos, transform.position) <= 0.1f)
    //        Hit(target);
    //}

   


