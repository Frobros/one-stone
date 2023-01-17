using UnityEngine;
using UnityEngine.InputSystem;

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
    private GridMovementPlayer movement;

    private PlayerInput playerInput;
    private InputActionMap movementActionMap;
    private InputActionMap rollingDiceActionMap;
    public InputMode mode;

    private void OnEnable()
    {
        dice.DoneRolling += OnDoneRolling;
    }

    private void OnDisable()
    {
        dice.DoneRolling -= OnDoneRolling;
    }

    public void Initialize()
    {
        InitializeMovement();
        movement = GetComponent<GridMovementPlayer>();
        movement.Initialize();
        transform.position = movement.GetGridCenterPosition(transform.position);
    }

    private void InitializeMovement()
    {
        playerInput = GetComponent<PlayerInput>();
        movementActionMap = playerInput.actions.FindActionMap("Move");
        rollingDiceActionMap = playerInput.actions.FindActionMap("ThrowDice");
    }

    internal void SwitchToMoveFreelyMode()
    {
        if (mode != InputMode.MOVE_FREELY)
        {
            mode = InputMode.MOVE_FREELY;
            rollingDiceActionMap.Disable();
            movementActionMap.Enable();
        }
    }

    internal float GetShadowAlphaAt(Vector3Int gridPosition)
    {
        return this.movement.GetShadowAlphaAt(gridPosition);
    }

    public void SwitchToMoveMode()
    {
        if (mode != InputMode.MOVE)
        {
            mode = InputMode.MOVE;
            rollingDiceActionMap.Disable();
            movementActionMap.Enable();
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
        dice.OnRollDice();
    }

    public void OnDoneRolling()
    {
        movement.ShowGrid(dice.DiceValue);
        GameLogic.Instance.SwitchMode(GameMode.PLAYER_MOVE_DICE_ROLL);
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        if (mode == InputMode.MOVE)
        {
            movement.SetMovementGridActive(false);
            DisableControls();
            var cell = movement.WorldToCell(transform.position);
            if (GameLogic.Instance.IsBaseTile(cell))
            {
                GameLogic.Instance.SwitchMode(GameMode.PLAYER_MOVE_FREELY);
            }
            else
            {
                GameLogic.Instance.SwitchMode(GameMode.ENEMY_ROLL_DICE);
            }
        }
        /*
        else if (mode == InputMode.MOVE_FREELY)
        {
            movement.SetMovementGridActive(false);
            GameLogic.Instance.SwitchToPlayerRollDiceMode();
        }
        */
    }

    public void OnReload()
    {
        GameLogic.Instance.OnReloadScene();
    }

    public void OnMove(InputAction.CallbackContext directionValue)
    {
        Vector2 direction = directionValue.ReadValue<Vector2>();
        if (direction != Vector2.zero)
        {
            bool isMovingFreely = mode == InputMode.MOVE_FREELY;
            if (isMovingFreely && !movement.IsMakingStep)
            {
                GameLogic.Instance.InitializeMoveAllEnemiesRandom();
            }

            if (direction.x < 0)
            {
                movement.MoveLeft(isMovingFreely);
            }
            else if (direction.x > 0)
            {
                movement.MoveRight(isMovingFreely);
            }
            else if (direction.y < 0)
            {
                movement.MoveDown(isMovingFreely);
            }
            else if (direction.y > 0)
            {
                movement.MoveUp(isMovingFreely);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnReload();
        }
    }
}
