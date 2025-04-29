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
    void OnEnter(T _owenr);
    void OnUpdate(T owner);
    void OnFixedUpdate(T owner);
    void OnExit(T _owenr);
}