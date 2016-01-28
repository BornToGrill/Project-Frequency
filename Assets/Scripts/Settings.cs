using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Settings : MonoBehaviour {
    Dropdown dropdown;
    Resolution[] PossibleResolutions;

    // Use this for initialization
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener((x) => { OnDropDownChanged(x); });

        PossibleResolutions = Screen.resolutions;
        foreach (Resolution res in PossibleResolutions)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = res.width + " x " + res.height });
        }
    }

    public void OnDropDownChanged(int index)
    {
        Screen.SetResolution(PossibleResolutions[index].width, PossibleResolutions[index].height, Screen.fullScreen);
    }
}

