using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    Vector3 mousePos;

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown()
    {
        if (BuildManager.instance.selectedTown != null)
        { 
            BuildManager.instance.OnTownBuild(mousePos);
            Debug.Log("집소환");
        }
        if (BuildManager.instance.selectedFriendly != null)
        { 
            BuildManager.instance.OnFriendlyBuild(mousePos);
            Debug.Log("병사소환");
        }
    }
}
