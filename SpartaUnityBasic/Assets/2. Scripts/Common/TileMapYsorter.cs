using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapYsorter : MonoBehaviour
{
    [SerializeField] private Tilemap renderTilemap;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int orderOffset = 0;

    private void LateUpdate()
    {
        Vector3Int cellPos = renderTilemap.WorldToCell(transform.position);
        spriteRenderer.sortingOrder = Mathf.Clamp(-cellPos.y * 10, 1, 201);
    }
}