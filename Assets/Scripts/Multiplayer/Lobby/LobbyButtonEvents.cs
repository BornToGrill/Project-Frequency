using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyButtonEvents : MonoBehaviour {

    private string _buttonType;
    private bool _enabled;

    public Sprite DefaultStart;
    public Sprite GrayStart;
    public Sprite DefaultReady;
    public Sprite GrayReady;

    public string ButtonType {
        get { return _buttonType; }
        set {
            _buttonType = value;
            //TODO: Make sprites instead of text
            if (value == "Ready") {
                SetPlayer();
            }
            else {

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

    private void SetPlayer() {
        GetComponent<Image>().sprite = GrayReady;
        SpriteState states = new SpriteState() {
            highlightedSprite = DefaultReady
        };
        GetComponent<Button>().spriteState = states;
    }

    private void SetHost() {
        GetComponent<Image>().sprite = Enabled ? DefaultStart : GrayStart;
        SpriteState states = new SpriteState() {
            highlightedSprite = DefaultStart
        };
        GetComponent<Button>().spriteState = states;
    }
}
