using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameNPC : MonoBehaviour, INPCFunction
{
    public NPCFunction FuncType { get; }
    private NPCData npcData;

    public void Initialize(NPCData data)
    {
        npcData = data;
    }

    public void Execute()
    {
        UIDialogue.Instance.StartDefaultDialogue(npcData.DefaultDialogues, UIMinigamePanel.Instance.Open);
    }
}