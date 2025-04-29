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

    private Rigidbody2D rigidbody2D;
    public MoveType CurrentMoveType { get; set; } = MoveType.None;

    [SerializeField] private float moveSpeed = 5f;
    private Vector2 targetPosition;

    private Vector2 keyboardInput;
    Camera mainCamera;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
            Ray     ray    = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector2 origin = ray.origin;
            Vector2 dir    = ray.direction;

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, Mathf.Infinity);
            if (hit.collider != null)
            {
                targetPosition = hit.point;
                CurrentMoveType = MoveType.Mouse;
            }
        }
    }

    public void HandleMove()
    {
        Vector2 moveDirection = Vector2.zero;
        switch (CurrentMoveType)
        {
            case MoveType.Keyboard:
                moveDirection = keyboardInput.normalized;
                break;
            case MoveType.Mouse:
                Vector2 currentPos = rigidbody2D.position;
                Vector2 targetDir  = (targetPosition - currentPos).normalized;
                moveDirection = targetDir;

                if (Vector2.Distance(currentPos, targetPosition) < 0.1f)
                {
                    CurrentMoveType = MoveType.None;
                }

                break;
        }

        Vector2 nextPos = rigidbody2D.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigidbody2D.MovePosition(nextPos);
    }

    public bool IsArrived()
    {
        return Vector2.Distance(transform.position, targetPosition) < 0.1f;
    }

    public bool IsKeyboardInputIdle()
    {
        return Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;
    }
}