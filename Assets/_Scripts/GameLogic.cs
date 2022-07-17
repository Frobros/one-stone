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
    public GameObject enemyPrefab;
    private bool isReloading;

    private void Awake()
    {
        _instance = this;
    }

    public void SetPlayerPosition(Vector3Int tilePosition)
    {
        Vector3 pos = FindObjectOfType<Grid>().GetCellCenterWorld(tilePosition);
        player.position = pos;
    }

    public void OnReloadScene()
    {
        if (!isReloading)
        {
            isReloading = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void StartGame()
    {
        SwitchToPlayerRollDiceMode();
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
    }

    public void SwitchToPlayerRollDiceMode()
    {
        FindObjectOfType<PlayerLink>().SwitchToRollDiceMode();
        UIManager.Instance.OnWaitForPlayerDiceRoll();
    }

    public void SwitchToPlayerMoveMode()
    {
        FindObjectOfType<PlayerLink>().SwitchToMoveMode();
        UIManager.Instance.OnPlayerMove();
    }

    internal void SpawnEnemy(Vector3Int gridPosition)
    {
        var enemy = Instantiate(enemyPrefab, FindObjectOfType<Grid>().GetCellCenterWorld(gridPosition), Quaternion.identity).GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    #region Enemy
    public List<Enemy> enemies = new List<Enemy>();
    public int currentEnemy = 0;

    public void SwitchToEnemyRollDiceMode()
    {
        currentEnemy = 0;
        UIManager.Instance.OnWaitForEnemyDiceRoll(enemies[currentEnemy]);
    }

    public void NextEnemyRollDice()
    {
        currentEnemy++;

        if (currentEnemy >= enemies.Count)
        {
            SwitchToPlayerRollDiceMode();
            return;
        }
        UIManager.Instance.OnWaitForEnemyDiceRoll(enemies[currentEnemy]);
    }

    public void SwitchToEnemyMoveMode()
    {
        UIManager.Instance.OnEnemyMove();
        enemies[currentEnemy].InitEnemyMovement();
    }
    #endregion
}
