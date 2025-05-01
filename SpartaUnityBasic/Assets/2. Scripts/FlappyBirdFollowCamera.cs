using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdFollowCamera : MonoBehaviour
{
    public Transform target;
    private float offsetX;
    void Start()
    {
        if (target == null)
        {
            return;
        }
        
        offsetX = transform.position.x - target.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.x = target.position.x + offsetX;
        transform.position = pos;
    }
}
