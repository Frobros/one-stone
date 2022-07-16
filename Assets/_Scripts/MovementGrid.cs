using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class MovementGrid : MonoBehaviour
{
    public TileBase tile;
    private Tilemap tilemap;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void OnShowGrid(Vector3 position, int radius)
    {
        tilemap.ClearAllTiles();
        Vector3Int positionInGrid = tilemap.WorldToCell(position);
        for (int i = positionInGrid.x - radius; i < position.x + radius; i++)
        {
            for (int j = positionInGrid.y - radius; j < position.y + radius; j++)
            {
                Vector3Int currentGridPosition = new Vector3Int(i, j, 0);
                float magnitude = (GetGridCenterPosition(currentGridPosition) - position).magnitude;
                if (LevelManager.Instance.IsWalkableTile(currentGridPosition) && magnitude <= radius)
                {
                    tilemap.SetTile(currentGridPosition, tile);
                }
            }
        }

        tilemap.gameObject.SetActive(true);
    }


    public Vector3Int GetGridCelll(Vector3 position)
    {
        return tilemap.WorldToCell(position);
    }


    public Vector3 GetGridCenterPosition(Vector3Int position)
    {
        return tilemap.CellToWorld(position) + 0.5f * tilemap.cellSize;
    }
}
