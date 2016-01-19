using UnityEngine;
using System.Collections;

public class SettingsAdjustResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseUp() {
		Screen.SetResolution (640, 480, true);
	}
}
