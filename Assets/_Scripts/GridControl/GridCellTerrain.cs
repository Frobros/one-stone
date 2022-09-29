using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GridCellTerrainType
{
    GROUND = 'g',
    BASE = 'b',
    PLAYER = 'p',
    ENEMY = 'e'

}

[Serializable]
public class GridCellTerrain
{
    public string name;
    public int id;
    public GridCellTerrainType type;
    public char representingChar;
    public LayerMask layerMask;
    public Tilemap tilemap;
    public TileBase tile;
    public bool isWalkableByPlayer;
    public bool isWalkableByEnemy;
}
