using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public enum InputMode
{
    MOVE,
    MOVE_FREELY,
    ROLL_DICE,
    DISABLED,
    ENEMY_MOVE
}

public class PlayerLink : MonoBehaviour
{
    public Dice dice;
    private GridMovement gridMovement;

    private PlayerInput playerInput;
    private InputActionMap movementActionMap;
    private InputActionMap rollingDiceActionMap;
    private bool isDiceDoneRolling = true;
    public InputMode mode;

    private void Start()
    {
        gridMovement = GetComponent<GridMovement>();
        transform.position = gridMovement.GetGridCenterPosition(transform.position);

        UIManager.Instance.SetPlayerDice(dice);
    }

    internal void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();
        movementActionMap = playerInput.actions.FindActionMap("Move");
        rollingDiceActionMap = playerInput.actions.FindActionMap("ThrowDice");
    }

    private void Update()
    {
        if (!isDiceDoneRolling)
        {
            isDiceDoneRolling = !dice.IsRolling;
            if (isDiceDoneRolling)
            {
                gridMovement.ShowMovementGrid(dice.DiceValue);
                GameLogic.Instance.SwitchToPlayerMoveMode();
            }
        }
    }

    internal void SwitchToMoveFreelyMode()
    {
        if (mode != InputMode.MOVE_FREELY)
        {
            mode = InputMode.MOVE_FREELY;
            movementActionMap.Enable();
            rollingDiceActionMap.Disable();
        }
    }

    public void SwitchToMoveMode()
    {
        if (mode != InputMode.MOVE)
        {
            mode = InputMode.MOVE;
            movementActionMap.Enable();
            rollingDiceActionMap.Disable();
        }
    }

    internal void SwitchToRollDiceMode()
    {
        if (mode != InputMode.ROLL_DICE)
        {
            mode = InputMode.ROLL_DICE;
            movementActionMap.Disable();
            rollingDiceActionMap.Enable();
        }
    }

    internal void DisableControls()
    {
        if (mode != InputMode.DISABLED)
        {
            mode = InputMode.DISABLED;
            movementActionMap.Disable();
            rollingDiceActionMap.Disable();
        }
    }

    public void OnRollDice()
    {
        isDiceDoneRolling = false;
        dice.OnRollDice();
        UIManager.Instance.OnPlayerRollDice();
    }
    public void OnPlace()
    {
        if (mode == InputMode.MOVE)
        {
            gridMovement.SetMovementGridActive(false);
            DisableControls();
            GameLogic.Instance.SwitchToEnemyRollDiceMode();
        }
        else if (mode == InputMode.MOVE_FREELY)
        {
            gridMovement.SetMovementGridActive(false);
            GameLogic.Instance.SwitchToPlayerRollDiceMode();
        }
    }

    public void OnReload()
    {
        GameLogic.Instance.OnReloadScene();
    }

    public void OnMove(InputAction.CallbackContext directionValue)
    {
        Vector2 direction = directionValue.ReadValue<Vector2>();
        if (direction.x < 0)
        {
            gridMovement.MoveLeft(mode == InputMode.MOVE_FREELY);
        }
        else if (direction.x > 0)
        {
            gridMovement.MoveRight(mode == InputMode.MOVE_FREELY);
        }
        else if (direction.y < 0)
        {
            gridMovement.MoveDown(mode == InputMode.MOVE_FREELY);
        }
        else if (direction.y > 0)
        {
            gridMovement.MoveUp(mode == InputMode.MOVE_FREELY);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("UR LOST!");
        if (other.CompareTag("Enemy"))
        {
            OnReload();
        }
    }
}
