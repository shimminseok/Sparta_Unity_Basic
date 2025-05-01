using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum PlayerState
{
    Idle,
    Move,
}

public enum MiniGameType
{
    None = -1,
    FlappyBird,
}

[Flags]
public enum NPCFunction
{
    None = 0,
    MiniGame = 1 << 0,
    Story = 1 << 1
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
    void Eixt();
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

    [Header("Function")]
    public NPCFunction NPCFunction;

    public bool HasFunction(NPCFunction _func)
    {
        return (NPCFunction & _func) == _func;
    }
}

[Serializable]
public class MiniGameData
{
    public int MiniGameID;
    public string MiniGameName;

    public Sprite MiniGameIcon;
    public MiniGameType MiniGameType;
}

[Serializable]
public class TransformData
{
    public int TransformID;
    public Sprite TransformIcon;
    public RuntimeAnimatorController AnimatorController;
    public int Speed;
}