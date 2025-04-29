using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Start()
    {
    }

    void Update()
    {
    }

    private void LateUpdate()
    {
        transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, transform.position.z);
    }
}