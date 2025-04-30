using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,
    Move,
}

public enum MiniGameType
{
    TheStack,
}

public enum NPCFunction
{
    MiniGame,
}

public interface ITable
{
    public          Type Type { get; }
    public abstract void CreateTable();
}

public interface IState<T> where T : MonoBehaviour
{
    void OnEnter(T _owenr);
    void OnUpdate(T owner);
    void OnFixedUpdate(T owner);
    void OnExit(T _owenr);

    PlayerState? CheckTransition(T owner);
}

public interface IInterfactable
{
    void Interact();
}

public interface INPCFunction
{
    NPCFunction FuncType { get; }
    void        Initialize(NPCData data);
    void        Execute();
}

[Serializable]
public class DoorData
{
    public List<Vector3Int> tilePos;
    public GameObject colliderObj;
}

[Serializable]
public class NPCData
{
    public int NPCID;
    public string Name;
    public List<string> DefaultDialogues;

    [Header("Function")] public NPCFunction NPCFunctions;

    public bool HasFunction(NPCFunction _func)
    {
        return (NPCFunctions & _func) == _func;
    }
}