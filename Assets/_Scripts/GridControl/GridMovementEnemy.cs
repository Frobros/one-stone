using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementEnemy : GridMovement
{
    private Coroutine interpolateAlphaCoroutine;
    private Enemy enemy;
    private PlayerLink player;
    private bool isPlayerDetected;

    public delegate void OnDoneMoving();
    public event OnDoneMoving OnDoneMovingToPlayer;

    public override void Initialize()
    {
        base.Initialize();
        enemy = GetComponent<Enemy>();
        player = FindObjectOfType<PlayerLink>();
        UpdateSpriteRenderer(false);
    }

    public void InitMovingToPlayer(int diceValue)
    {
        StartCoroutine(StartMovingToPlayer(diceValue));
    }

    private IEnumerator StartMovingToPlayer(int diceValue)
    {
        List<Vector3Int> movements = GameLogic.Instance.GridPathPositions;
        int i = 0;
        bool canReachTile = true;
        while (i <= diceValue && i < movements.Count && canReachTile)
        {
            canReachTile = isTileReachable(movements[i]);
            yield return new WaitUntil(() => !IsMakingStep);
            UpdateSpriteRenderer();
            Vector2 currentPosition = transform.position;
            Vector2 nextPosition = grid.GetCellCenterWorld(movements[i]);
            StartMovingRoutine(nextPosition - currentPosition, false);
            i++;
        }
        HideGrid();
        OnDoneMovingToPlayer?.Invoke();
    }

    private bool isTileReachable(Vector3Int position)
    {
        return grid.IsWalkable(position);
    }


    public void UpdateSpriteRenderer(bool interpolate = true)
    {
        Vector3Int gridPosition = grid.WorldToCell(transform.position);
        var color = this.spriteRenderer.color;
        color.a = Mathf.Min(2f * (1f - this.player.GetShadowAlphaAt(gridPosition)), 1f);

        if (!interpolate)
        {
            this.spriteRenderer.color = color;
            this.spriteRenderer.sortingOrder = color.a > 0f ? 6 : 4;
            return;
        }

        if (this.interpolateAlphaCoroutine != null)
        {
            StopCoroutine(this.interpolateAlphaCoroutine);
        }
        this.interpolateAlphaCoroutine = StartCoroutine(InterpolateSpriteRendererAlpha(color));
    }

    private IEnumerator InterpolateSpriteRendererAlpha(Color targetColor)
    {
        var animateFor = 0.5f;
        var animateTime = 0f;
        var color = this.spriteRenderer.color;

        while (animateTime < animateFor)
        {
            color = Color.Lerp(color, targetColor, animateTime / animateFor);
            this.spriteRenderer.sortingOrder = color.a > 0f ? 6 : 4;
            this.spriteRenderer.color = color;
            animateTime += Time.deltaTime;
            yield return null;
        }
        this.spriteRenderer.color = targetColor;
        this.spriteRenderer.sortingOrder = targetColor.a > 0f ? 6 : 4;
    }

    public void SwitchToDetectionGrid()
    {
        isPlayerDetected = false;
        ShowGrid(enemy.DetectionRadius);
    }

    public void SwitchToMovementGrid()
    {
        isPlayerDetected = true;
    }

    public void StartMovingRandomly()
    {
        if (!IsMakingStep)
        {
            IsMakingStep = true;
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

    protected override IEnumerator StartMoving(Vector2 direction)
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
        UpdateSpriteRenderer();

        if (!isPlayerDetected)
        {
            ShowGrid(enemy.DetectionRadius);
        }
    }

    public override void ShowGrid(int radius)
    {
        grid.OnShowGrid(transform.position, radius, true);
    }

    public void CheckIsPlayerInRange()
    {
        var playerPosition = player.transform.position;
        var playerCell = grid.WorldToCell(playerPosition);
        if (grid.IsCellInGrid(playerCell))
        {
            GameLogic.Instance.AddEnemyToEncounter(this.enemy);
        }
    }
}
