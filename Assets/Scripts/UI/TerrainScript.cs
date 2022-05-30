using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerrainScript : MonoBehaviour
{
    public Camera gameCamera;
    bool waveStart;

    private void Update()
    {
        waveStart = WaveManager.instance.waveStart;
        OnBuild();
    }
 
    private void OnBuild() 
    {
        
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1")) 
        {
            if (EventSystem.current.IsPointerOverGameObject() == false) 
            {  //UI이 위가 아니면 빌드
                if (BuildManager.instance.selectedTown != null && Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                        return;
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                        return;
                    BuildManager.instance.OnTownBuild(hit);
                }
                if (BuildManager.instance.selectedFriendly != null && Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                        return;
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                        return;
                    BuildManager.instance.OnFriendlyBuild(hit);
                }
            }
        }
        else if (Input.GetButtonDown("Fire2") && Physics.Raycast(ray,out hit) && !waveStart)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Town"))
                hit.collider.transform.Rotate(new Vector3(0, 90));
        }
    }

}
