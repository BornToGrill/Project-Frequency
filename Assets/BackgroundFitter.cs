using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BackgroundFitter : MonoBehaviour {

	public int margin = 10;

	// Use this for initialization
	void Start () {
		/*GameObject TextArea = GameObject.Find("Text");
		Text TextAreaText = TextArea.GetComponent<Text> ();
		TextAreaText.text = "Gogogo";
		RectTransform transText = TextArea.GetComponent<RectTransform> ();
		RectTransform transImage = gameObject.GetComponent<RectTransform>();
		transImage.sizeDelta = new Vector2 (100, 50);
		Debug.Log (transText.rect.width);*/

	}

	// Update is called once per frame
	void Update () {
		GameObject TextArea = GameObject.Find("Text");
		Text TextAreaText = TextArea.GetComponent<Text> ();
		TextAreaText.text = "Gogogo";
		RectTransform transText = TextArea.GetComponent<RectTransform> ();
		RectTransform transImage = gameObject.GetComponent<RectTransform>();
		transImage.sizeDelta = new Vector2 (transText.rect.width + margin, transText.rect.height + margin);



	}
}
