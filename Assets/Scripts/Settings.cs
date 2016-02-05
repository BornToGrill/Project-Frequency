using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class Settings : MonoBehaviour {
	Resolution[] PossibleResolutions;

	public bool isMute;
	public bool isWindowed;
	public bool setResolution;
	public bool isGrid;
	static public Color color = new Color(1, 1, 1, .3f);

	// Use this for initialization
	void Start()
	{
		if (setResolution) {
			SetResolution ();
		} else if (isWindowed) {
			Toggle toggle = GetComponent<Toggle> ();
			toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("windowed_mode"));
			IsWindowed ();
		} else if (isMute) {
			Toggle toggle = GetComponent<Toggle> ();
			toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("game_volume_int"));
			IsMute ();
		} else {
			Toggle toggle = GetComponent<Toggle> ();
			int p = PlayerPrefs.GetInt("grid_deactivated");
			toggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("grid_deactivated"));
			IsGrid ();
		}
	}

	private void SetResolution()
	{
		Dropdown dropdown = GetComponent<Dropdown> ();
		PossibleResolutions = Screen.resolutions;
		foreach (Resolution res in PossibleResolutions) {
			dropdown.options.Add (new Dropdown.OptionData () { text = res.width + " x " + res.height });
		}

		Resolution currentRes;
		if(PlayerPrefs.GetInt("windowed_mode") == 1) {
			currentRes = new Resolution () {
				width = Screen.currentResolution.width,
				height = Screen.currentResolution.height
			};
		} else {
			currentRes = new Resolution () {
				width = Screen.width,
				height = Screen.height
			};
		}

	    dropdown.value = Array.FindIndex(PossibleResolutions, 0,
	        x => x.height == currentRes.height && x.width == currentRes.width);

		dropdown.onValueChanged.AddListener ((index) => {
			PlayerPrefs.SetInt("screen_width", PossibleResolutions[index].width);
			PlayerPrefs.SetInt("screen_height", PossibleResolutions[index].height);
			Screen.SetResolution(PlayerPrefs.GetInt("screen_width"), PlayerPrefs.GetInt("screen_height"), Screen.fullScreen);
		});
	}

	private void IsWindowed()
	{
		Toggle toggle = GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener ((windowed) => {
			if (windowed) {
				PlayerPrefs.SetInt("windowed_mode", 1);
			} else {
				PlayerPrefs.SetInt("windowed_mode", 0);
			}

			Screen.fullScreen = !Convert.ToBoolean(PlayerPrefs.GetInt("windowed_mode"));
		});
	    toggle.onValueChanged.Invoke(toggle.isOn);
	}

	private void IsMute()
	{
		Toggle mute = GetComponent<Toggle> ();
		mute.onValueChanged.AddListener ((x) => {
			if (!x) {
				PlayerPrefs.SetFloat("game_volume", 1f);
				PlayerPrefs.SetInt("game_volume_int", 0);
			} else {
				PlayerPrefs.SetFloat("game_volume", 0f);
				PlayerPrefs.SetInt("game_volume_int", 1);
			}
			AudioListener.volume = PlayerPrefs.GetFloat("game_volume");
		});
	    mute.onValueChanged.Invoke(mute.isOn);
	}

	private void IsGrid()
	{
		Toggle grid = GetComponent<Toggle> ();
		grid.onValueChanged.AddListener ((x) => {
			if (x) {
				PlayerPrefs.SetInt("grid_deactivated", 1);
				color = new Color(1, 1, 1, 0);
			} else {
				PlayerPrefs.SetInt("grid_deactivated", 0);
				color = new Color(1, 1, 1, .3f);
			}
		});
	    grid.onValueChanged.Invoke(grid.isOn);
	}
}