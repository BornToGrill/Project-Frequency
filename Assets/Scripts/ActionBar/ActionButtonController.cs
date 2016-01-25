using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButtonController : MonoBehaviour, IPointerClickHandler {
    private Text _buttonText;
    private Callback _callback;
	private string _id;

    void Awake()
    {
        _buttonText = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
    }
	
	public void OnPointerClick(PointerEventData data)
    {
        if (_callback != null)
            _callback.Invoke(_id);
    }

	public void Initialize(string text, Callback callback, float offset, bool clickable)
    {
		_id = text;
        _buttonText.text = text;
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.pivot = new Vector2(0.5f, 0.5f); 
        rectTransform.localScale = new Vector3(1, 1, 0);
        rectTransform.anchoredPosition = new Vector2(offset+50, 50);
		if (!clickable) {
			gameObject.GetComponent<Image> ().color = new Color (0.0f, 0.0f, 0.0f, 0.5f);
			return;
		}
		_callback = callback;
    }
}
