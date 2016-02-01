using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoverScript : MonoBehaviour {

	public bool isStart;
	public bool isQuit;
	public bool isInstructions;
	public bool isSettings;
	public bool isMenu;

    // Use this for initialization
    void Start()
    {
		Text tm = gameObject.GetComponent<Text>();
		tm.color = Color.white;
    }

    public void OnMouseEnter()
    {
		Text tm = gameObject.GetComponent<Text>();
		tm.color = Color.red;
    }

	public void OnMouseExit()
    {
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
		if (isMenu) 
		{
			SceneManager.LoadScene("MainMenu");
		}
    }
}
