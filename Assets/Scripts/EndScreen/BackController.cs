using UnityEngine;
using System.Collections;
using System.Net;
using NetworkLibrary;
using UnityEngine.SceneManagement;

public class BackController : MonoBehaviour {



    public void BackClicked() {

        GameObject lobby = GameObject.Find("Lobby Settings");
        if (lobby == null) {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        SessionData session = lobby.GetComponent<SessionData>();
        if (session == null || session.Players == null || 
            string.IsNullOrEmpty(session.LobbyId)) {
            Destroy(lobby);
            SceneManager.LoadScene("MainMenu");
            return;
        }
        SceneManager.LoadScene("Lobby");

    }
}
