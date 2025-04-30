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


    public int NPCID => npcID;

    private void Awake()
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
            if (HasFunction(NpcData.NPCFunctions, func))
            {
                AddComponentForFunction(func);
            }
        }
    }

    bool HasFunction(NPCFunction npcFunctions, NPCFunction functionToCheck)
    {
        return (npcFunctions & functionToCheck) == functionToCheck;
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
            _                    => null,
        };
    }

    public void Interact()
    {
        npcFunction?.Execute();
    }
}