using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private static int currentId = 0;
    public int Id;
    private GridMovement movement;
    public Dice dice;
    public bool isMoving = false;
    public bool isDoneWithTurn = true;
    public bool isDiceDoneRolling = true;
    private bool isCalculatingPath;

    private void Awake()
    {
        Id = currentId++;
        movement = GetComponent<GridMovement>();
        movement.Initialize();
    }

    private void Update()
    {
        if (!isDiceDoneRolling && !dice.IsRolling)
        {
            isDiceDoneRolling = true;
            movement.ShowMovementGrid(dice.DiceValue);
            FindPathToPlayer();
        }


        if (isCalculatingPath && !GameLogic.Instance.IsCalculatingPath)
        {
            isCalculatingPath = false;
            InitMoveToPlayer();
        }

        if (!isDoneWithTurn && !movement.isMoving)
        {
            isDoneWithTurn = true;
            movement.HideMovementGrid();
            GameLogic.Instance.NextEnemyRollDice();
        }
    }


    internal void OnRollDice()
    {
        GameLogic.Instance.SetEnemyDice(dice);
        dice.OnRollDice();
        isDiceDoneRolling = false;
    }

    internal void FindPathToPlayer()
    {
        Grid grid = FindObjectOfType<Grid>();
        Vector3 from = transform.position;
        Vector3 to = FindObjectOfType<PlayerLink>().transform.position;
        GameLogic.Instance.StartPathFinding(grid.WorldToCell(from), grid.WorldToCell(to));
        isCalculatingPath = true;
    }

    internal void InitMoveToPlayer()
    {
        movement.isMoving = true;
        isDoneWithTurn = false;
        StartCoroutine(movement.StartMovingToPlayer());
    }

    internal void InitRandomMove()
    {
        movement.StartMovingRandomly();
    }
}
