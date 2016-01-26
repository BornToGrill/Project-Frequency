using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Settings : MonoBehaviour {
    Dropdown dropdown;
	Toggle toggle;
	Toggle mute;

    Resolution[] PossibleResolutions;

	public bool isMute;
	public bool isWindowed;
	public bool setResolution;
	public bool isGrid;

    // Use this for initialization
    void Start()
    {
		if (setResolution) {
			dropdown = GetComponent<Dropdown> ();
			PossibleResolutions = Screen.resolutions;
			foreach (Resolution res in PossibleResolutions) {
				dropdown.options.Add (new Dropdown.OptionData () { text = res.width + " x " + res.height });
			}

			Resolution currentRes = new Resolution () {
				width = Screen.width,
				height = Screen.height
			};

			dropdown.value = Array.IndexOf (PossibleResolutions, currentRes);
			dropdown.onValueChanged.AddListener ((x) => {
				OnDropDownChanged (x);
			});
		} else if (isWindowed) {
			toggle = GetComponent<Toggle> ();
			toggle.isOn = !Screen.fullScreen;

			toggle.onValueChanged.AddListener ((x) => {
				toggleFullscreen (x);
			});
		} else if (isMute) {
			mute = GetComponent<Toggle> ();
			mute.onValueChanged.AddListener ((x) => {
				muteSound (x);
			});
		} else if (isGrid) {
			// TODO: Make grid switch on and off.
		} else {
			return;
		}
    }

	public void toggleFullscreen(bool windowed) {
		Screen.fullScreen = !Screen.fullScreen;
		toggle.isOn = windowed;
	}

	public void muteSound(bool mute) {
		if (!mute)
			AudioListener.volume = 1f;
		else
			AudioListener.volume = 0;
	}

	public void OnDropDownChanged(int index)
	{
		Screen.SetResolution(PossibleResolutions[index].width, PossibleResolutions[index].height, Screen.fullScreen);
	}
}

