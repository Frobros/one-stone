using System.Collections;
using UnityEngine;
using RNG = System.Random;

public class GridMovement : MonoBehaviour
{
    protected RNG rng;
    protected SpriteRenderer spriteRenderer;

    protected Vector2 up;
    protected Vector2 down;
    protected Vector2 left;
    protected Vector2 right;

    public SpaceGrid grid;
    public Sprite spriteUp;
    public Sprite spriteLeft;
    public Sprite spriteDown;
    public Sprite spriteRight;
    public delegate void FinishedStepHandler();
    public event FinishedStepHandler FinishedStep;

    protected const float MOVE_TIME = 0.2f;
    protected bool isMakingStep;
    public bool IsMakingStep { get { return isMakingStep; } }

    public bool IsMoving;

    public virtual void Initialize()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rng = new RNG();

        var cellSize = grid.GetCellSize();
        up = 0.5f * new Vector2(cellSize.x, cellSize.y);
        down = -up;
        left = 0.5f * new Vector2(-cellSize.x, cellSize.y);
        right = -left;
    }

    private void OnEnable()
    {
        FinishedStep += HasFinishedStep;
    }

    private void OnDisable()
    {
        FinishedStep -= HasFinishedStep;
    }

    public void StartMovingRoutine(Vector2 direction, bool isMovingFreely)
    {
        if (!isMakingStep)
        {
            isMakingStep = true;
            Vector2 newPosition = transform.position + (Vector3)direction;
            Vector3Int tileCell = grid.WorldToCell(newPosition);
            bool isTileCellWalkable = isMovingFreely && GameLogic.Instance.IsWalkableTile(tileCell, false) || grid.IsWalkable(tileCell);
            if (isTileCellWalkable) 
            {
                StartCoroutine(Move(direction));
            }
            else
            {
                StartCoroutine(MoveBackAndForth(direction));
            }
        }
    }

    public virtual void HasFinishedStep() { }

    public virtual IEnumerator Move(Vector2 direction)
    {
        float elapsedTime = 0;
        SetSprite(direction);
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        while (elapsedTime < MOVE_TIME)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / MOVE_TIME);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        FinishedStep.Invoke();
    }   

    protected virtual IEnumerator MoveBackAndForth(Vector2 direction)
    {
        float elapsedTime = 0;
        SetSprite(direction);
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition = GetGridCenterPosition(transform.position + (Vector3)direction);
        float tryMoveTime = MOVE_TIME / 2f;
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
        FinishedStep.Invoke();
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

    public virtual void ShowMovementGrid(int radius)
    {
        grid.OnUpdateMovementGrid(transform.position, radius, false);
    }


    internal void OnHideMovementGrid()
    {
        grid.OnHideMovementGrid();
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
