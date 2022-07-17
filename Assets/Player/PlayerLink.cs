using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum InputMode
{
    MOVE,
    ROLL_DICE
}

public class PlayerLink : MonoBehaviour
{
    public float moveTime;
    public int radius;
    private Tilemap tilemap;
    public bool isMoving;
    private MovementGrid movementGrid;
    public Dice dice;
    private Vector2 up;
    private Vector2 down;
    private Vector2 left;
    private Vector2 right;

    private PlayerInput playerInput;
    private InputActionMap movementActionMap;
    private InputActionMap rollingDiceActionMap;
    private bool isDiceDoneRolling = true;
    public InputMode mode;
    
    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>();
        transform.position = GetGridCenterPosition(transform.position);
        movementGrid = FindObjectOfType<MovementGrid>();

        up = 0.5f * new Vector2(tilemap.cellSize.x, tilemap.cellSize.y);
        down = -up;
        left = 0.5f * new Vector2(-tilemap.cellSize.x, tilemap.cellSize.y);
        right = -left;


        playerInput = GetComponent<PlayerInput>();
        movementActionMap = playerInput.actions.FindActionMap("Move");
        rollingDiceActionMap = playerInput.actions.FindActionMap("ThrowDice");
    }

    private void Update()
    {
        if (!isDiceDoneRolling)
        {
            isDiceDoneRolling = !dice.IsRolling;
            if (isDiceDoneRolling)
            {
                GameLogic.Instance.SwitchToMoveMode();
            }
        }
    }
    public Vector3 GetGridCenterPosition(Vector3 position)
    {
        return tilemap.GetCellCenterWorld(tilemap.WorldToCell(position));
    }

    public void SwitchToMoveMode()
    {
        if (mode != InputMode.MOVE)
        {
            mode = InputMode.MOVE;
            movementActionMap.Enable();
            rollingDiceActionMap.Disable();
            movementGrid.gameObject.SetActive(true);
            movementGrid.OnShowGrid(transform.position, dice.DiceValue);
        }
    }

    internal void SwitchToRollDiceMode()
    {
        if (mode != InputMode.ROLL_DICE)
        {
            mode = InputMode.ROLL_DICE;
            movementActionMap.Disable();
            rollingDiceActionMap.Enable();
        }
    }

    public void OnRollDice()
    {
        isDiceDoneRolling = false;
        GetComponentInChildren<Dice>().OnRollDice();
    }
    public void OnPlace()
    {
        GameLogic.Instance.SwitchToRollDiceMode();
        movementGrid.gameObject.SetActive(false);
    }

    public void OnReload()
    {
        GameLogic.Instance.OnReloadScene();
    }

    public void OnMove(InputAction.CallbackContext directionValue)
    {
        Vector2 direction = directionValue.ReadValue<Vector2>();
        if (direction.x < 0)
        {
            MoveLeft();
        }
        else if (direction.x > 0)
        {
            MoveRight();
        }
        else if (direction.y < 0)
        {
            MoveDown();
        }
        else if (direction.y > 0)
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        StartMovingRoutine(up);
    }

    private void MoveDown()
    {
        StartMovingRoutine(down);
    }

    private void MoveRight()
    {
        StartMovingRoutine(right);
    }

    private void MoveLeft()
    {
        StartMovingRoutine(left);
    }

    public void StartMovingRoutine(Vector2 direction)
    {
        if (!isMoving)
        {
            isMoving = true;
            Vector2 newPosition = transform.position + (Vector3)direction;
            Vector3Int tileCell = tilemap.WorldToCell(newPosition);
            Debug.Log(tileCell);
            Debug.Log(newPosition);
            if (movementGrid.IsWalkable(tileCell) && LevelManager.Instance.IsWalkableTile(tileCell))
            {
                StartCoroutine(StartMoving(direction));
            }
            else
            {
                StartCoroutine(StartMovingBackAndForth(direction));
            }
        }
    }

    private IEnumerator StartMoving(Vector2 direction)
    {
        float elapsedTime = 0;
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        Debug.Log($"Move From {originalPosition} to {targetPosition}");
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;
    }

    private IEnumerator StartMovingBackAndForth(Vector2 direction)
    {
        float elapsedTime = 0;
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        float tryMoveTime = moveTime / 2f;
        Debug.Log($"Try Move From {originalPosition} to {targetPosition}");
        while (elapsedTime < tryMoveTime)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = tryMoveTime;
        while (elapsedTime > 0f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime);
            elapsedTime -= Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        isMoving = false;
    }
}
