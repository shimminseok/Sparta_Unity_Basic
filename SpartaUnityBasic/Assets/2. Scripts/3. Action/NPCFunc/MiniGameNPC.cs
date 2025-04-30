using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : MonoBehaviour, INPCFunction
{
    [SerializeField] private MiniGameType miniGameType;


    public NPCFunction FuncType { get; }

    public void Initialize(NPCData _data)
    {
    }

    public void Execute()
    {
        Debug.Log("일단 Execute 했다잉");
    }
}