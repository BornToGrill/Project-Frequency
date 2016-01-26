using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Callback(string name);

public class ActionBarController : MonoBehaviour {
    public float margin;
    public GameObject ActionButtonPrefab;
	public GameObject SoldierButtonPrefab;
	public GameObject RobotButtonPrefab;
	public GameObject TankButtonPrefab;
	public GameObject BoatButtonPrefab;
	public GameObject BarracksButtonPrefab;

    private List<GameObject> _buttons;

    void Start () { 
        _buttons = new List<GameObject>();
    }
    	
	public void AddButton(string text, Callback callback, bool clickable) {
        float offset = 0;
        foreach ( GameObject currentButton in _buttons)
        {
            RectTransform rectTransform = (RectTransform)currentButton.transform;
            offset += rectTransform.rect.width;
			offset += margin;
        }

		GameObject button;
		switch (text)
		{
		case "Soldier":
			button = Instantiate(SoldierButtonPrefab) as GameObject;
			break;
		case "Robot":
			button = Instantiate(RobotButtonPrefab) as GameObject;
			break;
		case "Tank":
			button = Instantiate(TankButtonPrefab) as GameObject;
			break;
		case "Boat":
			button = Instantiate(BoatButtonPrefab) as GameObject;
			break;
		case "Barracks":
			button = Instantiate(BarracksButtonPrefab) as GameObject;
			break;
		default:
			button = Instantiate(ActionButtonPrefab) as GameObject;
			break;
		}

		button.transform.SetParent(transform);
		button.GetComponent<ActionButtonController>().Initialize(text, callback, offset, clickable);
        _buttons.Add(button);
    }

    public void Clear()
    {
        foreach(GameObject go in _buttons)
            GameObject.Destroy(go);
        _buttons.Clear();
    }
}
