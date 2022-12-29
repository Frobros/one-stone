using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class MovementGrid : MonoBehaviour
{
    public TileBase tile;
    private Tilemap tilemap;
    private void Awake()
    {
        this.tilemap = GetComponent<Tilemap>();
        this.transform.parent = FindObjectOfType<Grid>().transform;
    }

    public void OnShowGrid(Vector3 position, int radius, bool isEnemy)
    {
        this.tilemap.gameObject.SetActive(true);
        this.tilemap.ClearAllTiles();
        Vector3Int playerPositionInGrid = tilemap.WorldToCell(position);
        HashSet<Vector3Int> pointSet = new HashSet<Vector3Int>();
        pointSet.Add(playerPositionInGrid);

        while (radius != 0)
        {
            List<Vector3Int> temp = new List<Vector3Int>(pointSet);
            foreach (Vector3Int point in temp)
            {
                Vector3Int pointRight = new Vector3Int(point.x + 1, point.y, point.z);
                Vector3Int pointLeft = new Vector3Int(point.x - 1, point.y, point.z);
                Vector3Int pointUp = new Vector3Int(point.x, point.y + 1, point.z);
                Vector3Int pointDown = new Vector3Int(point.x, point.y - 1, point.z);

                if (GameLogic.Instance.IsWalkableTile(pointUp, isEnemy))
                {
                    pointSet.Add(pointUp);
                }
                if (GameLogic.Instance.IsWalkableTile(pointLeft, isEnemy))
                {
                    pointSet.Add(pointLeft);
                }
                if (GameLogic.Instance.IsWalkableTile(pointDown, isEnemy))
                {
                    pointSet.Add(pointDown);
                }
                if (GameLogic.Instance.IsWalkableTile(pointRight, isEnemy))
                {
                    pointSet.Add(pointRight);
                }
            }

            radius--;
        }

        foreach (Vector3Int point in pointSet)
        {
            this.tilemap.SetTile(point, tile);
        }
    }

    public Vector2 GetCellCenterWorld(Vector3Int cellIndex)
    {
        return this.tilemap.GetCellCenterWorld(cellIndex);
    }

    public Vector2 GetCellSize()
    {
        return this.tilemap.cellSize;
    }

    public void HideGrid()
    {
        this.tilemap.gameObject.SetActive(false);
    }
    
    public Vector3Int GetGridCelll(Vector3 position)
    {
        return this.tilemap.WorldToCell(position);
    }


    public Vector3 GetGridCenterPosition(Vector3Int position)
    {
        return this.tilemap.CellToWorld(position) + 0.5f * tilemap.cellSize;
    }

    public bool IsWalkable(Vector3Int cellIndex)
    {
        return this.tilemap.GetTile(cellIndex) != null;
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return this.tilemap.WorldToCell(position);
    }

    public Vector3 CellToWorld(Vector3Int cellIndex)
    {
        return this.tilemap.CellToWorld(cellIndex);
    }

    public bool IsCellInGrid(Vector3Int playerCell)
    {
        return this.tilemap.GetTile(playerCell) != null;
    }
}
