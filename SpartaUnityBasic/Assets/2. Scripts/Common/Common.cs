using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,
    Move,
}

public interface IState<T> where T : MonoBehaviour
{
    void Enter(T _owenr);
    void Execute(T owner);
    void Exit(T _owenr);
}
