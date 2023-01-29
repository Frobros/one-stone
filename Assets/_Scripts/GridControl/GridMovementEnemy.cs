using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementEnemy : GridMovement
{
    private Coroutine interpolateAlphaCoroutine;
    private Enemy enemy;
    private PlayerLink player;

    [SerializeField]
    private bool isPlayerDetected;

    public delegate void OnDoneMoving();
    public event OnDoneMoving OnDoneMovingToPlayer;

    public override void Initialize()
    {
        base.Initialize();
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<PlayerLink>();
        UpdateSpriteRenderer(false);
        UpdateDetection();
    }

    public void InitMoveRandomly()
    {
        if (isMakingStep) return;

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
        StartCoroutine(Move(direction));
    }

    public override IEnumerator Move(Vector2 direction)
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
        UpdateSpriteRenderer();
        UpdateDetection();
        isMakingStep = false;
    }

    public void InitMovingToPlayer(int diceValue)
    {
        StartCoroutine(MoveToPlayer(diceValue));
    }

    private IEnumerator MoveToPlayer(int diceValue)
    {
        List<Vector3Int> movements = GameLogic.Instance.GridPathPositions;
        int i = 0;
        bool isPositionWalkable = true;
        while (i <= diceValue && i < movements.Count && isPositionWalkable)
        {
            isPositionWalkable = grid.IsWalkable(movements[i]);
            yield return new WaitUntil(() => !isMakingStep);
            UpdateSpriteRenderer();
            Vector2 currentPosition = transform.position;
            Vector2 nextPosition = grid.GetCellCenterWorld(movements[i]);
            StartMovingRoutine(nextPosition - currentPosition, false);
            i++;
        }
        OnDoneMovingToPlayer?.Invoke();
    }

    public void UpdateSpriteRenderer(bool interpolate = true)
    {
        Vector3Int gridPosition = grid.WorldToCell(transform.position);
        var color = spriteRenderer.color;
        color.a = Mathf.Min(2f * (1f - player.GetShadowAlphaAt(gridPosition)), 1f);

        if (!interpolate)
        {
            spriteRenderer.color = color;
            spriteRenderer.sortingOrder = color.a > 0f ? 6 : 4;
            return;
        }

        if (interpolateAlphaCoroutine != null)
        {
            StopCoroutine(interpolateAlphaCoroutine);
        }
        interpolateAlphaCoroutine = StartCoroutine(InterpolateSpriteRendererAlpha(color));
    }

    private IEnumerator InterpolateSpriteRendererAlpha(Color targetColor)
    {
        var animateFor = 0.5f;
        var animateTime = 0f;
        var color = spriteRenderer.color;

        while (animateTime < animateFor)
        {
            color = Color.Lerp(color, targetColor, animateTime / animateFor);
            spriteRenderer.sortingOrder = color.a > 0f ? 6 : 4;
            spriteRenderer.color = color;
            animateTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = targetColor;
        spriteRenderer.sortingOrder = targetColor.a > 0f ? 6 : 4;
    }

    public void UpdateDetection()
    {
        grid.OnUpdateDetectionGrid(transform.position, enemy.DetectionRadius);
        var isPlayerDetectedNow = IsPlayerInDetectionRange();
        if (isPlayerDetected && !isPlayerDetectedNow)
        {
            OnHideMovementGrid();
            GameLogic.Instance.RemoveEnemyFromEncounter(enemy);
        }
        else if (!isPlayerDetected && isPlayerDetectedNow)
        {
            GameLogic.Instance.AddEnemyToEncounter(enemy);
        }
        isPlayerDetected = isPlayerDetectedNow;
    }

    public void UpdateMovementGrid(int diceValue)
    {
        grid.OnUpdateMovementGrid(transform.position, diceValue, true);
    }

    public bool IsPlayerInDetectionRange()
    {
        var playerPosition = player.transform.position;
        var playerCell = grid.WorldToCell(playerPosition);
        return grid.IsCellInDetectionRadius(playerCell);
    }
}
