using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomController : MonoBehaviour
{
    [SerializeField] private GameObject darkOverlay;
    [SerializeField] private GameObject roomLight;


    private void OnTriggerEnter2D(Collider2D other)
    {
        darkOverlay.SetActive(true);
        roomLight.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        darkOverlay.SetActive(false);
        roomLight.SetActive(false);
    }
}