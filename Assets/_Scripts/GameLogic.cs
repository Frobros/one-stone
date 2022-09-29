using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private static GameLogic _instance;
    public static GameLogic Instance { get { return _instance; } }
    public PlayerLink player;
    public FollowTarget cam;
    public GameObject enemyPrefab;
    private bool isReloading;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _instance = this;
    }

    public void SetPlayerPosition(Vector3Int tilePosition)
    {
        Vector3 pos = FindObjectOfType<Grid>().GetCellCenterWorld(tilePosition);
        player.transform.position = pos;
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
        player.Initialize();
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        SwitchToPlayerMoveFreelyMode();
    }

    private void SwitchToPlayerMoveFreelyMode()
    {
        cam.Target = player.transform;
        player.SwitchToMoveFreelyMode();
        UIManager.Instance.OnPlayerMoveFreely();
    }

    public void SwitchToPlayerRollDiceMode()
    {
        cam.Target = player.transform;
        player.SwitchToRollDiceMode();
        UIManager.Instance.OnWaitForPlayerDiceRoll();
    }

    public void SwitchToPlayerMoveMode()
    {
        player.SwitchToMoveMode();
        UIManager.Instance.OnPlayerMove();
    }

    #region Enemy
    internal void SpawnEnemy(Vector3Int gridPosition)
    {
        var enemy = Instantiate(enemyPrefab, FindObjectOfType<Grid>().GetCellCenterWorld(gridPosition), Quaternion.identity).GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    internal void PlayerMakingFreeMove()
    {
        foreach (var enemy in enemies)
        {
            enemy.InitRandomMove();
        }
    }

    public List<Enemy> enemies = new List<Enemy>();
    public int currentEnemy = 0;

    public void SwitchToEnemyRollDiceMode()
    {
        currentEnemy = 0;
        cam.Target = enemies[currentEnemy].transform;
        UIManager.Instance.OnEnemyRollDice(enemies[currentEnemy]);
    }

    public void NextEnemyRollDice()
    {
        currentEnemy++;

        if (currentEnemy >= enemies.Count)
        {
            SwitchToPlayerRollDiceMode();
            return;
        }

        cam.Target = enemies[currentEnemy].transform;
        UIManager.Instance.OnEnemyRollDice(enemies[currentEnemy]);
    }
    #endregion
}
