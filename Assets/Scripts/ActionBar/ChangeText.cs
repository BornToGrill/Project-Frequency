using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Text unitName = gameObject.GetComponent<Text>();
        unitName.text = "Hagrid";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
