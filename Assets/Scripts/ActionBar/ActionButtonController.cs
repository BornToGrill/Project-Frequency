using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Transform text = transform.Find("ActionButtonText");
        GameObject textgo = text.gameObject;
        Text texttext = textgo.GetComponent<Text>();
        texttext.text = "hagrid";




	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
