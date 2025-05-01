using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIDialogue : UIBase
{
    private UIDialogue intance;

    public static UIDialogue Instance;

    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI dialogueText;


    private bool isInputNextDialogue;

    private Coroutine currentCoroutine;
    public bool IsDialogueRunning { get; private set; }


    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    void ResetDescription()
    {
        dialogueText.text = string.Empty;
        StopAllCoroutines();
        currentCoroutine = null;
    }


    public void StartDefaultDialogue(List<string> dialogue, Action onDialogueComplete = null)
    {
        ResetDescription();
        Open();
        currentCoroutine = StartCoroutine(StartDialogue(dialogue, onDialogueComplete));
    }

    private IEnumerator StartDialogue(List<string> desc, Action onDialogueComplete = null)
    {
        IsDialogueRunning = true;
        for (int i = 0; i < desc.Count; i++)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
            dialogueText.text = desc[i];
            yield return null; //한프레임 쉬도록
        }

        yield return null; //한프레임 쉬도록
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
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
        IsDialogueRunning = false;
        UIMinigamePanel.Instance.Close();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}