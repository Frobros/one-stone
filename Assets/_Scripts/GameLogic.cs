using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameLogic : MonoBehaviour
{
    private PlayerLink scenePlayer;
    private bool isReloading;

    private LevelManager levelManager;
    private UIManager uiManager;
    private GridPathFinding gridPathFinding;
    private GridTerrainManager sceneGridTerrainManager;
    public GridTerrainManager GridTerrainManager { get { return sceneGridTerrainManager; } }
    private List<Enemy> sceneEnemies;
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

        SwitchToPlayerMoveFreelyMode();
        FindObjectOfType<UIManager>().Initialize();
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

    public void OnReloadScene()
    {
        if (!isReloading)
        {
            isReloading = true;
            levelManager.LoadCurrentLevel();
        }
    }

    public void SwitchToPlayerMoveFreelyMode()
    {
        sceneCamera.Target = scenePlayer.transform;
        scenePlayer.SwitchToMoveFreelyMode();
        uiManager.OnPlayerMoveFreely();
        foreach (var enemy in sceneEnemies)
        {
            enemy.SwitchToDetectionGrid();
        }
        sceneGridTerrainManager.ClearPathFindingTilemap();
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

    public void SwitchToPlayerRollDiceMode()
    {
        sceneCamera.Target = scenePlayer.transform;
        scenePlayer.SwitchToRollDiceMode();
        uiManager.OnWaitForPlayerDiceRoll();
        foreach (var enemy in sceneEnemies)
        {
            enemy.SwitchToMovementGrid();
        }
    }

    public void SwitchToPlayerMoveMode()
    {
        scenePlayer.SwitchToMoveMode();
        uiManager.OnPlayerMove();
    }

    #region Enemy

    internal void InitializeMoveAllEnemiesRandom()
    {
        foreach (var enemy in sceneGridTerrainManager.enemies)
        {
            enemy.InitRandomMove(); 
        }
    }

    public int currentEnemy = 0;

    public void SwitchToEnemyRollDiceMode()
    {
        currentEnemy = 0;

        if (currentEnemy >= sceneEnemies.Count)
        {
            Debug.LogWarning("No enemies present!");
            SwitchToPlayerRollDiceMode();
            return;
        }
        var firstEnemy = sceneEnemies[currentEnemy];
        sceneCamera.Target = firstEnemy.transform;
        uiManager.OnEnemyRollDice(firstEnemy);
        firstEnemy.OnRollDice();
    }

    public void NextEnemyRollDice()
    {
        currentEnemy++;

        if (currentEnemy >= sceneEnemies.Count)
        {
            SwitchToPlayerRollDiceMode();
            return;
        }


        var nextEnemy = sceneEnemies[currentEnemy];
        sceneCamera.Target = nextEnemy.transform;
        uiManager.OnEnemyRollDice(nextEnemy);
        nextEnemy.OnRollDice();
    }

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
