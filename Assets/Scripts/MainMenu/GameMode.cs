using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour {
	public void startGame() {
		SceneManager.LoadScene ("MainGame");
	}

	public void startMultiplayer() {
		SceneManager.LoadScene ("Lobby");
	}
}
