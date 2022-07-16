using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _instance;
    public static GameLogic Instance { get { return _instance; } }
    public GameObject playerPrefab;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnPlayer(Vector3Int tilePosition)
    {
        Vector3 pos = FindObjectOfType<Grid>().GetCellCenterWorld(tilePosition);
        Transform player = Instantiate(playerPrefab, pos, Quaternion.identity).transform;
        Camera.main.transform.parent = player;
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
    }
}
