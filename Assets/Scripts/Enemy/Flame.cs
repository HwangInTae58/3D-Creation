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

    public AudioClip[] audioClip;

    float speed;
    float range;
    float atRange;
    float atExtent;

    public bool hitFlame;

    private void Awake()
    {
        speed = data.speed;
        range = boss.data.range;
        atRange = data.atRange;
        atExtent = data.atExtent;
        hitFlame = false;
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
        Collider[] findDamageTarget = Physics.OverlapBox(transform.position, new Vector3(atExtent, 5, atRange), boss.transform.rotation, LayerMask.GetMask("Friendly", "Town"));
        //Physics.OverlapSphere(transform.position, atRange, LayerMask.GetMask("Friendly", "Town"));
        if (findTarget.Length <= 0)
            return;
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

            if (Vector3.Distance(findTarget[index].transform.position, transform.position) < 0.8f)
            {
                if (null != prefab)
                    prefab.SetActive(false);
                if (null != endPrefab)
                    endPrefab.SetActive(true);
               
                hitflame(findDamageTarget);
            }

        }
    }
    private void hitflame(Collider[] target)
    {
        if (target.Length > 0 && !hitFlame)
        {
            if (null != audioClip[0])
            {
                //audiosource.clip = audioClip[0];
                //audiosource.Play();
                SoundPool.instance.SetSound(audioClip[0], gameObject.transform, 0.8f);
            }
            hitFlame = true;
            speed = 0;
            Destroy(gameObject, 1f);
            for (int i = 0; i < target.Length; i++)
            {
                if (target[i] != null)
                {
                    IDamaged damaged = target[i].GetComponent<IDamaged>();
                    damaged?.Damaged(data.damage);
                }
                else
                    return;
            }
        }
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,new Vector3(data.atExtent, 5, data.atRange));
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

   


