using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class SpaceGrid : MonoBehaviour
{
    public TileBase Tile;
    public Tilemap MovementGrid;
    public Tilemap DetectionGrid;
    public Color DetectionGridActiveColor;
    public Color DetectionGridInactiveColor;

    private void Awake()
    {
        transform.parent = FindObjectOfType<Grid>().transform;
    }

    public void OnUpdateMovementGrid(Vector3 position, int radius, bool isEnemy)
    {
        Debug.Log("Show Movement Radius!");
        MovementGrid.ClearAllTiles();
        Vector3Int playerPositionInGrid = WorldToCell(position);
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
            MovementGrid.SetTile(point, Tile);
        }
    }

    public void OnUpdateDetectionGridActive(Vector3 position, int radius)
    {
        DetectionGrid.color = DetectionGridActiveColor;
        OnUpdateDetectionGrid(position, radius);
    }

    public void OnUpdateDetectionGridInactive(Vector3 position, int radius)
    {
        DetectionGrid.color = DetectionGridInactiveColor;
        OnUpdateDetectionGrid(position, radius);
    }

    public void OnUpdateDetectionGrid(Vector3 position, int radius)
    {
        DetectionGrid.ClearAllTiles();
        Vector3Int playerPositionInGrid = WorldToCell(position);
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

                if (GameLogic.Instance.IsWalkableTile(pointUp, true))
                {
                    pointSet.Add(pointUp);
                }
                if (GameLogic.Instance.IsWalkableTile(pointLeft, true))
                {
                    pointSet.Add(pointLeft);
                }
                if (GameLogic.Instance.IsWalkableTile(pointDown, true))
                {
                    pointSet.Add(pointDown);
                }
                if (GameLogic.Instance.IsWalkableTile(pointRight, true))
                {
                    pointSet.Add(pointRight);
                }
            }

            radius--;
        }

        foreach (Vector3Int point in pointSet)
        {
            DetectionGrid.SetTile(point, Tile);
        }
        DetectionGrid.CompressBounds();
    }

    public Vector2 GetCellCenterWorld(Vector3Int cellIndex)
    {
        return MovementGrid.GetCellCenterWorld(cellIndex);
    }

    public Vector2 GetCellSize()
    {
        return MovementGrid.cellSize;
    }

    public void OnHideMovementGrid()
    {
        MovementGrid.ClearAllTiles();
    }
    
    public Vector3Int GetGridCelll(Vector3 position)
    {
        return MovementGrid.WorldToCell(position);
    }


    public Vector3 GetGridCenterPosition(Vector3Int position)
    {
        return MovementGrid.CellToWorld(position) + 0.5f * MovementGrid.cellSize;
    }

    public bool IsWalkable(Vector3Int cellIndex)
    {
        return MovementGrid.GetTile(cellIndex) != null;
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        return MovementGrid.WorldToCell(position);
    }

    public Vector3 CellToWorld(Vector3Int cellIndex)
    {
        return MovementGrid.CellToWorld(cellIndex);
    }

    public bool IsCellInDetectionRadius(Vector3Int playerCell)
    {
        return DetectionGrid.GetTile(playerCell) != null;
    }
}
