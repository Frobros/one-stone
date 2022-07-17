using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Dice dice;
    public GridMovement gridMovement;
    public bool isDiceDoneRolling = true;
    public bool isMoving = false;
    public bool isDoneWithTurn = true;

    private void Update()
    {
        if (!isDiceDoneRolling)
        {
            isDiceDoneRolling = !dice.IsRolling;

            if (isDiceDoneRolling)
            {
                gridMovement.ShowMovementGrid(dice.DiceValue);
                GameLogic.Instance.SwitchToEnemyMoveMode();
            }
        }


        if (!isDoneWithTurn)
        {
            isDoneWithTurn = !gridMovement.isMoving;
            if (isDoneWithTurn)
            {
                gridMovement.HideMovementGrid();
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
        gridMovement.isMoving = true;
        isDoneWithTurn = false;
        StartCoroutine(gridMovement.StartMovingToPlayer());
    }
}
