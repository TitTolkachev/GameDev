using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    public Transform playerTransform;
    public float movingSpeed;
    
    private Camera cam;

    public float mapMinX = -30, mapMaxX = 30, mapMinY = -15, mapMaxY = 15;

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

    void Start()
    {
        cam = Camera.main;
        transform.position = new(playerTransform.transform.position.x, playerTransform.transform.position.y, playerTransform.transform.position.z - 10);
    }

    void Update()
    {
        Vector3 target = new(playerTransform.transform.position.x, playerTransform.transform.position.y, playerTransform.transform.position.z - 10);

        Vector3 pos = ClampCamera(Vector3.Lerp(transform.position, target, movingSpeed * Time.deltaTime));

        transform.position = pos;
    }
}
