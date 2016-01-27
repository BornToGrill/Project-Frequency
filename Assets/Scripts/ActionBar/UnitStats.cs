using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitStats : MonoBehaviour {

	public Text Attack;
	public Text Defense;
	private Image _image;


	// Use this for initialization
	void Start () {
		_image = GetComponent<Image> ();
		Hide ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hide() {
		Attack.enabled = false;
		Defense.enabled = false;
		_image.enabled = false;
	}

	public void Set(int attackValue, int defenseValue) {
		Attack.enabled = true;
		Defense.enabled = true;
		_image.enabled = true;
		Attack.text = attackValue.ToString ();
		Defense.text = defenseValue.ToString ();
	}
}
