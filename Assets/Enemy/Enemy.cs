using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static int currentId = 0;
    public int DetectionRadiusInactive;
    public int DetectionRadiusActive;
    public int Id;
    public GridMovementEnemy movement;
    private PlayerLink player;
    public Dice dice;

    public void Initialize()
    {
        Id = currentId++;
        player = FindObjectOfType<PlayerLink>();
        movement.Initialize();
        movement.OnDoneMovingToPlayer += OnDoneMoving;
        dice.SetMaximumDiceValue(3);
    }

    private void OnEnable()
    {
        dice.DoneRolling += FindPathToPlayer;
    }

    private void OnDisable()
    {
        dice.DoneRolling -= FindPathToPlayer;
        movement.OnDoneMovingToPlayer -= OnDoneMoving;
    }

    public void OnRollDice()
    {
        GameLogic.Instance.SetEnemyDice(dice);
        dice.OnRollDice();
    }

    public void FindPathToPlayer()
    {
        movement.UpdateMovementGrid(dice.DiceValue);
        Vector3 from = transform.position;
        Vector3 to = player.transform.position;
        GameLogic.Instance.StartPathFinding(
            movement.WorldToCell(from),
            movement.WorldToCell(to),
            () => this.movement.InitMovingToPlayer(dice.DiceValue)
        );
    }

    public void FindPathBackHome()
    {
    }

    public void SetPosition(Vector3Int _gridStartPosition)
    {
        movement.SetPosition(_gridStartPosition);
    }

    public void OnDoneMoving()
    {
        GameLogic.Instance.SwitchMode(GameMode.ENEMY_ROLL_DICE);
    }

    public void InitUndetectedMove()
    {
        movement.InitUndetectedMove();
    }

    public void UpdateSpriteRenderer()
    {
        movement.UpdateSpriteRenderer();
    }

    public void UpdateDetection()
    {
        movement.UpdateDetection();
    }
}
