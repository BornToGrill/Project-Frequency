using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class CreateAccountController : MonoBehaviour {

    public InputField Username;
    public InputField DisplayName;
    public InputField Password;

    public GameObject CreateAccountDialog;
    public GameObject LoginDialog;

    public Text ErrorField;

    public void OnEnable() {
        Username.text = "";
        DisplayName.text = "";
        Password.text = "";
        ErrorField.text = "";
    }

    public void CreateAccount() {
        if (Username.text.Length == 0 || DisplayName.text.Length == 0 || Password.text.Length == 0) {
            ErrorField.text = "Input fields can not be empty.";
            ErrorField.color = Color.red;
            return;
        }

        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        s.Connect(GlobalSettings.Instance.ServerIp, GlobalSettings.Instance.ServerPort);
        SecurePassword pass = Cryptography.GetSaltHash(Password.text);
        string response = GetResponse(s,
            string.Format("Request:CreateAccount:{0}|{1}|{2}|{3}", Username.text, DisplayName.text, pass.Hash, pass.Salt));
        if (response == null)
            return;
        else {
            Username.text = "";
            DisplayName.text = "";
            Password.text = "";
            ErrorField.text = "Account created successfully.";
            ErrorField.color = Color.green;
        }
    }

    public void HideCreateAccountDialog() {
        CreateAccountDialog.SetActive(false);
        LoginDialog.SetActive(true);
    }

    private string GetResponse(Socket s, string query) {
        s.Send(new ASCIIEncoding().GetBytes(query));
        if (!s.Poll(10000000, SelectMode.SelectRead)) {  // 10 seconds
            ErrorField.text = "Failed to connect to the server.";
            ErrorField.color = Color.red;
            return null;
        }
        byte[] buffer = new byte[2048];
        int received = s.Receive(buffer);
        string response = new ASCIIEncoding().GetString(buffer, 0, received);
        SplitData data = response.GetFirst();
        if (data.CommandType == "Error") {
            ErrorField.text = "Could not create account";
            ErrorField.color = Color.red;
            return null;
        }
        return data.Values.GetFirst().Values;
    }


}
