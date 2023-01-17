using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static int currentId = 0;
    [SerializeField] private int detectionRadius;
    public int DetectionRadius { get { return detectionRadius; } } 
    public int Id;
    private GridMovementEnemy movement;
    public Dice dice;
    public bool isMoving = false;
    public bool isDoneWithTurn = true;
    public bool isDiceDoneRolling = true;
    private bool isCalculatingPath;
    private Vector3Int startPosition;

    public void Initialize()
    {
        Id = currentId++;
        movement = GetComponent<GridMovementEnemy>();
        movement.Initialize();
        movement.ShowGrid(detectionRadius);
        startPosition = movement.WorldToCell(transform.position);
    }

    private void OnEnable()
    {
        dice.DoneRolling += FindPathToPlayer;
    }

    private void OnDisable()
    {
        dice.DoneRolling -= FindPathToPlayer;
    }

    private void Update()
    {
        if (isCalculatingPath && !GameLogic.Instance.IsCalculatingPath)
        {
            isCalculatingPath = false;
            InitMoveToPlayer();
        }

        if (!isDoneWithTurn && !movement.IsMoving)
        {
            isDoneWithTurn = true;
            movement.HideGrid();
            GameLogic.Instance.SwitchMode(GameMode.ENEMY_ROLL_DICE);
        }
    }

    public void OnRollDice()
    {
        GameLogic.Instance.SetEnemyDice(dice);
        dice.OnRollDice();
        isDiceDoneRolling = false;
    }

    public void FindPathToPlayer()
    {
        movement.ShowGrid(dice.DiceValue);
        Grid grid = FindObjectOfType<Grid>();
        Vector3 from = transform.position;
        Vector3 to = FindObjectOfType<PlayerLink>().transform.position;
        GameLogic.Instance.StartPathFinding(grid.WorldToCell(from), grid.WorldToCell(to));
        isCalculatingPath = true;
    }

    public void InitMoveToPlayer()
    {
        movement.IsMoving = true;
        isDoneWithTurn = false;
        StartCoroutine(movement.StartMovingToPlayer(dice.DiceValue));
    }

    public void InitRandomMove()
    {
        movement.StartMovingRandomly();
    }

    public void UpdateSpriteRenderer()
    {
        movement.UpdateSpriteRenderer();
    }

    public void SwitchToMovementGrid()
    {
        movement.SwitchToMovementGrid();
    }

    public void SwitchToDetectionGrid()
    {
        movement.SwitchToDetectionGrid();
    }
}
