using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using RNG = System.Random;

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

    public bool isEnemy;
    private RNG rng;
    private GridShadowController _gridShadowController;


    public void Initialize()
    {
        _gridShadowController = FindObjectOfType<GridShadowController>();
        grid = FindObjectOfType<Grid>();
        movementGrid = Instantiate(movementGridPrefab, grid.transform).GetComponent<MovementGrid>();

        isEnemy = GetComponentInParent<Enemy>() != null;
        movementGrid.isEnemy = isEnemy;

        up = 0.5f * new Vector2(grid.cellSize.x, grid.cellSize.y);
        down = -up;
        left = 0.5f * new Vector2(-grid.cellSize.x, grid.cellSize.y);
        right = -left;

        rng = new RNG();
    }

    public void StartMovingRoutine(Vector2 direction, bool isMovingFreely)
    {
        if (!isMakingStep)
        {
            isMakingStep = true;
            Vector2 newPosition = transform.position + (Vector3)direction;
            Vector3Int tileCell = grid.WorldToCell(newPosition);
            bool isTileCellWalkable = isMovingFreely && GameLogic.Instance.IsWalkableTile(tileCell, false) || movementGrid.IsWalkable(tileCell);
            if (isTileCellWalkable) 
            {
                if (!isEnemy)
                {
                    _gridShadowController.ApplyLight(newPosition);
                }
                StartCoroutine(StartMoving(direction));
            }
            else
            {
                StartCoroutine(StartMovingBackAndForth(direction));
            }
        }
    }

    public void StartMovingRandomly()
    {
        if (!isMakingStep)
        {
            isMakingStep = true;
            List<Vector3Int> positions = new List<Vector3Int>();
            Vector3Int pos = grid.WorldToCell((Vector2)transform.position + up);
            if (GameLogic.Instance.IsWalkableTile(pos, true))
            {
                positions.Add(pos);
            }
            pos = grid.WorldToCell((Vector2)transform.position + down);
            if (GameLogic.Instance.IsWalkableTile(pos, true))
            {
                positions.Add(pos);
            }
            pos = grid.WorldToCell((Vector2)transform.position + left);
            if (GameLogic.Instance.IsWalkableTile(pos, true))
            {
                positions.Add(pos);
            }
            pos = grid.WorldToCell((Vector2)transform.position + right);
            if (GameLogic.Instance.IsWalkableTile(pos, true))
            {
                positions.Add(pos);
            }

            Vector3Int tileCell = positions[rng.Next(0, positions.Count)];
            Vector2 direction = grid.CellToWorld(tileCell) - transform.position;
            StartCoroutine(StartMoving(direction));
        }
    }

    internal IEnumerator StartMovingToPlayer()
    {
        List<Vector3Int> movements = GameLogic.Instance.GridPathPositions;

        int i = 0;
        bool canReachTile = true;
        while (i < movements.Count && canReachTile)
        {
            canReachTile = isTileReachable(movements[i]);
            yield return new WaitUntil(() => !isMakingStep);
            Vector2 currentPosition = transform.position;
            Vector2 nextPosition = grid.GetCellCenterWorld(movements[i]);
            StartMovingRoutine(nextPosition - currentPosition, false);
            i++;
        }
        isMoving = false;
    }

    public void UpdateSpriteRenderer()
    {
        UpdateSpriteRenderer(grid.WorldToCell(transform.position));
    }
    private void UpdateSpriteRenderer(Vector3Int gridPosition)
    {
        var color = this.spriteRenderer.color;
        color.a = Mathf.Min(2f * (1f - _gridShadowController.GetAlphaAt(gridPosition)), 1f);
        this.spriteRenderer.sortingOrder = color.a > 0f ? 6 : 4;
        this.spriteRenderer.color = color;
    }

    internal bool isTileReachable(Vector3Int position)
    {
        return movementGrid.IsWalkable(position);
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
        UpdateSpriteRenderer(grid.WorldToCell(targetPosition));
    }

    private IEnumerator StartMovingBackAndForth(Vector2 direction)
    {
        float elapsedTime = 0;
        SetSprite(direction);
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        float tryMoveTime = moveTime / 2f;
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

    public Vector3Int WorldToCell(Vector3 position)
    {
        return grid.WorldToCell(position);
    }

    public void SetMovementGridActive(bool active)
    {
        movementGrid.gameObject.SetActive(active);
    }

    public void ShowMovementGrid(int radius)
    {
        movementGrid.OnShowGrid(transform.position, radius, isEnemy);
    }


    internal void HideMovementGrid()
    {
        movementGrid.HideGrid();
    }

    public void MoveUp(bool isMovingFreely)
    {
        StartMovingRoutine(up, isMovingFreely);
    }

    public void MoveDown(bool isMovingFreely)
    {
        StartMovingRoutine(down, isMovingFreely);
    }

    public void MoveRight(bool isMovingFreely)
    {
        StartMovingRoutine(right, isMovingFreely);
    }

    public void MoveLeft(bool isMovingFreely)
    {
        StartMovingRoutine(left, isMovingFreely);
    }
}
