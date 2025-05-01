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

    private void Start()
    {
    }


    private void OnDestroy()
    {
        Instance = null;
    }
}