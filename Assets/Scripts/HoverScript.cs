using UnityEngine;
using System.Collections;

public class HoverScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        //renderer.material.color = Color.black;
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        tm.color = Color.white;
    }

    void OnMouseEnter()
    {
        //renderer.material.color = Color.red;
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        tm.color = Color.red;
    }

    void OnMouseExit()
    {
        //renderer.material.color = Color.black;
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        tm.color = Color.white;
    }

    public bool isStart;
    public bool isQuit;

    void OnMouseUp()
    {
        if (isStart)
        {
            Application.LoadLevel();
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
