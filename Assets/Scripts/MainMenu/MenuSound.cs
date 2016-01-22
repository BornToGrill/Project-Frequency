﻿using UnityEngine;
using System.Collections;

public class MenuSound : MonoBehaviour {
	private static MenuSound instance;

	AudioSource audio;

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

	void OnApplicationQuit() {
		instance = null;
	}

	// Update is called once per frame
	void Update () {

	}

	// Use this for initialization
	void Start () { 
		audio = gameObject.GetComponent<AudioSource> ();
		if (!audio.isPlaying) {
			audio.Play ();
		}
	}	
}