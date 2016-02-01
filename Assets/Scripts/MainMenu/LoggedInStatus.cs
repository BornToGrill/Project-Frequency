using UnityEngine;
using UnityEngine.UI;

public class LoggedInStatus : MonoBehaviour {

    public GameObject StatusOverlay;
    public Text NameTextbox;

	void Start () {
	    GameObject status = GameObject.Find("LoginStatus");
	    if (status != null) {
	        StatusOverlay.SetActive(true);
	        NameTextbox.text = status.GetComponent<LoginStatus>().Name;
	    }
	    else {
	        StatusOverlay.SetActive(false);
	    }
	}
}
