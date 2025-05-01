using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour, IInterfactable
{
    [SerializeField] private int npcID;

    public string  Name    { get; private set; }
    public NPCData NpcData { get; private set; }
    private INPCFunction npcFunction;

    private void Awake()
    {
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        NpcData = TableManager.Instance.GetTable<NPCTable>()?.GetDataByID(npcID);
        Name = NpcData.Name;
        AddNPCComponents();
    }

    void AddNPCComponents()
    {
        var functionType = Enum.GetValues(typeof(NPCFunction));
        foreach (NPCFunction func in functionType)
        {
            if (NpcData.HasFunction(func))
            {
                AddComponentForFunction(func);
            }
        }
    }

    void AddComponentForFunction(NPCFunction func)
    {
        Type componentType = GetComponentTypeForFunction(func);
        if (componentType != null && !gameObject.TryGetComponent(componentType, out _))
        {
            var component = gameObject.AddComponent(componentType);
            if (component is INPCFunction npcComponent)
            {
                npcComponent.Initialize(NpcData);
                npcFunction = npcComponent;
            }
        }
    }

    private Type GetComponentTypeForFunction(NPCFunction function)
    {
        return function switch
        {
            NPCFunction.MiniGame => typeof(MiniGameNPC),
            NPCFunction.Story    => typeof(StoryNPC),
            _                    => null,
        };
    }

    public void Interact()
    {
        npcFunction?.Execute();
    }

    public void Eixt()
    {
        if (UIDialogue.Instance != null)
        {
            if (UIDialogue.Instance.IsDialogueRunning)
                UIDialogue.Instance.Close();
        }
    }
}