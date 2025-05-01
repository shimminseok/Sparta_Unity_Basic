using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyBirdController : MonoBehaviour
{
    private readonly int Die = Animator.StringToHash("Die");

    private Animator animator;
    private Rigidbody2D rigidbody;

    public float flapForce = 6f;
    public float forwardSpeed = 3f;
    public bool isDead = false;
    private float deathCooldown = 0f;

    private bool isFlap = false;
    public bool godMode = false;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            if (deathCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    //게임 재시작
                }
            }
            else
            {
                deathCooldown -= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                isFlap = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        
        Vector2 velocity = rigidbody.velocity;

        velocity.x = forwardSpeed;

        if (isFlap)
        {
            velocity.y += flapForce;
            isFlap = false;
        }

        rigidbody.velocity = velocity;

        float angle = Mathf.Clamp((rigidbody.velocity.y * 10f), -90f, 90f);
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(godMode)
            return;
        
        if(isDead)
            return;

        animator.SetTrigger(Die);
        isDead = true;
        deathCooldown = 1f;    
    }

}
