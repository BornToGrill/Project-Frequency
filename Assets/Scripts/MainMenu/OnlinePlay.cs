using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UdpClient = NetworkLibrary.UdpClient;

public class OnlinePlay : MonoBehaviour {

    public void CreateLobby() {

        SessionData session = GetSession();

        UdpClient client = new UdpClient();
        client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9500));
        client.Send("Request:CreateLobby");
        if (!client.Socket.Poll(1000000, SelectMode.SelectRead)) {
            //TODO : Handle error
            Debug.LogError("Failed to receive lobby ip in 10000 of something");
            EndSession();
            return;
        }
        byte[] buffer = new byte[4096];
        int received = client.Socket.Receive(buffer);

        string response = new ASCIIEncoding().GetString(buffer, 0, received);
        SplitData command = response.GetFirst();
        if (command.CommandType == "Error") {
            // TODO: Handle error
            EndSession();
            return;
        }
        else {
            string[] data = command.Values.Split('|');
            IPEndPoint lobby = new IPEndPoint(IPAddress.Parse(data[0]), int.Parse(data[1]));
            session.LobbyIp = lobby;

            SceneManager.LoadScene("Lobby");
        }
    }

    public void ShowJoinScreen(GameObject panel) {
        panel.SetActive(true);
    }

    public void JoinLobby(InputField input) {
        if (string.IsNullOrEmpty(input.text)) {
            input.text = "";
            GameObject.Find("JoinLobbyOverlay").SetActive(false);
            return;
        }

        SessionData session = GetSession();

        UdpClient client = new UdpClient();
        client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9500));
        client.Send(string.Format("Request:JoinLobby:{0}", input.text));
        if (!client.Socket.Poll(1000000, SelectMode.SelectRead)) {
            //TODO : Handle error
            Debug.LogError("Failed to receive lobby ip in 10000 of something");
            EndSession();
            return;
        }
        byte[] buffer = new byte[4096];
        int received = client.Socket.Receive(buffer);

        string response = new ASCIIEncoding().GetString(buffer, 0, received);
        SplitData command = response.GetFirst();
        if (command.CommandType == "Error") {
            // TODO: Handle error
            EndSession();
            return;
        }
        else {
            string[] data = command.Values.Split('|');
            IPEndPoint lobby = new IPEndPoint(IPAddress.Parse(data[0]), int.Parse(data[1]));
            session.LobbyIp = lobby;
            SceneManager.LoadScene("Lobby");
        }
    }

    SessionData GetSession() {
        GameObject lobbySettings = GameObject.Find("Lobby Settings");
        if (lobbySettings != null)
            Destroy(lobbySettings);
        lobbySettings = new GameObject("Lobby Settings");
        SessionData session = lobbySettings.AddComponent<SessionData>();
        DontDestroyOnLoad(lobbySettings);
        return session;
    }

    void EndSession() {
        GameObject go = GameObject.Find("Lobby Settings");
        if (go != null)
            Destroy(go);
    }

    LoginStatus GetLoginStatus() {
        GameObject go = GameObject.Find("LoginStatus");
        if(go != null)
            return go.GetComponent<LoginStatus>();
        go = new GameObject("LoginStatus");
        return go.AddComponent<LoginStatus>();
    }

}
