using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void Callback();

public class ActionBarController : MonoBehaviour {
    public float margin;
    public GameObject ActionButtonPrefab;
    private List<GameObject> _actionButtons;
    // Use this for initialization
	void Start () {
        _actionButtons = new List<GameObject>();
        AddButton("hagrid", () => Debug.Log("clicked"));
        AddButton("Gogogo", Clear);
        AddButton("Hello", Bart);
        
	}
	void Bart()
    {
        AddButton("Gogogo", Clear);
    }
	// Update is called once per frame
	void Update () {
	
	}
    public void AddButton(string text, Callback callback) {
        float offset = 0;
        foreach ( GameObject currentButton in _actionButtons)
        {
            RectTransform rectTransform = (RectTransform)currentButton.transform;
            offset += rectTransform.rect.width;
            offset += margin;
        }
        
        GameObject button = Instantiate(ActionButtonPrefab) as GameObject;
        button.transform.SetParent(transform);
        button.GetComponent<ActionButtonController>().Initialize(text, callback, offset);
        _actionButtons.Add(button);
        Debug.Log(_actionButtons.Count);
    }
    public void Clear()
    {
        foreach(GameObject go in _actionButtons)
        {
            GameObject.Destroy(go);
        }
        _actionButtons.Clear();
    }
}
