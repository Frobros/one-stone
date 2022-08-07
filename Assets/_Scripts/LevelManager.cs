using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TerrainType
{
    GROUND = 'g',
    BASE = 'b',
    PLAYER = 'p',
    ENEMY = 'e'

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
    public bool isWalkableByPlayer;
    public bool isWalkableByEnemy;
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

    private void Start()
    {
        LoadTerrain();
        GameLogic.Instance.StartGame();
    }

    private void LoadTerrain()
    {
        var textFile = Resources.Load<TextAsset>("TestLevel");
        var content = textFile.text;
        var allLines = content.Split('\n');
        for (int i = 0; i < allLines.Length; i++)
        {
            var words = allLines[i].Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                int c = words[j][0];
                if (Enum.IsDefined(typeof(TerrainType), c))
                {
                    TerrainType terrainType = (TerrainType)(c);
                    SetTile(terrainType, j, -i);
                }

                if (words[j].Length > 1 && Enum.IsDefined(typeof(TerrainType), (int)words[j][1]))
                {
                    if (words[j][1] == 'p')
                    {
                        GameLogic.Instance.SetPlayerPosition(new Vector3Int(j, -i, 0));
                    }
                    else if (words[j][1] == 'e')
                    {
                        GameLogic.Instance.SpawnEnemy(new Vector3Int(j, -i, 0));
                    }
                }
            }
        }
    }

    private void SetTile(TerrainType terrainType, int row, int column)
    {
        var terrain = terrainList.Find(t => t.type == terrainType);
        terrain.tilemap.SetTile(new Vector3Int(row, column, 0), terrain.tile);
    }

    public bool IsWalkableTile(Vector3Int cell, bool requestFromEnemy)
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
}
