using System.Collections;
using UnityEngine;
using RNG = System.Random;

public class GridMovement : MonoBehaviour
{
    public GameObject movementGridPrefab;

    protected const float moveTime = 0.2f;
    public bool IsMakingStep;
    public bool IsMoving;

    protected RNG rng;
    protected MovementGrid grid;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 up;
    protected Vector2 down;
    protected Vector2 left;
    protected Vector2 right;

    public Sprite spriteUp;
    public Sprite spriteLeft;
    public Sprite spriteDown;
    public Sprite spriteRight;

    public virtual void Initialize()
    {
        this.grid = Instantiate(movementGridPrefab).GetComponent<MovementGrid>();
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.rng = new RNG();

        var cellSize = grid.GetCellSize();
        this.up = 0.5f * new Vector2(cellSize.x, cellSize.y);
        this.down = -up;
        this.left = 0.5f * new Vector2(-cellSize.x, cellSize.y);
        this.right = -left;
    }

    public void StartMovingRoutine(Vector2 direction, bool isMovingFreely)
    {
        if (!IsMakingStep)
        {
            IsMakingStep = true;
            Vector2 newPosition = transform.position + (Vector3)direction;
            Vector3Int tileCell = grid.WorldToCell(newPosition);
            bool isTileCellWalkable = isMovingFreely && GameLogic.Instance.IsWalkableTile(tileCell, false) || grid.IsWalkable(tileCell);
            if (isTileCellWalkable) 
            {
                StartCoroutine(StartMoving(direction));
            }
            else
            {
                StartCoroutine(StartMovingBackAndForth(direction));
            }
        }
    }


    protected virtual IEnumerator StartMoving(Vector2 direction)
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
        IsMakingStep = false;
    }

    protected virtual IEnumerator StartMovingBackAndForth(Vector2 direction)
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
        IsMakingStep = false;

        GameLogic.Instance.UpdateAllEnemySprites();
    }

    protected void SetSprite(Vector2 direction)
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
        grid.gameObject.SetActive(active);
    }

    public virtual void ShowGrid(int radius)
    {
        grid.OnShowGrid(transform.position, radius, false);
    }


    internal void HideGrid()
    {
        grid.HideGrid();
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
