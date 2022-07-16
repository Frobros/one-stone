using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float moveTime;
    public int radius;
    private Tilemap tilemap;
    public bool isMoving;
    private MovementGrid movementGrid;

    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>();
        transform.position = GetGridCenterPosition(transform.position);
        movementGrid = FindObjectOfType<MovementGrid>();
    }

    public Vector3 GetGridCenterPosition(Vector3 position)
    {
        return tilemap.CellToWorld(tilemap.WorldToCell(position)) + 0.5f * tilemap.cellSize;
    }

    public void OnAction()
    {
        movementGrid.OnShowGrid(transform.position, radius);
    }

    public void OnMove(InputAction.CallbackContext directionValue)
    {
        Vector2 direction = directionValue.ReadValue<Vector2>();
        if (direction.x < 0)
        {
            MoveLeft();
        }
        else if (direction.x > 0)
        {
            MoveRight();
        }
        else if (direction.y < 0)
        {
            MoveDown();
        }
        else if (direction.y > 0)
        {
            MoveUp();
        }

    }

    private void MoveUp()
    {
        StartMovingRoutine(Vector2.up);
    }

    private void MoveDown()
    {
        StartMovingRoutine(Vector2.down);
    }

    private void MoveRight()
    {
        StartMovingRoutine(Vector2.right);
    }

    private void MoveLeft()
    {
        StartMovingRoutine(Vector2.left);
    }

    public void StartMovingRoutine(Vector2 direction)
    {
        if (!isMoving && LevelManager.Instance.IsWalkableTile(tilemap.WorldToCell(transform.position + (Vector3) direction)))
        {
            isMoving = true;
            StartCoroutine(StartMoving(direction));
        }
    }

    private IEnumerator StartMoving(Vector2 direction)
    {
        float elapsedTime = 0;
        Vector2 originalPosition = GetGridCenterPosition(transform.position);
        Vector2 targetPosition= GetGridCenterPosition(transform.position + (Vector3)direction);
        Debug.Log(originalPosition);
        Debug.Log(targetPosition);
        Debug.Log("------------------");

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime/moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;
    }
}
