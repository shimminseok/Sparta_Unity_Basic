using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNPC : MonoBehaviour, INPCFunction
{
    public NPCFunction FuncType { get; }
    private NPCData npcData;

    public void Initialize(NPCData data)
    {
        npcData = data;
    }

    public void Execute()
    {
        bool canTalk = GameManager.Instance.StoryProgress >= npcData.NPCID - 1;


        if (!canTalk)
        {
            UIDialogue.Instance.StartDefaultDialogue(new() { "이전 스토리를 듣고와야지 집중이 잘 되지 않겠어?!" });
            return;
        }

        UIDialogue.Instance.StartDefaultDialogue(npcData.DefaultDialogues);

        if (GameManager.Instance.StoryProgress == npcData.NPCID - 1)
            GameManager.Instance.StoryProgress++;
    }
}