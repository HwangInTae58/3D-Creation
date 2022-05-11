using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public float translationSpeed = 40f;
    public float altitude = 40f;
    public float zoomSpeed = 100f;
    public float Distance;

    private Camera camera;
    private RaycastHit hit;
    private Ray ray;

    private Vector3 forwardDir;
    private Vector3 AxisVec;  
    private int mouseOnScreenBorder;
    private Coroutine mouseOnScreenCoroutine;
    private Transform mainTransForm;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        mainTransForm = camera.transform;
        forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        mouseOnScreenBorder = -1;
        mouseOnScreenCoroutine = null;
    }

    private void Update()
    {
        if(mouseOnScreenBorder >= 0)
        {
            TranslateCamera(mouseOnScreenBorder);
        }
        if (Input.GetKey(KeyCode.UpArrow))
            TranslateCamera(0);
        else if (Input.GetKey(KeyCode.DownArrow))
            TranslateCamera(1);
        else if (Input.GetKey(KeyCode.RightArrow))
            TranslateCamera(2);
        else if (Input.GetKey(KeyCode.LeftArrow))
            TranslateCamera(3);
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
            Zoom(Input.mouseScrollDelta.y > 0f ? 1 : -1);

    }
    private void TranslateCamera(int dir)
    {
        if (dir == 0) // 위
            transform.Translate(forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 1) // 아래
            transform.Translate(-forwardDir * Time.deltaTime * translationSpeed, Space.World);
        else if (dir == 2) // 오른쪽
            transform.Translate(transform.right * Time.deltaTime * translationSpeed);
        else if (dir == 3) // 왼쪽
            transform.Translate(-transform.right * Time.deltaTime * translationSpeed);

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
        camera.orthographicSize -= zoomDir * Time.deltaTime * zoomSpeed;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 8f, 26f);

    }

}
