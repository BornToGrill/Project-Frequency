using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Callback(string name);

public class ActionBarController : MonoBehaviour {
    public float margin;
    public GameObject ActionButtonPrefab;
    private List<GameObject> _buttons;

    void Start () { 
        _buttons = new List<GameObject>();
    }
    	
	public void AddButton(string text, Callback callback) {
        float offset = 0;
        foreach ( GameObject currentButton in _buttons)
        {
            RectTransform rectTransform = (RectTransform)currentButton.transform;
            offset += rectTransform.rect.width;
            offset += margin;
        }
        GameObject button = Instantiate(ActionButtonPrefab) as GameObject;
        button.transform.SetParent(transform);
        button.GetComponent<ActionButtonController>().Initialize(text, callback, offset);
        _buttons.Add(button);
    }

    public void Clear()
    {
        foreach(GameObject go in _buttons)
            GameObject.Destroy(go);
        _buttons.Clear();
    }
}
