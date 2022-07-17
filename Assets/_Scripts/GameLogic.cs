using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _instance;
    public static GameLogic Instance { get { return _instance; } }
    public Transform player;
    // public GameObject playerPrefab;
    private bool isReloading;

    private void Awake()
    {
        _instance = this;
    }

    public void SpawnPlayer(Vector3Int tilePosition)
    {
        Vector3 pos = FindObjectOfType<Grid>().GetCellCenterWorld(tilePosition);
        // Transform player = Instantiate(playerPrefab, pos, Quaternion.identity).transform;
        UIManager.Instance.SetPlayerDice(player.GetComponentInChildren<Dice>());

        Camera.main.transform.parent = player;
        Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);
    }

    public void OnReloadScene()
    {
        if (!isReloading)
        {
            isReloading = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void Start()
    {
        SwitchToRollDiceMode();
    }

    public void SwitchToRollDiceMode()
    {
        FindObjectOfType<PlayerLink>().SwitchToRollDiceMode();
        UIManager.Instance.OnRollDice();
    }

    public void SwitchToMoveMode()
    {
        FindObjectOfType<PlayerLink>().SwitchToMoveMode();
        UIManager.Instance.OnMove();
    }
}
