using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : MonoBehaviour, INPCFunction
{
    [SerializeField] private MiniGameType miniGameType;

    private NPCData npcData;
    public NPCFunction FuncType { get; }

    public void Initialize(NPCData data)
    {
        npcData = data;
    }

    public void Execute()
    {
        UIDialogue.Instance.StartDefaultDialogue(npcData);
    }
}