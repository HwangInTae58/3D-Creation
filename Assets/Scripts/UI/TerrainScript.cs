using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TerrainScript : MonoBehaviour
{
    public Camera gameCamera;

    private void Update()
    {
        
        OnBuild();
    }
 
    private void OnBuild() 
    {
        
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetButtonDown("Fire1") ) 
        {
            if (EventSystem.current.IsPointerOverGameObject() == false) 
            {  //UI이 위가 아니면 빌드
                if (BuildManager.instance.selectedTown != null && Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                        return;
                    BuildManager.instance.OnTownBuild(hit);
                    Debug.Log("집소환");
                }
                if (BuildManager.instance.selectedFriendly != null && Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                        return;
                    BuildManager.instance.OnFriendlyBuild(hit);
                    Debug.Log("병사소환");
                }
            }
        
        }
    }

}
