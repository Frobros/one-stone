using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region FakeSingleton
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    private void Awake()
    {
        _instance = this;
    }
    #endregion

    #region Dice UI
    public DiceUI playerDiceUI;
    public DiceUI enemyDiceUI;
    internal void SetPlayerDice(Dice dice)
    {
        playerDiceUI.SetDice(dice);
    }

    internal void SetEnemyDice(Dice dice)
    {
        enemyDiceUI.SetDice(dice);
    }

    #endregion


    public GameObject textPanel;
    public TextMeshProUGUI gameText;

    public void OnWaitForPlayerDiceRoll()
    {
        gameObject.SetActive(true);
        gameText.text = "Press SPACE to roll your dice!";
    }
    public void OnPlayerRollDice()
    {
        gameObject.SetActive(true);
        gameText.text = "Wait for it...";
    }

    public void OnPlayerMove()
    {
        gameObject.SetActive(true);
        gameText.text = "Move and Place with SPACE!";
    }

    public void OnInitEnemyDiceRolls()
    {
    }
    public void OnWaitForEnemyDiceRoll(Enemy enemy)
    {
        gameObject.SetActive(true);
        gameText.text = "Your Enemies are on the move...";
        enemy.OnRollDice();
    }

    public void OnEnemyRollDice() {}

    public void OnEnemyMove() {}
}
