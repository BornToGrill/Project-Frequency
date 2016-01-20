using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButtonController : MonoBehaviour, IPointerClickHandler {
    private Text buttonText;
    
    private Callback callback; 

	// Use this for initialization
	void Awake () {
        buttonText = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
        //Initialize("Hagrid", ()=> Debug.Log("gogogo"));
	}
	
	public void OnPointerClick(PointerEventData data)
    {
        if (callback != null)
          callback.Invoke();
        
    }

    public void Initialize(string text, Callback callback, float offset)
    {
        this.callback = callback;
        buttonText.text = text;
        RectTransform rectTransform = transform as RectTransform;
        //transform.position = new Vector3(0, 0, 0);
        //transform.localScale = new Vector3(1, 1, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f); 
        rectTransform.localScale = new Vector3(1, 1, 0);
        //rectTransform.position = new Vector3(offset, 0, 0);
        //rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(offset+50, 50);





        
        

    }
}
