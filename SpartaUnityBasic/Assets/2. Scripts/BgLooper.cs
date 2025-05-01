using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBgCount = 5;
    public int obstacleCount = 0;
    public Vector2 obstacleLastPosition = Vector2.zero;
    void Start()
    {
        Obstacle[] obstacles = GameObject.FindObjectsOfType<Obstacle>();
        obstacleLastPosition = obstacles[0].transform.position;
        obstacleCount = obstacles.Length;

        for (int i = 0; i < obstacleCount; i++)
        {
            obstacleLastPosition = obstacles[i].SetRanmdonPlace(obstacleLastPosition,obstacleCount);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BackGround"))
        {
            float   widthOfBgObject = ((BoxCollider2D)other).size.x - 0.1f;
            Vector2 pos             = other.transform.position;

            pos.x += widthOfBgObject * numBgCount;
            other.transform.position = pos;
            return;
        }
        Obstacle obstacle = other.GetComponent<Obstacle>();
        if (obstacle)
        {
            obstacleLastPosition = obstacle.SetRanmdonPlace(obstacleLastPosition, obstacleCount);
        }
    }
}
