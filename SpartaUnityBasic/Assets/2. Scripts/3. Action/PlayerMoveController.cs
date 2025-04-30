using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveController : MonoBehaviour
{
    public enum MoveType
    {
        None,
        Mouse,
        Keyboard
    }


    public MoveType CurrentMoveType { get; private set; } = MoveType.None;

    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rigidbody2D;
    private SpriteRenderer playerSpriteRenderer;
    private Camera mainCamera;

    private Vector2 targetPosition;
    private Vector2 keyboardInput;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }


    public void HandleInput()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // 키보드 입력 우선 처리
        if (inputX != 0 || inputY != 0)
        {
            CurrentMoveType = MoveType.Keyboard;
            keyboardInput = new Vector2(inputX, inputY).normalized;
            targetPosition = transform.localPosition;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            TrySetMouseTarget();
        }
        else if (CurrentMoveType == MoveType.Keyboard)
        {
            CurrentMoveType = MoveType.None;
        }
    }

    private void TrySetMouseTarget()
    {
        Ray          ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            targetPosition = hit.point;
            CurrentMoveType = MoveType.Mouse;
        }
    }

    private Vector2 GetMoveDirection()
    {
        switch (CurrentMoveType)
        {
            case MoveType.Keyboard:
                return keyboardInput;
            case MoveType.Mouse:
                Vector2 dir = (targetPosition - rigidbody2D.position).normalized;
                if (Vector2.Distance(rigidbody2D.position, targetPosition) < 0.1f)
                    CurrentMoveType = MoveType.None;
                return dir;
            default:
                return Vector2.zero;
        }
    }

    public void HandleMove()
    {
        Vector2 moveDirection = GetMoveDirection();
        if (moveDirection.x != 0)
            playerSpriteRenderer.flipX = moveDirection.x < 0;

        Vector2 nextPosition = rigidbody2D.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(nextPosition);
    }

    public bool IsArrived()           => Vector2.Distance(transform.position, targetPosition) < 0.1f;
    public bool IsKeyboardInputIdle() => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;
}