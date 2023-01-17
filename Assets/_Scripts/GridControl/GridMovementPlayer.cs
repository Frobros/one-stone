using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMovementPlayer : GridMovement
{
    private GridShadowController gridShadowController;
    public override void Initialize()
    {
        base.Initialize();
        this.gridShadowController = FindObjectOfType<GridShadowController>();
        var levelBounds = FindObjectOfType<GridTerrainManager>().GetLevelBounds();
        this.gridShadowController.Init(levelBounds);
        this.gridShadowController.UpdateShadow(transform.position);
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

        this.gridShadowController.UpdateShadow(targetPosition);
        GameLogic.Instance.UpdateAllEnemySprites();
    }

    public override void ShowGrid(int radius)
    {
        Debug.Log("Show Grid");
        grid.OnShowGrid(transform.position, radius, false);
    }

    internal float GetShadowAlphaAt(Vector3Int gridPosition)
    {
        return this.gridShadowController.GetAlphaAt(gridPosition);
    }
}
