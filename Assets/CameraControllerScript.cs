using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{

    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3;
    [SerializeField] private float zoomSpeed = 300;

    private Vector3 dragOrigin;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        //if(scrollData != 0)
        //{
            targetZoom -= scrollData * zoomFactor;
            float yVelocity = 0.0f;
            targetZoom = Mathf.Clamp(targetZoom, 2f, 12f);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomSpeed);

        //if(scrollData > 0)
        //    cam.transform.position = new Vector3((cam.transform.position.x + cam.ScreenToWorldPoint(Input.mousePosition).x) / 2,
        //    (cam.transform.position.y + cam.ScreenToWorldPoint(Input.mousePosition).y) / 2,
        //    cam.transform.position.z);
        //else
        //    cam.transform.position = new Vector3((cam.transform.position.x - cam.ScreenToWorldPoint(Input.mousePosition).x) / 2,
        //    (cam.transform.position.y - cam.ScreenToWorldPoint(Input.mousePosition).y) / 2,
        //    cam.transform.position.z);
        //}

        //Движение по WASD камеры
        PanCamera();
    }

    void PanCamera()
    {
        if(Input.GetMouseButtonDown(1))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }
    }
}
