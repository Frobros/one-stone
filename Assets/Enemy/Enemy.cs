using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GridMovement movement;
    public Dice dice;
    public bool isDiceDoneRolling = true;
    public bool isMoving = false;
    public bool isDoneWithTurn = true;

    private void Awake()
    {
        movement = GetComponent<GridMovement>();
    }

    private void Update()
    {
        if (!isDiceDoneRolling)
        {
            isDiceDoneRolling = !dice.IsRolling;

            if (isDiceDoneRolling)
            {
                movement.ShowMovementGrid(dice.DiceValue);
                GameLogic.Instance.SwitchToEnemyMoveMode();
            }
        }

        if (!isDoneWithTurn)
        {
            isDoneWithTurn = !movement.isMoving;
            if (isDoneWithTurn)
            {
                movement.HideMovementGrid();
                GameLogic.Instance.NextEnemyRollDice();
            }
        }
    }


    internal void OnRollDice()
    {
        UIManager.Instance.SetEnemyDice(dice);
        dice.OnRollDice();
        isDiceDoneRolling = false;
    }

    internal void InitEnemyMovement()
    {
        movement.isMoving = true;
        isDoneWithTurn = false;
        StartCoroutine(movement.StartMovingToPlayer());
    }

    internal void InitRandomEnemyMove()
    {
        movement.StartMovingRandomly();
    }
}
