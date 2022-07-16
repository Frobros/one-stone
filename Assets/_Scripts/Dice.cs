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

using RNG = System.Random;


public class Dice : MonoBehaviour
{
	RNG rng;

	void Awake()
	{
		rng = new Random();
	}

	int roll()
	{
		return rng.Next(1,7); // creates a number between 1 and 6
	}

}
