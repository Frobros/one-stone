using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var textFile = Resources.Load<TextAsset>("TestLevel");
        var content = textFile.text;
        var allLines = content.Split('\n');
        for (int i = 0; i < allLines.Length; i++)
        {
            int y = 0;
            var words = allLines[i].Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                int c = words[j][0];
                if (Enum.IsDefined(typeof(TerrainType), c))
                {
                    TerrainType terrainType = (TerrainType)(c);
                    FindObjectOfType<LevelManager>().SetTile(terrainType, y, -i);
                    y++;
                }

                if (words[j].Length > 1 && Enum.IsDefined(typeof(TerrainType), (int)words[j][1]))
                {
                    TerrainType terrainType = (TerrainType)words[j][1];
                    GameLogic.Instance.SpawnPlayer(new Vector3Int(y, -i, 0));
                }
            }
        }
    }
}
