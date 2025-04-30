using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LeverManager : Singleton<LeverManager>
{
    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile leverOnTile;

    [SerializeField] private Tile leverOffTile;

    [SerializeField] private List<Tile> doorOpenTiles = new();
    [SerializeField] private List<Tile> doorClosedTiles = new();

    private Dictionary<string, Vector3Int> leverTileMap = new Dictionary<string, Vector3Int>();
    private Dictionary<string, DoorData> leverToDoors = new Dictionary<string, DoorData>();


    public void AddLever(string leverName, Vector3 position)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(position);
        leverTileMap[leverName] = tilePosition;
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
        if (leverTileMap.TryGetValue(leverName, out var leverPosition))
            tilemap.SetTile(leverPosition, isOpen ? leverOnTile : leverOffTile);

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