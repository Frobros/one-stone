/*

####################################
System.Random vs. UnityEngine.Random
####################################

Both are initialized with system clock.

UnityEngine.Random is a static class. Its state is globally shared. 
Not optimal when multiple independent RNGs are required.

In our case, every dice needs its own RNG. That's why the instanced system RNG is used.

For completeness, an alias is used for Random in order to avoid possible "ambiguous reference" compiler errors,
although there should not be any reference uncertanties.

https://docs.unity3d.com/ScriptReference/Random.html
Section "Versus System.Random"

*/

using System.Collections;
using RNG = System.Random;
using UnityEngine;
using TMPro;

public class Dice : MonoBehaviour
{
	private static RNG rng;
	private void Awake()
	{
		if (rng == null)
        {
			rng = new RNG();
        }
	}

    #region Dice Value
    [SerializeField] private float rollTime;
	[SerializeField] private int diceValue;
	public int DiceValue { get { return diceValue; } }
	public delegate void DoneRollingHandler();
	public event DoneRollingHandler DoneRolling;
	private Coroutine rollDiceCoroutine;

	public bool CompareIfGreater(Dice other)
    {
		return this.DiceValue > other.DiceValue;
    }

	public void OnRollDice()
    {
		if (rollDiceCoroutine != null)
        {
			StopCoroutine(rollDiceCoroutine);
        }
		rollDiceCoroutine = StartCoroutine(StartRollDice());
    }

	private IEnumerator StartRollDice()
    {
		float elapsedTime = 0;
		float switchTime = 0;
		float switchTimeIncrement = 0;
		while (elapsedTime <= rollTime)
        {
			if (elapsedTime >= switchTime)
            {
				switchTime += switchTimeIncrement;
				switchTimeIncrement = Mathf.Lerp(0f, 0.05f * rollTime, elapsedTime/rollTime);
				diceValue = rng.Next(1, 7);
            }
			elapsedTime += Time.deltaTime;
			yield return null;
        }
		DoneRolling?.Invoke();
	}
    #endregion
}
