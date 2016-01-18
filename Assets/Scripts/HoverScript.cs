using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HoverScript : MonoBehaviour {
	public bool isStart;
	public bool isExit;

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
        tm.color = Color.green;
    }

    void OnMouseExit()
    {
        //renderer.material.color = Color.black;
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        tm.color = Color.white;
    }

    void OnMouseUp()
    {
        if (isStart)
			SceneManager.LoadScene("MainGame");
        if (isExit)
            Application.Quit();
    }
}
