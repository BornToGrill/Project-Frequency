using UnityEngine;
using System.Collections;

public class TextCrawl : MonoBehaviour {

	public float speed;
	private RectTransform _rectTransform;

	// Use this for initialization
	void Start () {
		_rectTransform = transform as RectTransform;
	}
	
	// Update is called once per frame
	void Update () {
		_rectTransform.anchoredPosition = new Vector2 (_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + speed * Time.deltaTime);


	}
}