using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackField : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	private Image _image;
    private Color _selectionColor;
    private bool _selectable;
    private bool _isSplitting;
    private Action<int> _callback;

    public StackField previous;

	void Start() {
		_image = GetComponent<Image> ();
	}

	public void Set(Sprite sprite, Color color, bool selectable, Action<int> callback){
		_image.sprite = sprite;
	    _image.color = Color.white;
		_image.enabled = true;
	    _selectable = selectable;
	    _selectionColor = color;
	    _callback = callback;
	}

	public void Hide() {
		_image.sprite = null;
		_image.color = Color.white;
		_image.enabled = false;
	    _selectable = false;
	}

	public void OnPointerClick(PointerEventData e) {
	    if (!_selectable)
	        return;
	    _isSplitting = true;
	    int count = 0;
	    StackField curr = this;
	    while (curr != null) {
	        count++;
	        curr = curr.previous;
	    }
	    _callback.Invoke(count);
	}

	public void OnPointerEnter(PointerEventData e) {
	    if (!_selectable || _isSplitting)
	        return;

	    _image.color = _selectionColor;
		if (previous != null)
			previous.OnPointerEnter (e);
	}

	public void OnPointerExit(PointerEventData e) {
	    if (!_selectable || _isSplitting)
	        return;

		_image.color = Color.white;
	    if (previous != null)
	        previous.OnPointerExit(e);
	}
}