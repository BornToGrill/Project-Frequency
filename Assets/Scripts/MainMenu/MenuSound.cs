using UnityEngine;
using System.Collections;

public class MenuSound : MonoBehaviour {
	private static MenuSound instance;


	void Awake() {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
		} else {
			instance = this;
		}

		DontDestroyOnLoad (this.gameObject);
	}

	public void turnMusicOff() {
		if (instance != null) {
			Destroy (this.gameObject);
			instance = null;
		}
	}

	public void pauseMusic() {
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		audio.Pause ();
	}

	public void unPauseMusic() {
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		audio.UnPause ();
	}

	void OnApplicationQuit() {
		instance = null;
	}

	// Update is called once per frame
	void Update () {

	}

	// Use this for initialization
	void Start () { 
		AudioSource audio = gameObject.GetComponent<AudioSource> ();
		if (!audio.isPlaying) {
			audio.Play ();
		}
	    AudioListener.volume = PlayerPrefs.GetFloat("game_volume");
	}	
}