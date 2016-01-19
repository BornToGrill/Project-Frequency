using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoverScript : MonoBehaviour {

	public bool isStart;
	public bool isQuit;
	public bool isInstructions;
	public bool isSettings;

    // Use this for initialization
    void Start()
    {
        //renderer.material.color = Color.black;
		Text tm = gameObject.GetComponent<Text>();
		tm.color = Color.white;
    }

    public void OnMouseEnter()
    {
        //renderer.material.color = Color.red;
		Text tm = gameObject.GetComponent<Text>();
		tm.color = Color.red;
    }

	public void OnMouseExit()
    {
        //renderer.material.color = Color.black;
        //TextMesh tm = gameObject.GetComponent<TextMesh>();
        //tm.color = Color.white;
		Text tm = gameObject.GetComponent<Text>();
		tm.color = Color.white;
    }
	public void OnMouseUp()
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
			SceneManager.LoadScene("Instructions");
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }
}
