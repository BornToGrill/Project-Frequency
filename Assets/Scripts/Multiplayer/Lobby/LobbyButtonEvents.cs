using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtonEvents : MonoBehaviour {

    private string _buttonType;
    private bool _enabled;
    public string ButtonType {
        get { return _buttonType; }
        set {
            _buttonType = value;
            //TODO: Make sprites instead of text
            if (value == "Ready")
                GetComponentInChildren<Text>().text = "Ready";
            else {
                GetComponentInChildren<Text>().text = "";
                Ready = false;
            }
        }
    }

    public bool Enabled {
        get { return _enabled; }
        set {
            _enabled = value;
            if (value)
                GetComponent<Image>().color = Color.white;
            else
                GetComponent<Image>().color = Color.gray;
        }
    }
    public bool Ready { get; set; }


    public void GameReadyOrStart() {
        GameObject go = GameObject.Find("Canvas");
        LobbyController lobby = go.GetComponent<LobbyController>();
        if (_buttonType == "Start")
            lobby.ComHandler.Notify.GameStart();
        else {
            Ready = !Ready;
            lobby.ComHandler.Notify.LobbyReady(Ready);
        }
    }

    public void LeaveLobby() {
        GameObject go = GameObject.Find("Lobby Settings");
        if (go != null) {
            SessionData session = go.GetComponent<SessionData>();
            if(session.LobbyConnection != null)
                session.LobbyConnection.Dispose();
            if (session.ServerCom != null)
                session.ServerCom.Dispose();
            Destroy(go);

        }
        SceneManager.LoadScene("GameSelection");
    }
}
