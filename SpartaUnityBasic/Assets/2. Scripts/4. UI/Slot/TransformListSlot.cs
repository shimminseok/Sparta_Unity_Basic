using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransformListSlot : MonoBehaviour
{
    [SerializeField] private Image transformIcon;
    [SerializeField] private Toggle cheackToggle;

    TransformData transformData;

    private void Start()
    {
        cheackToggle.group = GetComponentInParent<ToggleGroup>();
    }

    public void SetTransformIconSlot(TransformData data)
    {
        transformData = data;
        transformIcon.sprite = transformData.TransformIcon;
    }

    public void OnClickSlot()
    {
        UITransform.Instance.SelectTransform(cheackToggle.isOn ? transformData : null);
    }
}