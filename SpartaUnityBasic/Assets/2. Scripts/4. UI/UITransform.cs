using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UITransform : UIBase
{
    public static UITransform Instance { get; private set; }


    [SerializeField] private TransformListSlot transformListSlotPrefabs;
    [SerializeField] private Transform tranformListSlotRoot;
    public TransformData SelectedTaransformData { get; private set; }

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    void Start()
    {
        CreateTransformListSlot();
    }

    public void CreateTransformListSlot()
    {
        foreach (var data in TableManager.Instance.GetTable<TransformTable>().dataDic.Values)
        {
            var slot = Instantiate(transformListSlotPrefabs, tranformListSlotRoot);
            slot.SetTransformIconSlot(data);
        }
    }

    public void SelectTransform(TransformData data)
    {
        SelectedTaransformData = data;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}