using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StackWindow : MonoBehaviour {

	public StackField[] StackFields;
	public Sprite[] Sprites;
	private Image _image;

	void Start()
	{
		_image = GetComponent<Image> ();
	}

	void Show(string unitName, int amount)
	{
		Sprite sprite = new Sprite ();

		switch (unitName) {
		case "Soldier":
			sprite = Sprites [0];
			break;
		case "Tank":
			sprite = Sprites [1];
			break;
		case "Robot":
			sprite = Sprites [2];
			break;
		}

		_image.enabled = true;
		for (int i = 0; i < amount; i++) {
			StackFields [i].Set (sprite);
		}
	}

	void Hide()
	{
		_image.enabled = false;
		foreach (StackField stackField in StackFields) {
			stackField.Hide ();
		}
	}
}