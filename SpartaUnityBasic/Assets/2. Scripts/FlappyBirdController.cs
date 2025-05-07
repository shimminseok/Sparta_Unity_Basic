using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlappyBirdController : MonoBehaviour
{
    private readonly int Die = Animator.StringToHash("Die");

    private Animator animator;
    private Rigidbody2D rigid2D;

    public float flapForce = 6f;
    public float forwardSpeed = 3f;
    public bool isDead = false;

    private bool isFlap = false;
    public bool godMode = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            isFlap = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        Vector2 velocity = rigid2D.velocity;

        velocity.x = forwardSpeed;

        if (isFlap)
        {
            velocity.y += flapForce;
            isFlap = false;
        }

        rigid2D.velocity = velocity;

        float angle = Mathf.Clamp((rigid2D.velocity.y * 10f), -90f, 90f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isDead)
            return;

        if (other.gameObject.CompareTag("Obstacle"))
        {
            FlappyBirdGameManager.Instance?.AddScore(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (godMode)
            return;

        if (isDead)
            return;


        animator.SetTrigger(Die);
        isDead = true;
        if (FlappyBirdGameManager.Instance != null)
            FlappyBirdGameManager.Instance.GameOver();
    }
}