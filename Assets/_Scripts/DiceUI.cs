using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceUI : MonoBehaviour
{
    public TextMeshProUGUI diceValueText;
    public Dice dice;
    public Dice Dice { get { return dice; } }


    public void SetDice(Dice _dice)
    {
        dice = _dice;
    }

    void Update()
    {
        if (dice == null)
        {
            diceValueText.text = "-";
        }
        else
        {
            diceValueText.text = dice.DiceValue.ToString();
        }
    }

    public void OnRollDice()
    {
        dice.OnRollDice();
    }
}
