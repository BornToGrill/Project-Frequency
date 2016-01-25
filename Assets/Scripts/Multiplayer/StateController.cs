using UnityEngine;

public class StateController : MonoBehaviour, IInvokable {

    private CommunicationHandler _comHandler;

	void Awake () {
	    _comHandler = new CommunicationHandler(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayerConnected(string message) {
        Debug.Log("Player connected : " + message);
    }
}
