using UnityEngine;
using System.Collections;
using UnityEngine.UI;
<<<<<<< HEAD
using UnityEngine.EventSystems;

public class ActionButtonController : MonoBehaviour, IPointerClickHandler {
    private Text _buttonText;
    private Callback _callback;

    void Awake()
    {
        _buttonText = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
    }
	
	public void OnPointerClick(PointerEventData data)
    {
        if (_callback != null)
            _callback.Invoke();
    }

    public void Initialize(string text, Callback callback, float offset)
    {
        this._callback = callback;
        _buttonText.text = text;
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.pivot = new Vector2(0.5f, 0.5f); 
        rectTransform.localScale = new Vector3(1, 1, 0);
        rectTransform.anchoredPosition = new Vector2(offset+50, 50);
    }
=======

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
>>>>>>> tilevalues
}
