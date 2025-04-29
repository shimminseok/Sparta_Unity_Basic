using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    T ownerEntity;
    IState<T> currentState;

    public void Setup(T owner, IState<T> entryState)
    {
        ownerEntity = owner;
        ChangeState(entryState);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void OnUpdate()
    {
        if (currentState != null)
        {
            currentState.OnUpdate(ownerEntity);
        }
    }

    public void OnFixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnFixedUpdate(ownerEntity);
        }
    }

    public void ChangeState(IState<T> newState)
    {
        if (newState == null)
            return;


        if (currentState != null)
        {
            currentState.OnExit(ownerEntity);
        }

        currentState = newState;
        currentState.OnEnter(ownerEntity);
    }
}