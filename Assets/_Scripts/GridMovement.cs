using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    public float moveTime;
    public bool isMakingStep;
    public bool isMoving = false;

    public GameObject movementGridPrefab;
    private Grid grid;
    private MovementGrid movementGrid;
    private Vector2 up;
    private Vector2 down;
    private Vector2 left;
    private Vector2 right;

    public SpriteRenderer spriteRenderer;
    public Sprite spriteUp;
    public Sprite spriteLeft;
    public Sprite spriteDown;
    public Sprite spriteRight;

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        movementGrid = Instantiate(movementGridPrefab, grid.transform).GetComponent<MovementGrid>();

        up = 0.5f * new Vector2(grid.cellSize.x, grid.cellSize.y);
        down = -up;
        left = 0.5f * new Vector2(-grid.cellSize.x, grid.cellSize.y);
        right = -left;
    }

    public void StartMovingRoutine(Vector2 direction)
    {
        if (!isMakingStep)
        {
            isMakingStep = true;
            Vector2 newPosition = transform.position + (Vector3)direction;
            Vector3Int tileCell = grid.WorldToCell(newPosition);
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
    internal IEnumerator StartMovingToPlayer()
    {
        List<Vector3Int> movements = GetMovementStepsToPlayer();
        int i = 0;
        bool canReachTile = true;
        while (i < movements.Count && canReachTile)
        {
            canReachTile = isTileReachable(movements[i]);
            yield return new WaitUntil(() => !isMakingStep);
            Vector2 currentPosition = transform.position;
            Vector2 nextPosition = grid.GetCellCenterWorld(movements[i]);
            StartMovingRoutine(nextPosition - currentPosition);
            i++;
        }
        isMoving = false;
    }

    internal bool isTileReachable(Vector3Int position)
    {
        return movementGrid.IsWalkable(position);
    }

    internal List<Vector3Int> GetMovementStepsToPlayer()
    {
        return movementGrid.GetMovementStepsToPlayer(transform.position);
    }

    private IEnumerator StartMoving(Vector2 direction)
    {
        float elapsedTime = 0;
        SetSprite(direction);
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isMakingStep = false;
    }

    private IEnumerator StartMovingBackAndForth(Vector2 direction)
    {
        float elapsedTime = 0;
        SetSprite(direction);
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
        isMakingStep = false;
    }



    private void SetSprite(Vector2 direction)
    {
        if (direction == up)
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (direction == down)
        {
            spriteRenderer.sprite = spriteDown;
        }
        else if (direction == left)
        {
            spriteRenderer.sprite = spriteLeft;
        }
        else if (direction == right)
        {
            spriteRenderer.sprite = spriteRight;
        }
    }

    public Vector3 GetGridCenterPosition(Vector3 position)
    {
        return grid.GetCellCenterWorld(grid.WorldToCell(position));
    }

    public void SetMovementGridActive(bool active)
    {
        movementGrid.gameObject.SetActive(active);
    }

    public void ShowMovementGrid(int radius)
    {
        movementGrid.OnShowGrid(transform.position, radius);
    }


    internal void HideMovementGrid()
    {
        movementGrid.HideGrid();
    }

    public void MoveUp()
    {
        StartMovingRoutine(up);
    }

    public void MoveDown()
    {
        StartMovingRoutine(down);
    }

    public void MoveRight()
    {
        StartMovingRoutine(right);
    }

    public void MoveLeft()
    {
        StartMovingRoutine(left);
    }
}
