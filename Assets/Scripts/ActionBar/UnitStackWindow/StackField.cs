using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackField : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	private Image _image;
	public StackField previous;

	void Start() {
		_image = GetComponent<Image> ();
	}

	public void Set(Sprite sprite){
		_image.sprite = sprite;
		_image.enabled = true;
	}

	public void Hide() {
		_image.sprite = null;
		_image.color = Color.white;
		_image.enabled = false;
	}

	public void OnPointerClick(PointerEventData e){
		
	}

	public void OnPointerEnter(PointerEventData e){
		_image.color = Color.blue;
		if (previous != null)
			previous.OnPointerEnter (e);
	}

	public void OnPointerExit(PointerEventData e){
		_image.color = Color.white;
	}
}