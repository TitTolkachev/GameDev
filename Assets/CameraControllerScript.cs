using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{

    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3;
    [SerializeField] private float zoomSpeed = 300;

    [SerializeField]
    public SpriteRenderer mapRenderer;

    private Vector3 dragOrigin;

    public float mapMinX = -30, mapMaxX = 30, mapMinY = -15, mapMaxY = 15;
    public float zoomMin = 2f, zoomMax = 12f;

    //private void Awake()
    //{
    //    mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
    //    mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
    //    mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
    //    mapMaxY = mapRenderer.transform.position.x + mapRenderer.bounds.size.y / 2f;
    //}

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        if(/*!FindObjectOfType<LevelManagerScript>().DestroyIsOpen && */!FindObjectOfType<LevelManagerScript>().GameIsPaused)
        {
            float scrollData = Input.GetAxis("Mouse ScrollWheel");

            targetZoom -= scrollData * zoomFactor;
            float yVelocity = 0.0f;
            targetZoom = Mathf.Clamp(targetZoom, zoomMin, zoomMax);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomSpeed);
            cam.transform.position = ClampCamera(cam.transform.position);
            PanCamera();
        }
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

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;


        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;


        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
