using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridTerrainManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public List<GridCellTerrain> terrainList;
    public List<Enemy> enemies;
    public Tilemap pathFindingTilemap;
    public TileBase pathfindingTileCurrent;
    public TileBase pathfindingTileOld;
    public Vector3Int oldPosition;

    public void LoadTerrainForLevel(int level)
    {
        var textFile = Resources.Load<TextAsset>($"Level{level}");
        var content = textFile.text;
        var allLines = content.Split('\n');
        for (int i = 0; i < allLines.Length; i++)
        {
            var words = allLines[i].Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                int c = words[j][0];
                if (Enum.IsDefined(typeof(GridCellTerrainType), c))
                {
                    GridCellTerrainType terrainType = (GridCellTerrainType)(c);
                    SetTile(terrainType, j, -i);
                }

                if (words[j].Length > 1 && Enum.IsDefined(typeof(GridCellTerrainType), (int)words[j][1]))
                {
                    if (words[j][1] == 'p')
                    {
                        GameLogic.Instance.SetPlayerPosition(new Vector3Int(j, -i, 0));
                    }
                    else if (words[j][1] == 'e')
                    {
                        var gridPosition = new Vector3Int(j, -i, 0);
                        var enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, transform);
                        var enemyS = enemy.GetComponentInChildren<Enemy>();
                        enemyS.SetPosition(gridPosition);
                        enemies.Add(enemyS);
                    }
                }
            }
        }
    }

    public BoundsInt GetLevelBounds()
    {

        BoundsInt levelBounds = new BoundsInt();
        foreach (var terrain in terrainList)
        {
            BoundsInt tilemapBounds = terrain.tilemap.cellBounds;
            levelBounds.xMin = Mathf.Min(levelBounds.xMin, tilemapBounds.xMin);
            levelBounds.xMax = Mathf.Max(levelBounds.xMax, tilemapBounds.xMax);
            levelBounds.yMin = Mathf.Min(levelBounds.yMin, tilemapBounds.yMin);
            levelBounds.yMax = Mathf.Max(levelBounds.yMax, tilemapBounds.yMax);
        }
        levelBounds.xMin -= 2;
        levelBounds.xMax += 2;
        levelBounds.yMin -= 2;
        levelBounds.yMax += 2;
        return levelBounds;
    }


    internal void PaintNode(Vector3Int position)
    {
        pathFindingTilemap.SetTile(position, pathfindingTileCurrent);
        oldPosition = position;
    }

    internal void PaintPath(GridPathNode node)
    {
        pathFindingTilemap.CompressBounds();
        BoundsInt bounds = pathFindingTilemap.cellBounds;
        TileBase[] tiles = pathFindingTilemap.GetTilesBlock(bounds);

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = tiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    Vector3Int tilePosition = new Vector3Int(bounds.min.x + x, bounds.min.y + y, 0);
                    pathFindingTilemap.SetTile(tilePosition, pathfindingTileOld);
                }
            }
        }

        while (node.previous != null)
        {
            pathFindingTilemap.SetTile(node.position, pathfindingTileCurrent);
            node = node.previous;
        }
    }

    internal void ClearPathFindingTilemap()
    {
        pathFindingTilemap.ClearAllTiles();
    }

    private void SetTile(GridCellTerrainType terrainType, int row, int column)
    {
        var terrain = terrainList.Find(t => t.type == terrainType);
        terrain.tilemap.SetTile(new Vector3Int(row, column, 0), terrain.tile);
    }

    internal void ClearNodes()
    {
        pathFindingTilemap.ClearAllTiles();
    }

    public bool IsWalkableTile(Vector3Int cell, bool requestFromEnemy = false)
    {
        foreach (var terrain in terrainList)
        {
            if (requestFromEnemy)
            {
                if (terrain.isWalkableByEnemy && terrain.tilemap.GetTile(cell) != null)
                {
                    return true;
                }
            }
            else if (terrain.isWalkableByPlayer && terrain.tilemap.GetTile(cell) != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsBaseTile(Vector3Int cell)
    {
        return terrainList.Exists(t => t.type == GridCellTerrainType.BASE && t.tilemap.GetTile(cell) != null);
    }
}
