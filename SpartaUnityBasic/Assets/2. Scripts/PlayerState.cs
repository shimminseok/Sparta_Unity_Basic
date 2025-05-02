using UnityEngine;

//FSM (유한상태머신) 
public class IdleState : IState<PlayerController>
{
    public void OnEnter(PlayerController owenr)
    {
        //대기상태
    }

    public void OnUpdate(PlayerController owner)
    {
        owner.PlayerMoveController.HandleInput();
    }

    public void OnFixedUpdate(PlayerController owner)
    {
    }

    public void OnExit(PlayerController owenr)
    {
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        var moveType = owner.PlayerMoveController.CurrentMoveType;
        if (moveType != PlayerMoveController.MoveType.None)
            return PlayerState.Move;


        return null;
    }
}


public class MoveState : IState<PlayerController>
{
    private readonly int MoveHash = Animator.StringToHash("IsMove");

    public void OnEnter(PlayerController owenr)
    {
        owenr.Animator.SetBool(MoveHash, true);
    }

    public void OnUpdate(PlayerController owner)
    {
        owner.PlayerMoveController.HandleInput();
    }

    public void OnFixedUpdate(PlayerController owner)
    {
        owner.PlayerMoveController.HandleMove();
    }

    public void OnExit(PlayerController owenr)
    {
        owenr.Animator.SetBool(MoveHash, false);
    }

    public PlayerState? CheckTransition(PlayerController owner)
    {
        if (owner.PlayerMoveController.CurrentMoveType == PlayerMoveController.MoveType.None)
            return PlayerState.Idle;

        return null;
    }
}