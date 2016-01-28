using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButtonController : MonoBehaviour, IPointerClickHandler {
    private Text _buttonText;
    private Callback _callback;
	private string _id;
	public Sprite TankButton;
	public Sprite SoldierButton;
	public Sprite RobotButton;
	public Sprite BarracksButton;


    void Awake()
    {
		Transform textObject = transform.Find ("Text");
		if (textObject != null)
        	_buttonText = textObject.GetComponent<Text>();
    }
	
	public void OnPointerClick(PointerEventData data)
    {
        if (_callback != null)
            _callback.Invoke(_id);
    }

	public void Initialize(string text, Callback callback, float offset, bool clickable)
    {
		_id = text;
		if (_buttonText != null)
        	_buttonText.text = text;
        RectTransform rectTransform = transform as RectTransform;
        //rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.localScale = new Vector3(1, 1, 0);
        rectTransform.anchoredPosition = new Vector2(offset + 20, 0);
		if (!clickable) {
			gameObject.GetComponent<Button> ().interactable = false;
			return;
		}
		_callback = callback;
    }
}
