using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static int currentId = 0;
    public int DetectionRadiusInactive;
    public int DetectionRadiusActive;
    public int Id;
    private GridMovementEnemy movement;
    private PlayerLink player;
    public Dice dice;
    private bool isCalculatingPath;

    public void Initialize()
    {
        Id = currentId++;
        movement = GetComponent<GridMovementEnemy>();
        player = FindObjectOfType<PlayerLink>();
        movement.Initialize();
        movement.OnDoneMovingToPlayer += OnDoneMoving;
    }

    private void OnEnable()
    {
        dice.DoneRolling += FindPathToPlayer;
    }

    private void OnDisable()
    {
        dice.DoneRolling -= FindPathToPlayer;
        movement.OnDoneMovingToPlayer -= OnDoneMoving;
    }

    private void Update()
    {
        if (isCalculatingPath && !GameLogic.Instance.IsCalculatingPath)
        {
            isCalculatingPath = false;
            InitMoveToPlayer();
        }
    }

    public void OnRollDice()
    {
        GameLogic.Instance.SetEnemyDice(dice);
        dice.OnRollDice();
    }

    public void FindPathToPlayer()
    {
        movement.UpdateMovementGrid(dice.DiceValue);
        Grid grid = FindObjectOfType<Grid>();
        Vector3 from = transform.position;
        Vector3 to = player.transform.position;
        GameLogic.Instance.StartPathFinding(grid.WorldToCell(from), grid.WorldToCell(to));
        isCalculatingPath = true;
    }

    public void OnDoneMoving()
    {
        GameLogic.Instance.SwitchMode(GameMode.ENEMY_ROLL_DICE);
    }

    public void InitMoveToPlayer()
    {
        movement.InitMovingToPlayer(dice.DiceValue);
    }

    public void InitRandomMove()
    {
        movement.InitMoveRandomly();
    }

    public void UpdateSpriteRenderer()
    {
        movement.UpdateSpriteRenderer();
    }

    public void UpdateDetection()
    {
        movement.UpdateDetection();
    }
}
