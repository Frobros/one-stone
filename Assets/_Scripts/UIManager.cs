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

    internal void SetPlayerDice(Dice dice)
    {
        GetComponentInChildren<DiceUI>().SetDice(dice);
    }
    #endregion

    public TextMeshProUGUI rollDiceText;
    public TextMeshProUGUI moveText;

    public void OnRollDice()
    {
        rollDiceText.gameObject.SetActive(true);
        moveText.gameObject.SetActive(false);
    }

    public void OnMove()
    {
        rollDiceText.gameObject.SetActive(false);
        moveText.gameObject.SetActive(true);
    }
}
