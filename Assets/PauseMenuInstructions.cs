using UnityEngine;
using System.Collections;


public class PauseMenuInstructions : MonoBehaviour {

    public GameObject instructions;

	public void PMInstructions()
    {
        instructions.SetActive(true);
    }
}
