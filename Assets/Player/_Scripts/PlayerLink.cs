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
    public InputMode inputMode;

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
        // Initialize inputs
        playerInput = GetComponent<PlayerInput>();
        movementActionMap = playerInput.actions.FindActionMap("Move");
        rollingDiceActionMap = playerInput.actions.FindActionMap("ThrowDice");

        // Initialize movement components
        movement = GetComponent<GridMovementPlayer>();
        movement.Initialize();
        transform.position = movement.GetGridCenterPosition(transform.position);
    }

    public void SwitchGameMode(GameMode gameMode)
    {
        switch (gameMode)
        {
            case GameMode.PLAYER_MOVE_FREELY:
                if (inputMode != InputMode.MOVE_FREELY)
                {
                    inputMode = InputMode.MOVE_FREELY;
                    rollingDiceActionMap.Disable();
                    movementActionMap.Enable();
                }
                break;
            case GameMode.PLAYER_ROLL_DICE:
                if (inputMode != InputMode.ROLL_DICE)
                {
                    inputMode = InputMode.ROLL_DICE;
                    movementActionMap.Disable();
                    rollingDiceActionMap.Enable();
                }
                break;
            case GameMode.PLAYER_MOVE_DICE_ROLL:
                if (inputMode != InputMode.MOVE)
                {
                    inputMode = InputMode.MOVE;
                    rollingDiceActionMap.Disable();
                    movementActionMap.Enable();
                }
                break;
        }
    }

    internal void DisableControls()
    {
        if (inputMode != InputMode.DISABLED)
        {
            inputMode = InputMode.DISABLED;
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
        movement.ShowMovementGrid(dice.DiceValue);
        GameLogic.Instance.SwitchMode(GameMode.PLAYER_MOVE_DICE_ROLL);
    }

    public void OnPlace(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        
        if (inputMode == InputMode.MOVE)
        {
            DisableControls();
            movement.OnHideMovementGrid();
            var cell = movement.WorldToCell(transform.position);
            GameLogic.Instance.UpdateEnemyDetection();
            if (GameLogic.Instance.IsBaseTile(cell) || !GameLogic.Instance.IsDetectedByAnyEnemy())
            {
                GameLogic.Instance.ClearEncounteredEnemies();
                GameLogic.Instance.SwitchMode(GameMode.PLAYER_MOVE_FREELY);
            }
            else
            {
                GameLogic.Instance.SwitchMode(GameMode.ENEMY_ROLL_DICE);
            }
        }   
    }

    public void OnReload()
    {
        GameLogic.Instance.OnReloadScene();
    }

    public void OnMove(InputAction.CallbackContext directionValue)
    {
        Vector2 direction = directionValue.ReadValue<Vector2>();
        if (direction == Vector2.zero || movement.IsMakingStep) return;

        // TODO: Call also when not moving freely and only for enemies that have not detected the player
        GameLogic.Instance.MoveRemainingEnemiesRandomly();

        bool isMovingFreely = inputMode == InputMode.MOVE_FREELY;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            OnReload();
        }
    }

    internal float GetShadowAlphaAt(Vector3Int gridPosition)
    {
        return this.movement.GetShadowAlphaAt(gridPosition);
    }
}
