using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	public void ActivateHover() {
		gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
	}

	public void ExitHover() {
		gameObject.GetComponent<TileController> ().Environment = gameObject.GetComponent<TileController> ().Environment;
	}
}