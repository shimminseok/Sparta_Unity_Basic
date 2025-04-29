using System;
using System.Collections;
using System.Collections.Generic;
using PlayerStates;
using UnityEngine;

[RequireComponent(typeof(PlayerMoveController))]
[RequireComponent(typeof(StateMachine<PlayerController>))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Commponent")]
    private StateMachine<PlayerController> stateMachine;
    public PlayerMoveController PlayerMoveController {get; private set;}
    public Animator Animator { get; private set; }

    private IState<PlayerController>[] states;
    private PlayerState currentState;
    
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
        stateMachine.Excute();
    }

    private void FixedUpdate()
    {
        
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
            _ => null
        };
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

}
