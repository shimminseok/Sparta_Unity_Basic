using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransform : MonoBehaviour
{
    public static UITransform Instance { get; private set; }

    [SerializeField]
    private GameObject transformListUI;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
    }

    public void CreateTransformList()
    {
    }

    public void OpenTransformList()
    {
        transformListUI.SetActive(true);
    }

    public void CloseTransformList()
    {
        transformListUI.SetActive(false);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}