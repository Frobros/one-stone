using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    public TileBase tile;
    private Tilemap tilemap;
    public bool isEnemy;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void OnShowGrid(Vector3 position, int radius, bool isEnemy)
    {
        tilemap.gameObject.SetActive(true);
        tilemap.ClearAllTiles();
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
            tilemap.SetTile(point, tile);
        }
    }

    internal void HideGrid()
    {
        tilemap.gameObject.SetActive(false);
    }
    
    public Vector3Int GetGridCelll(Vector3 position)
    {
        return tilemap.WorldToCell(position);
    }


    public Vector3 GetGridCenterPosition(Vector3Int position)
    {
        return tilemap.CellToWorld(position) + 0.5f * tilemap.cellSize;
    }

    internal bool IsWalkable(Vector3Int tileCell)
    {
        return tilemap.GetTile(tileCell) != null;
    }
}
