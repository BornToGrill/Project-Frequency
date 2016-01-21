using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeButtonText : MonoBehaviour {
    
    
    
	// Use this for initialization
	void Start () {
       Transform ButtonTextTransform = gameObject.transform.Find("Text");
        Text buttonText = ButtonTextTransform.gameObject.GetComponent<Text>();
        buttonText.text = "Hagrid gogogo";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
