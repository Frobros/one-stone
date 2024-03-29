using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public enum GameMode
{
    PLAYER_MOVE_FREELY,
    PLAYER_ROLL_DICE,
    ENEMY_ROLL_DICE,
    PLAYER_MOVE_DICE_ROLL,
    ENEMY_MOVE_DICE_ROLL
}

public class GameLogic : MonoBehaviour
{
    private PlayerLink scenePlayer;
    private bool isReloading;

    private LevelManager levelManager;
    private UIManager uiManager;
    private GridPathFinding gridPathFinding;
    private GridTerrainManager sceneGridTerrainManager;
    public GridTerrainManager GridTerrainManager { get { return sceneGridTerrainManager; } }
    public List<Enemy> sceneEnemies;
    public List<Enemy> encounteredEnemies = new List<Enemy>();
    private FollowTarget sceneCamera;
    public int levelWhenReload = 0;

    public List<Vector3Int> GridPathPositions {  get { return gridPathFinding.positions; } }
    public bool IsCalculatingPath { get { return gridPathFinding.isCalculating; } }

    #region Singleton

    private static GameLogic _instance;
    public static GameLogic Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60;
        _instance = this;
        DontDestroyOnLoad(gameObject);

        gridPathFinding = GetComponent<GridPathFinding>();
        levelManager = GetComponent<LevelManager>();
        uiManager = GetComponent<UIManager>();
    }

    #endregion Singleton

    #region Initialization
    // called first
    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) { Initialize(); }

    // called when the game is terminated
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void Initialize()
    {
        encounteredEnemies.Clear();
        isReloading = false;
        scenePlayer = FindObjectOfType<PlayerLink>();
        sceneCamera = FindObjectOfType<FollowTarget>();
        sceneGridTerrainManager = FindObjectOfType<GridTerrainManager>();
        LoadLevel(levelWhenReload);

        scenePlayer.Initialize();
        sceneEnemies = sceneGridTerrainManager.enemies;
        foreach(var enemy in sceneEnemies)
        {
            enemy.Initialize();
        }
        FindObjectOfType<UIManager>().Initialize();
        SwitchMode(GameMode.PLAYER_MOVE_FREELY);
    }

    internal void PaintPath(GridPathNode currentNode)
    {
        sceneGridTerrainManager.PaintPath(currentNode);
    }

    // Is also used when pressing R key
    private void LoadLevel(int level)
    {
        sceneGridTerrainManager.LoadTerrainForLevel(level);
    }

    #endregion Initialization

    public void SetPlayerPosition(Vector3Int tilePosition)
    {
        Vector3 pos = FindObjectOfType<Grid>().GetCellCenterWorld(tilePosition);
        scenePlayer.transform.position = pos;
    }

    internal void UpdateEnemyDetection()
    {
        for (int i = encounteredEnemies.Count-1; i >= 0; i--)
        {
            encounteredEnemies[i].UpdateDetection();
        }
    }

    internal void ClearEncounteredEnemies()
    {
        encounteredEnemies.Clear();
    }

    internal bool IsDetectedByAnyEnemy()
    {
        return encounteredEnemies.Count > 0;
    }

    public void OnReloadScene()
    {
        if (!isReloading)
        {
            isReloading = true;
            levelManager.LoadCurrentLevel();
        }
    }

    internal void UpdateAllEnemySprites()
    {
        foreach (var enemy in sceneEnemies)
        {
            enemy.UpdateSpriteRenderer();
        }
    }

    public bool IsBaseTile(Vector3Int cell)
    {
        return GridTerrainManager.IsBaseTile(cell);
    }

    public bool IsWalkableTile(Vector3Int neighbourAt, bool isEnemy = false)
    {
        return sceneGridTerrainManager.IsWalkableTile(neighbourAt, isEnemy);
    }

    public void SwitchMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.PLAYER_MOVE_FREELY:
                sceneCamera.Target = scenePlayer.transform;
                scenePlayer.SwitchGameMode(mode);
                uiManager.OnPlayerMoveFreely();
                break;
            case GameMode.PLAYER_ROLL_DICE:
                currentEnemy = 0;
                sceneCamera.Target = scenePlayer.transform;
                scenePlayer.SwitchGameMode(mode);
                uiManager.OnWaitForPlayerDiceRoll();
                break;
            case GameMode.PLAYER_MOVE_DICE_ROLL:
                scenePlayer.SwitchGameMode(mode);
                uiManager.OnPlayerMove();
                break;
            case GameMode.ENEMY_ROLL_DICE:
                if (encounteredEnemies.Count == 0)
                {
                    Debug.LogWarning("No enemies present!");
                    SwitchMode(GameMode.PLAYER_MOVE_FREELY);
                    return;
                }

                if (currentEnemy >= encounteredEnemies.Count)
                {
                    Debug.Log("Players Turn!");
                    SwitchMode(GameMode.PLAYER_ROLL_DICE);
                    return;
                }

                var nextEnemy = encounteredEnemies[currentEnemy];
                sceneCamera.Target = nextEnemy.transform;
                uiManager.OnEnemyRollDice(nextEnemy);
                nextEnemy.OnRollDice();

                currentEnemy++;
                break;
            default:
                break;
        }
    }

    internal void RemoveEnemyFromEncounter(Enemy enemy)
    {
        this.encounteredEnemies.Remove(enemy);
    }

    public void AddEnemyToEncounter(Enemy _enemy)
    {
        if (encounteredEnemies.Contains(_enemy)) return;
        
        encounteredEnemies.Add(_enemy);
        if (encounteredEnemies.Count == 1)
        {
            SwitchMode(GameMode.PLAYER_ROLL_DICE);
        }
    }

    #region Enemy

    internal void MoveRemainingEnemiesRandomly()
    {
        var remainingEnemies = sceneEnemies.Where(x => !encounteredEnemies.Contains(x));
        foreach (var enemy in remainingEnemies)
        {
            enemy.InitRandomMove(); 
        }
    }

    public int currentEnemy = 0;

    public void SetEnemyDice(Dice dice)
    {
        uiManager.SetEnemyDice(dice);
    }

    public void StartPathFinding(Vector3Int fromCell, Vector3Int toCell)
    {
        gridPathFinding.StartPathFinding(fromCell, toCell);
    }
   #endregion
}
