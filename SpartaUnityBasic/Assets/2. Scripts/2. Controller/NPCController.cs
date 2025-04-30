using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCController : MonoBehaviour, IInterfactable
{
    [SerializeField] private int npcID;

    public string Name { get; private set; }
    private NPCData npcData;

    private INPCFunction npcFunction;
    private BoxCollider2D boxCollider2D;


    public int NPCID => npcID;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        Init();
    }


    void Start()
    {
    }

    void Update()
    {
    }

    public void Init()
    {
        npcData = TableManager.Instance.GetTable<NPCTable>()?.GetDataByID(npcID);
        Name = npcData.Name;

        AddNPCComponents();
    }

    void AddNPCComponents()
    {
        var functionType = Enum.GetValues(typeof(NPCFunction));
        foreach (NPCFunction func in functionType)
        {
            if (HasFunction(npcData.NPCFunctions, func))
            {
                AddComponentForFunction(func, npcData);
            }
        }
    }

    bool HasFunction(NPCFunction npcFunctions, NPCFunction functionToCheck)
    {
        return (npcFunctions & functionToCheck) == functionToCheck;
    }

    void AddComponentForFunction(NPCFunction _func, NPCData _npcData)
    {
        Type componentType = GetComponentTypeForFunction(_func);
        if (componentType != null && !gameObject.TryGetComponent(componentType, out _))
        {
            var component = gameObject.AddComponent(componentType);
            if (component is INPCFunction npcComponent)
            {
                npcComponent.Initialize(npcData);
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