using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LeverManager : MonoBehaviour
{
    public static LeverManager Instance { get; private set; }
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Sprite leverOnTile;

    [SerializeField] private Sprite leverOffTile;

    [SerializeField] private List<Tile> doorOpenTiles = new();
    [SerializeField] private List<Tile> doorClosedTiles = new();

    private Dictionary<string, SpriteRenderer> leverTileMap = new Dictionary<string, SpriteRenderer>();
    private Dictionary<string, DoorData> leverToDoors = new Dictionary<string, DoorData>();


    private void Awake()
    {
        Instance = this;
    }

    public void AddLever(string leverName, SpriteRenderer renderer)
    {
        leverTileMap[leverName] = renderer;
    }

    public void RegisterDoor(string leverId, List<Transform> worldPositions, GameObject colliderObject)
    {
        var tilePositions = new List<Vector3Int>();
        foreach (var pos in worldPositions)
        {
            tilePositions.Add(tilemap.WorldToCell(pos.position));
        }

        if (!leverToDoors.ContainsKey(leverId))
            leverToDoors[leverId] = (new DoorData()
            {
                tilePos = tilePositions,
                colliderObj = colliderObject,
            });
    }

    public void ToggleLever(string leverName, bool isOpen)
    {
        if (leverTileMap.TryGetValue(leverName, out var renderer))
        {
            renderer.sprite = isOpen ? leverOnTile : leverOffTile;
        }

        if (leverToDoors.TryGetValue(leverName, out var door))
        {
            for (int i = 0; i < door.tilePos.Count; i++)
            {
                tilemap.SetTile(door.tilePos[i], isOpen ? doorOpenTiles[i] : doorClosedTiles[i]);
            }

            door.colliderObj.SetActive(!isOpen);
        }
    }
}