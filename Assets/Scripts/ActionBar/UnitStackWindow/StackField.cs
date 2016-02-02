using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackField : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
	private Image _image;
    private Color _selectionColor;
    private bool _selectable;
    private Action<int> _callback;

    public StackField Previous;
    public StackField Next;

    internal bool IsSplitting;

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
	    IsSplitting = false;
	}

	public void OnPointerClick(PointerEventData e) {
	    if (!_selectable)
	        return;
        IsSplitting = true;
	    int count = 0;
	    StackField curr = this;
	    while (curr != null) {
	        count++;
            curr.IsSplitting = true;
	        curr = curr.Previous;
	    }
	    curr = Next;
	    while (curr != null) {
	        curr.IsSplitting = false;
	        curr = curr.Next;
	    }
	    _callback.Invoke(count);
	}

	public void OnPointerEnter(PointerEventData e) {
	    if (!_selectable)
	        return;
	    _image.color = _selectionColor;
	    StackField field = Next;
	    while (field != null) {
	        field._image.color = Color.white;
	        field = field.Next;
	    }

	    field = Previous;
	    while (field != null) {
	        field._image.color = _selectionColor;
	        field = field.Previous;
	    }
	}

	public void OnPointerExit(PointerEventData e) {
	    if (!_selectable)
	        return;

        if (!IsSplitting)
            _image.color = Color.white;
	    StackField field = Previous;
	    while (field != null) {
	        if (!field.IsSplitting)
	            field._image.color = Color.white;
	        field = field.Previous;
	    }
	    field = Next;
	    while (field != null) {
	        if (!field.IsSplitting)
	            field._image.color = Color.white;
	        else
	            field._image.color = field._selectionColor;
	        field = field.Next;
	    }
	}
}