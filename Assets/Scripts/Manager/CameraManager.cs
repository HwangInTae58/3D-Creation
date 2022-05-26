﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Cameramanager : MonoBehaviour
{
    public float translationSpeed = 40f;
    public float altitude = 40f;
    public float zoomSpeed = 100f;
    public float Distance;

    private Camera gameCamera;

    public GameObject saveOriginalPos;
    public GameObject cameraSavePos;

    private Vector3 forwardDir;
    private int mouseOnScreenBorder;
    private Coroutine mouseOnScreenCoroutine;

    private void Awake()
    {
        gameCamera = GetComponent<Camera>();
        forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        mouseOnScreenBorder = -1;
        mouseOnScreenCoroutine = null;
    }
    private void Start()
    {
        saveOriginalPos.transform.position = transform.position;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            UIManager.instance.OpenPausdWindow();
        if(mouseOnScreenBorder >= 0)
            TranslateCamera(mouseOnScreenBorder);
        if (Input.GetKey(KeyCode.W))
            TranslateCamera(0);
        else if (Input.GetKey(KeyCode.S))
            TranslateCamera(1);
        else if (Input.GetKey(KeyCode.D))
            TranslateCamera(2);
        else if (Input.GetKey(KeyCode.A))
            TranslateCamera(3);
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
            Zoom(Input.mouseScrollDelta.y > 0f ? 1 : -1);
        if (Input.GetKeyDown(KeyCode.Space))
            transform.position = saveOriginalPos.transform.position;
        if (WaveManager.instance.boss) { 
        BossScene(WaveManager.instance.boss);

        }
    }
    private void TranslateCamera(int dir)
    {
        if (dir == 0 && gameCamera.transform.position.z < 280) // 위
            transform.Translate(forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 1 && gameCamera.transform.position.z > 0) // 아래
            transform.Translate(-forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 2 && gameCamera.transform.position.x < 300) // 오른쪽
            transform.Translate(transform.right * Time.deltaTime * translationSpeed);
        else if (dir == 3 && gameCamera.transform.position.x > 65) // 왼쪽
            transform.Translate(-transform.right * Time.deltaTime * translationSpeed);

    }
    public void BossScene(bool boss)
    {
        cameraSavePos.transform.position = gameCamera.transform.position;
        gameCamera.transform.position = new Vector3(280, 60, 146);
        Debug.Log("/?");
        StartCoroutine(BossSceneMove());
    }
    private IEnumerator BossSceneMove()
    {
        Debug.Log("123");
        yield return new WaitForSeconds(2f);
        //TODO : Lerp사용
        gameCamera.transform.Translate(cameraSavePos.transform.position.normalized * 100 * Time.deltaTime, Space.World);
        WaveManager.instance.boss = false;
    }
    public void OnMouseEnterScreenBorder(int borderIndex)
    {
        mouseOnScreenCoroutine = StartCoroutine(SetMounseOnScreenBorder(borderIndex));
    }
    public void OnMouseExitScreenBorder()
    {
        StopCoroutine(mouseOnScreenCoroutine);
        mouseOnScreenBorder = -1;
    }
    
    private IEnumerator SetMounseOnScreenBorder(int borderIndex)
    {
        yield return new WaitForSeconds(0.3f);
        mouseOnScreenBorder = borderIndex;
    }
    private void Zoom(int zoomDir)
    {
        gameCamera.orthographicSize -= zoomDir * Time.deltaTime * zoomSpeed;
        gameCamera.orthographicSize = Mathf.Clamp(gameCamera.orthographicSize, 8f, 40f);

    }

}
