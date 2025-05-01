using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Tilemap tilemap;
    private float camHalfWidth;
    private float camHalfHeight;

    private Bounds mapBounds;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        camHalfHeight = mainCamera.orthographicSize;
        camHalfWidth = camHalfHeight * mainCamera.aspect;
        mapBounds = tilemap.localBounds;
    }

// 오클루젼 컬링 - 카메라와 오브젝트에 가려져있는걸 비활성화
// 절두체   컬링 - 카메라 밖에 있는것만 비활성화
//LOD
    private void Update()
    {
        HandleZoom();
    }

    private void LateUpdate()
    {
        if (target == null) return;


        Vector3 desiredPosition = target.position;

        float minX = mapBounds.min.x + camHalfWidth;
        float maxX = mapBounds.max.x - camHalfWidth;

        float minY = mapBounds.min.y + camHalfHeight;
        float maxY = mapBounds.max.y - camHalfHeight;

        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scroll * 5f, 3f, 8f);
            camHalfHeight = mainCamera.orthographicSize;
            camHalfWidth = camHalfHeight * mainCamera.aspect;
        }
    }
}