using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveController : MonoBehaviour
{
    public enum MoveType
    {
        None,
        Mouse,
        Keyboard,
        AStar
    }


    public MoveType CurrentMoveType { get; private set; } = MoveType.None;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    private Rigidbody2D rigid;
    private Camera mainCamera;

    private Vector2 targetPosition;
    private Vector2 keyboardInput;

    private bool isPathMoving = false;

    private Coroutine moveRoutine;
    private List<Vector2Int> lastPath = new List<Vector2Int>();

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        HashSet<Vector2Int> wallSet = new HashSet<Vector2Int>();

        TileMapAStar.SetWallData(wallTilemap);
    }

    public void HandleInput()
    {
        //UI를했냐
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        //키보드입력
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // 키보드 입력 우선 처리
        if (inputX != 0 || inputY != 0)
        {
            if (moveRoutine != null)
                StopCoroutine(moveRoutine);
            isPathMoving = false;
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
        // Ray          ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        // RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        // if (hit.collider != null)
        // {
        //     targetPosition = hit.point;
        //     CurrentMoveType = MoveType.Mouse;
        // }


        Vector3    worldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos  = floorTilemap.WorldToCell(worldPos);

        Vector3Int starToCell = floorTilemap.WorldToCell(rigid.position); //내 시작 좌표 안정화(rigid.pos로 하면 셀 경계에 있을때 애매해짐

        Vector2Int start = new Vector2Int(starToCell.x, starToCell.y);
        Vector2Int end   = new Vector2Int(cellPos.x, cellPos.y);

        List<Vector2Int> path = TileMapAStar.FindPath(start, end);
        if (path == null || path.Count == 0)
            return;

        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        lastPath = path;
        CurrentMoveType = MoveType.AStar;
        moveRoutine = StartCoroutine(FollowPath(path));
        isPathMoving = true;
    }

    IEnumerator FollowPath(List<Vector2Int> path)
    {
        foreach (Vector2Int point in path)
        {
            Vector2 target = floorTilemap.GetCellCenterWorld(new Vector3Int(point.x, point.y, 0));

            while (Vector2.Distance(rigid.position, target) > moveSpeed * Time.fixedDeltaTime)
            {
                Vector2 dir = (target - rigid.position).normalized;
                rigid.MovePosition(rigid.position + dir * moveSpeed * Time.fixedDeltaTime);

                if (dir.x != 0)
                    playerSpriteRenderer.flipX = dir.x < 0;

                yield return new WaitForFixedUpdate();
            }

            rigid.MovePosition(target);
        }

        CurrentMoveType = MoveType.None;
        isPathMoving = false;
    }

    private void OnDrawGizmos()
    {
        if (lastPath.Count == 0)
            return;

        Gizmos.color = Color.red;
        foreach (Vector2Int point in lastPath)
        {
            Vector3 worldPos = floorTilemap.GetCellCenterWorld(new Vector3Int(point.x, point.y, 0));
            Gizmos.DrawSphere(worldPos, 0.15f);
        }
    }

    private Vector2 GetMoveDirection()
    {
        switch (CurrentMoveType)
        {
            case MoveType.Keyboard:
                return keyboardInput;
            case MoveType.Mouse:
                Vector2 dir = (targetPosition - rigid.position).normalized;
                if (Vector2.Distance(rigid.position, targetPosition) < 0.1f)
                    CurrentMoveType = MoveType.None;
                return dir;
            default:
                return Vector2.zero;
        }
    }

    public void HandleMove()
    {
        if (CurrentMoveType != MoveType.Keyboard)
            return;

        Vector2 moveDirection = GetMoveDirection();
        if (moveDirection.x != 0)
            playerSpriteRenderer.flipX = moveDirection.x < 0;

        Vector2 nextPosition = rigid.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(nextPosition);
    }

    public void ChangeMoveSpeed(float speed) => moveSpeed = speed;

    public bool IsArrived()           => Vector2.Distance(transform.position, targetPosition) < 0.1f;
    public bool IsKeyboardInputIdle() => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;
}