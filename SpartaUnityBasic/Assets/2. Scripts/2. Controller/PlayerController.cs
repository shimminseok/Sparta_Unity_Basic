using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Commponent")]
    private StateMachine<PlayerController> stateMachine;

    private IState<PlayerController>[] states;

    private SpriteRenderer playerSpriteRenderer;
    public PlayerMoveController PlayerMoveController { get; private set; }
    public Animator             Animator             { get; private set; }

    private PlayerState currentState;

    private IInterfactable currentTarget;
    private RuntimeAnimatorController originAnimator;

    private void Awake()
    {
        PlayerMoveController = GetComponent<PlayerMoveController>();
        Animator = GetComponent<Animator>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        SetupState();
        originAnimator = Animator.runtimeAnimatorController;
    }

    void Update()
    {
        stateMachine?.OnUpdate();
        TryStateTransition();
        if (Input.GetKeyDown(KeyCode.F) && !UIDialogue.Instance.IsDialogueRunning)
        {
            currentTarget?.Interact();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            TransformTo();
        }
    }

    private void FixedUpdate()
    {
        stateMachine?.OnFixedUpdate();
    }

    private void SetupState()
    {
        states = new IState<PlayerController>[Enum.GetValues(typeof(PlayerState)).Length];
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = GetState((PlayerState)i);
        }

        stateMachine = new StateMachine<PlayerController>();
        stateMachine.Setup(this, states[(int)PlayerState.Idle]);
    }

    //팩토리패턴(디자인패턴)
    IState<PlayerController> GetState(PlayerState state)
    {
        return state switch
        {
            PlayerState.Idle => new IdleState(),
            PlayerState.Move => new MoveState(),
            _                => null
        };
    }

    private void TryStateTransition()
    {
        var next = states[(int)currentState].CheckTransition(this);
        if (next.HasValue && next.Value != currentState)
        {
            ChangeState(next.Value);
        }
    }

    public void ChangeState(PlayerState newState)
    {
        stateMachine.ChangeState(states[(int)newState]);
        currentState = newState;
    }

    public void TransformTo()
    {
        if (originAnimator != Animator.runtimeAnimatorController)
        {
            Animator.runtimeAnimatorController = originAnimator;
            PlayerMoveController.ChangeMoveSpeed(7);
            return;
        }

        var transformData = UITransform.Instance.SelectedTaransformData;


        if (transformData != null)
        {
            Animator.runtimeAnimatorController = transformData.AnimatorController;
            PlayerMoveController.ChangeMoveSpeed(transformData.Speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInterfactable>(out IInterfactable interfactable))
        {
            currentTarget = interfactable;
            UIHUD.Instance?.StartSwapInterfactorSprite();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInterfactable>(out IInterfactable interfactable))
        {
            if (currentTarget == interfactable)
            {
                currentTarget.Eixt();
                currentTarget = null;
                UIHUD.Instance?.StopSwapInterfactorSprite();
            }
        }
    }
}