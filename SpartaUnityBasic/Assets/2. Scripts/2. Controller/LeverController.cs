using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeverController : MonoBehaviour, IInterfactable
{
    [SerializeField] private string leverName;
    [SerializeField] private List<Transform> doorPos;
    [SerializeField] private GameObject colliderObject;
    [SerializeField] private GameObject privateRoomLight;

    private bool isOnLever = true;

    void Start()
    {
        LeverManager.Instance.AddLever(leverName, GetComponent<SpriteRenderer>());
        LeverManager.Instance.RegisterDoor(leverName, doorPos, colliderObject);
    }

    public void Interact()
    {
        isOnLever = !isOnLever;
        LeverManager.Instance.ToggleLever(leverName, isOnLever);
    }

    public void Eixt()
    {
    }
}