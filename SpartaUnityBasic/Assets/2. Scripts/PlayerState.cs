using UnityEngine;

public class IdleState : IState<PlayerController>
{
    public void OnEnter(PlayerController owenr)
    {
        Debug.Log("Idle 상태 진입");
    }

    public void OnUpdate(PlayerController owner)
    {
        owner.PlayerMoveController.HandleInput();
        if (owner.IsMouseMoving() || !owner.PlayerMoveController.IsKeyboardInputIdle())
            owner.ChangeState(PlayerState.Move);
    }

    public void OnFixedUpdate(PlayerController owner)
    {
    }

    public void OnExit(PlayerController owenr)
    {
    }
}


public class MoveState : IState<PlayerController>
{
    private bool isMouseMove = false;

    public void OnEnter(PlayerController owenr)
    {
        Debug.Log("Move 상태 진입");
        isMouseMove = owenr.IsMouseMoving();
        owenr.Animator.SetBool("IsMove", true);
    }

    public void OnUpdate(PlayerController owner)
    {
        owner.PlayerMoveController.HandleInput();
        owner.PlayerMoveController.HandleMove();
        if (isMouseMove)
        {
            if (owner.IsArrived())
                owner.ChangeState(PlayerState.Idle);
        }
        else
        {
            if (owner.PlayerMoveController.IsKeyboardInputIdle())
                owner.ChangeState(PlayerState.Idle);
        }
    }

    public void OnFixedUpdate(PlayerController owner)
    {
    }

    public void OnExit(PlayerController owenr)
    {
        owenr.Animator.SetBool("IsMove", false);
    }
}