using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        tm.color = Color.green;
    }

    void OnMouseExit()
    {
        //renderer.material.color = Color.black;
        TextMesh tm = gameObject.GetComponent<TextMesh>();
        tm.color = Color.white;
    }

    public bool isStart;
    public bool isQuit;
    public bool isInstructions;
    public bool isSettings;

    void OnMouseUp()
    {
        if (isStart)
        {
            SceneManager.LoadScene("MainGame"); 
        }
        if (isSettings)
        {
            SceneManager.LoadScene("Settings");
        }
        if (isInstructions)
        {
            // SceneManager.LoadScene();
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
