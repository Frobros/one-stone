using UnityEngine;

public class GridMovementPlayer : GridMovement
{
    private GridShadowController gridShadowController;

    public override void Initialize()
    {
        base.Initialize();
        gridShadowController = FindObjectOfType<GridShadowController>();
        gridShadowController.Initialize();
        gridShadowController.UpdateShadow(transform.position);
    }

    public override void HasFinishedStep()
    {
        gridShadowController.UpdateShadow(transform.position);
        GameLogic.Instance.UpdateAllEnemySprites();
        isMakingStep = false;
    }

    public override void ShowMovementGrid(int radius)
    {
        grid.OnUpdateMovementGrid(transform.position, radius, false);
    }

    internal float GetShadowAlphaAt(Vector3Int gridPosition)
    {
        return gridShadowController.GetAlphaAt(gridPosition);
    }
}
