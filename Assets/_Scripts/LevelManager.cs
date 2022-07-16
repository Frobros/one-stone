using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TerrainType
{
    WATER = 'w',
    GRASS = 'g',
    MOUNTAIN = 'm',
    FOREST = 'f',
    PLAYER = 'p'
}

[Serializable]
public class Terrain
{
    public string name;
    public int id;
    public TerrainType type;
    public char representingChar;
    public LayerMask layerMask;
    public Tilemap tilemap;
    public TileBase tile;
    public bool isWalkable;

}

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }
    public List<Terrain> terrainList;

    private void Awake()
    {
        _instance = this;
    }

    internal void SetTile(TerrainType terrainType, int row, int column)
    {
        var terrain = terrainList.Find(t => t.type == terrainType);
        terrain.tilemap.SetTile(new Vector3Int(row, column, 0), terrain.tile);
    }

    internal bool IsWalkableTile(Vector3Int cell)
    {
        foreach (var terrain in terrainList)
        {
            if (terrain.isWalkable && terrain.tilemap.GetTile(cell) != null)
            {
                return true;
            }
        }
        return false;
    }
}
