using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TcpClient = NetworkLibrary.TcpClient;
using UdpClient = NetworkLibrary.UdpClient;

public class OnlinePlay : MonoBehaviour {

    public GameObject LoginOverlay;
    public GameObject ErrorOverlay;

    public void ShowJoinScreen(GameObject panel) {
        if (!GlobalSettings.LatestVersion) {
            ShowError("Failed to retrieve the server settings.\n" +
                "Make sure you have a working internet connection.");
            return;
        }
        if (!LoginStatus())
            return;
        panel.SetActive(true);
    }

    public void HideJoinScreen(GameObject panel) {
        panel.SetActive(false);
    }

    public void CreateLobby() {
        if (!GlobalSettings.LatestVersion) {
            ShowError("Failed to retrieve the server settings.\n" +
                "Make sure you have a working internet connection.");
            return;
        }

        if (!LoginStatus())
            return;

        UdpClient client = new UdpClient();
        client.Connect(new IPEndPoint(IPAddress.Parse(GlobalSettings.Instance.ServerIp),
            GlobalSettings.Instance.ServerPort));

        string response = GetResponse(client.Socket, "Request:CreateLobby");
        if (response == null) {
            EndSession();
            return;
        }
        SetNameHandshake(response);
    }




    public void JoinLobby(InputField input) {
        if (string.IsNullOrEmpty(input.text)) {
            input.text = "";
            GameObject.Find("JoinLobbyOverlay").SetActive(false);
            return;
        }

        UdpClient client = new UdpClient();
        client.Connect(new IPEndPoint(IPAddress.Parse(GlobalSettings.Instance.ServerIp), GlobalSettings.Instance.ServerPort));
        string response = GetResponse(client.Socket, string.Format("Request:JoinLobby:{0}", input.text));
        if (response == null) {
            EndSession();
            return;
        }
        SetNameHandshake(response);
    }

    private void ShowError(string error) {
        ErrorOverlay.GetComponentInChildren<Text>().text = error;
        ErrorOverlay.SetActive(true);
    }

    public void SetNameHandshake(string response) {
        LoginStatus login = GetLoginStatus();
        SessionData session = GetSession();

        IPEndPoint lobbyIp = GetLobbyIp(response);
        if (lobbyIp == null) {
            EndSession();
            return;
        }
        // TODO: Method v
        Socket tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        tcp.Connect(lobbyIp);
        TcpClient tcpClient = new TcpClient(tcp);


        string auth = GetResponse(tcpClient.Socket, string.Format("[Notify:SetName:{0}]", login.Name));
        if (auth == null) {
            EndSession();
            return;
        }

        string[] messages =
            auth.Split(new [] { "]" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.TrimStart('['))
                .ToArray();

        SplitData first = messages[0].GetFirst();
        if (first.CommandType == "Error") {
            ShowError(first.Values.Split(':').Last());
            return;
        }

        string[] authData = messages[0].GetFirst().Values.GetFirst().Values.Split(new [] {"|",}, StringSplitOptions.RemoveEmptyEntries);
        session.Guid = authData[0];
        session.OwnId = Int32.Parse(authData[1]);
        session.OwnName = authData[2];
        session.LobbyId = authData[3];
        session.LobbyConnection = tcpClient;

        string[] playerData = messages[1].GetFirst().Values.GetFirst().Values.Split(new[] { "|", }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        TempPlayer[] players = new TempPlayer[playerData.Length];
        for (int i = 0; i < players.Length; i++) {
            string info = playerData[i].Trim('(', ')');
            string[] splt = info.Split(':');
            players[i] = new TempPlayer() {
                Name = splt[0],
                Id = int.Parse(splt[1]),
                Ready = bool.Parse(splt[2]),
                IsHost = bool.Parse(splt[3])
            };
        }

        session.Players = players;
        SceneManager.LoadScene("Lobby");
    }


    #region Info getters

    public bool LoginStatus() {
        GameObject go = GameObject.Find("LoginStatus");
        if (go == null || string.IsNullOrEmpty(go.GetComponent<LoginStatus>().Name)) {
            LoginOverlay.SetActive(true);
            return false;
        }
        return true;
    }

    public IPEndPoint GetLobbyIp(string response) {
        SplitData command = response.GetFirst();
        if (command.CommandType == "Error") {
            // TODO: Handle error
            EndSession();
            return null;
        }
        string[] data = command.Values.Split('|');
        IPEndPoint lobby = new IPEndPoint(IPAddress.Parse(data[0]), int.Parse(data[1]));
        return lobby;
    }

    public string GetResponse(Socket socket, string query) {
        socket.Send(new ASCIIEncoding().GetBytes(query));
        if (!socket.Poll(1000000, SelectMode.SelectRead)) {
            Debug.LogError("Socket did not respond within 1000000 Micro seconds");
            return null;
        }
        byte[] buffer = new byte[4096];
        int received = socket.Receive(buffer);
        return new ASCIIEncoding().GetString(buffer, 0, received);
    }

    public SessionData GetSession() {
        GameObject lobbySettings = GameObject.Find("Lobby Settings");
        if (lobbySettings != null) {
            SessionData disp = lobbySettings.GetComponent<SessionData>();
            if (disp.ServerCom != null)
                disp.ServerCom.Dispose();
            if (disp.LobbyConnection != null)
                disp.LobbyConnection.Dispose();
            DestroyImmediate(lobbySettings);
        }
        lobbySettings = new GameObject("Lobby Settings");
        SessionData session = lobbySettings.AddComponent<SessionData>();
        DontDestroyOnLoad(lobbySettings);
        return session;
    }

    public void EndSession() {
        GameObject go = GameObject.Find("Lobby Settings");
        if (go != null)
            Destroy(go);
    }

    internal LoginStatus GetLoginStatus() {
        GameObject go = GameObject.Find("LoginStatus");
        if(go != null)
            return go.GetComponent<LoginStatus>();
        Debug.LogError("User is not logged in. Using default name for debugging");
        go = new GameObject("LoginStatus");
        LoginStatus status = go.AddComponent<LoginStatus>();
        status.Name = "Bert " + new System.Random().Next(100);
        return status;
    }
    #endregion

}
