using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogue : UIBase
{
    private UIDialogue intance;

    public static UIDialogue Instance;

    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI dialogueText;


    private bool isDialogueRunning;
    private bool isInputNextDialogue;

    private Coroutine currentCoroutine;


    protected override void Awake()
    {
        base.Awake();
        if (intance == null)
            Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInputNextDialogue = true;
        }
    }

    void ResetDescription()
    {
        dialogueText.text = string.Empty;
        StopAllCoroutines();
    }

    public void StartDefaultDialogue(NPCData data)
    {
        
        ResetDescription();
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        npcName.text = data.Name;
        Open();
        currentCoroutine = StartCoroutine(StartDialogue(data.DefaultDialogues));
    }

    private IEnumerator StartDialogue(List<string> desc, Action onDialogueComplete = null)
    {
        isDialogueRunning = true;
        for (int i = 0; i < desc.Count; i++)
        {
            dialogueText.text = desc[i];
            yield return new WaitUntil(() => isInputNextDialogue);
            isInputNextDialogue = false;
        }

        onDialogueComplete?.Invoke();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
        ResetDescription();
        isDialogueRunning = false;
    }
}