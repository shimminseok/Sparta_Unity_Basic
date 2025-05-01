using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformManager : MonoBehaviour
{
    public static TransformManager Instance { get; private set; }

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

    public void TransformTo()
    {
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}