using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController))]
[RequireComponent(typeof(StateMachine<PlayerController>))]
[RequireComponent(typeof(Animator))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Commponent")] private StateMachine<PlayerController> stateMachine;
    public PlayerMoveController PlayerMoveController { get; private set; }
    public Animator             Animator             { get; private set; }

    private IState<PlayerController>[] states;
    private PlayerState currentState;

    private IInterfactable currentTarget;

    private void Awake()
    {
        PlayerMoveController = GetComponent<PlayerMoveController>();
        Animator = GetComponent<Animator>();

        SetupState();
    }

    void Start()
    {
    }

    void Update()
    {
        stateMachine?.OnUpdate();
        TryStateTransition();

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentTarget?.Interact();
        }
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
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

    public bool IsMouseMoving()
    {
        return PlayerMoveController.CurrentMoveType == PlayerMoveController.MoveType.Mouse;
    }

    public bool IsArrived()
    {
        return PlayerMoveController.IsArrived();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInterfactable>(out IInterfactable interfactable))
        {
            currentTarget = interfactable;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInterfactable>(out IInterfactable interfactable))
        {
            if (currentTarget == interfactable)
                currentTarget = null;
        }
    }
}