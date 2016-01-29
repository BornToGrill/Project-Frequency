﻿using System;
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
    public void ShowJoinScreen(GameObject panel) {
        panel.SetActive(true);
    }

    public void CreateLobby() {

        SessionData session = GetSession();

        UdpClient client = new UdpClient();
        client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9500));

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
        client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9500));
        string response = GetResponse(client.Socket, string.Format("Request:JoinLobby:{0}", input.text));
        if (response == null) {
            EndSession();
            return;
        }
        SetNameHandshake(response);
    }

    private void SetNameHandshake(string response) {
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
            auth.Split(new string[] { "]" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.TrimStart('['))
                .ToArray();
        string[] authData = messages[0].GetFirst().Values.GetFirst().Values.Split(new [] {"|",}, StringSplitOptions.RemoveEmptyEntries);
        session.Guid = authData[0];
        session.OwnId = Int32.Parse(authData[1]);
        session.OwnName = authData[2];
        session.LobbyId = authData[3];
        session.LobbyConnection = tcpClient;

        string[] playerData = messages[1].GetFirst().Values.GetFirst().Values.Split(new[] { "|", }, StringSplitOptions.RemoveEmptyEntries).ToArray();
        TempPlayer[] players = new TempPlayer[playerData.Length];
        for (int i = 0; i < playerData.Length; i++)
            players[i] = new TempPlayer() { Name = playerData[i] };

        session.Players = players;
        SceneManager.LoadScene("Lobby");
    }


    #region Info getters
    IPEndPoint GetLobbyIp(string response) {
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

    string GetResponse(Socket socket, string query) {
        socket.Send(new ASCIIEncoding().GetBytes(query));
        if (!socket.Poll(1000000, SelectMode.SelectRead)) {
            Debug.LogError("Socket did not respond within 1000000 Micro seconds");
            return null;
        }
        byte[] buffer = new byte[4096];
        int received = socket.Receive(buffer);
        return new ASCIIEncoding().GetString(buffer, 0, received);
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
        Debug.LogError("User is not logged in. Using default name for debugging");
        go = new GameObject("LoginStatus");
        LoginStatus status = go.AddComponent<LoginStatus>();
        status.Name = "Bert " + new System.Random().Next(100);
        return status;
    }
    #endregion

}
