using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public DiceUI enemyDiceUI;
    public DiceUI playerDiceUI;
    public GameObject textPanel;
    public Image textBackground;
    public Color[] Colors;
    public TextMeshProUGUI gameText;

    private void Awake()
    {
        textBackground.color = Colors[0];
        playerDiceUI.SetDice(FindObjectOfType<PlayerLink>().dice);
    }

    public void SetEnemyDice(Dice dice)
    {
        enemyDiceUI.SetDice(dice);
    }

    public void OnWaitForPlayerDiceRoll()
    {
        textPanel.SetActive(true);
        textBackground.color = Colors[1];
        gameText.text = "Press SPACE to roll your dice!";
    }
    public void OnPlayerRollDice()
    {
        textPanel.SetActive(true);
        textBackground.color = Colors[2];
        gameText.text = "Wait for it...";
    }

    public void OnPlayerMove()
    {
        textPanel.SetActive(true);
        textBackground.color = Colors[3];
        gameText.text = "Move and Place with SPACE!";
    }

    public void OnEnemyRollDice(Enemy enemy)
    {
        textPanel.SetActive(true);
        gameText.text = "Your Enemies are on the move...";
        textBackground.color = Colors[4];
    }

    internal void OnPlayerMoveFreely()
    {
        textBackground.color = Colors[0];
        textPanel.SetActive(false);
    }
}
