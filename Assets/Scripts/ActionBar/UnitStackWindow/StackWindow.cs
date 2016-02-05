using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StackWindow : MonoBehaviour {

	public StackField[] StackFields;
	private Image _image;

	void Start()
	{
		_image = GetComponent<Image> ();
	}

	public void Show(Sprite unitSprite, Color ownerColor, int amount, bool selectable, Action<int> callback)
	{
		Sprite sprite = new Sprite ();
	    sprite = unitSprite;

		_image.enabled = true;
		for (int i = 0; i < amount; i++) {
		    StackFields[i].Set(sprite, ownerColor, selectable, callback);
		}
	}

	public void Hide()
	{
		_image.enabled = false;
		foreach (StackField stackField in StackFields) {
			stackField.Hide ();
		}
	}

    public void Reset() {
        foreach (StackField field in StackFields) {
            field.Reset();
        }
    }
}