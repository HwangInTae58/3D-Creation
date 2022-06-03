using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public float translationSpeed;
    public float altitude;
    public float zoomSpeed;
    public float Distance;

    private Camera gameCamera;

    public GameObject saveOriginalPos;
    public GameObject bossSpawnPos;
    private Vector3 forwardDir;

    private void Awake()
    {
        translationSpeed = 100f;
        altitude = 40f;
        zoomSpeed = 100f;
    }
    private void Start()
    {
        forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        gameCamera = GetComponent<Camera>();
        saveOriginalPos.transform.position = transform.position;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            UIManager.instance.OpenPausdWindow();
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
        if (WaveManager.instance.boss) 
            StartCoroutine(BossSceneMove());
    }
    private void TranslateCamera(int dir)
    {
        if (WaveManager.instance.boss)
            return;
        if (dir == 0 && gameCamera.transform.position.z < 280) // 위
            transform.Translate(forwardDir * Time.fixedDeltaTime * translationSpeed, Space.World);
        else if (dir == 1 && gameCamera.transform.position.z > 0) // 아래
            transform.Translate(-forwardDir * Time.fixedDeltaTime * translationSpeed, Space.World);
        else if (dir == 2 && gameCamera.transform.position.x < 250) // 오른쪽
            transform.Translate(transform.right * Time.fixedDeltaTime * translationSpeed);
        else if (dir == 3 && gameCamera.transform.position.x > 70) // 왼쪽
            transform.Translate(-transform.right * Time.fixedDeltaTime * translationSpeed);

    }
    private IEnumerator BossSceneMove()
    {
        gameCamera.transform.position = bossSpawnPos.transform.position;
        yield return new WaitForSeconds(3.5f);
        WaveManager.instance.boss = false;
        gameCamera.transform.position = saveOriginalPos.transform.position;
    }
    private void Zoom(int zoomDir)
    {
        gameCamera.orthographicSize -= zoomDir * Time.fixedDeltaTime * zoomSpeed;
        gameCamera.orthographicSize = Mathf.Clamp(gameCamera.orthographicSize, 8f, 40f);
    }

}
