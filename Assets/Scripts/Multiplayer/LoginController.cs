using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class LoginController : MonoBehaviour {

    public InputField Username;
    public InputField Password;

    public GameObject LoginDialog;
    public GameObject CreateAccountDialog;

    public Text ErrorText;

    public void Login() {
        
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        s.Connect("127.0.0.1", 9500);
        GameObject login = GameObject.Find("LoginStatus");
        if (login != null)
            Destroy(login);
        login = new GameObject("LoginStatus");
        LoginStatus status = login.AddComponent<LoginStatus>();
        string response = GetResponse(s, string.Format("Request:Login:{0}|{1}", Username.text, Password.text));
        if (response == null) {
            Destroy(login);
            return; //TODO: Invalid acc error
        }

        status.Name = response;
        DontDestroyOnLoad(login);
        HideLoginDialog();
    }

    public void ShowCreateAccountDialog() {
        ErrorText.text = "";
        Username.text = "";
        Password.text = "";
        LoginDialog.SetActive(false);
        CreateAccountDialog.SetActive(true);
    }

    public void HideLoginDialog() {
        ErrorText.text = "";
        Username.text = "";
        Password.text = "";
        LoginDialog.SetActive(false);
        CreateAccountDialog.SetActive(false);
    }

    public void ClearError() {
        ErrorText.text = "";
    }


    private string GetResponse(Socket s, string query) {
        s.Send(new ASCIIEncoding().GetBytes(query));
        if (!s.Poll(10000000, SelectMode.SelectRead)) { // 10 seconds
            ErrorText.text = "Failed to connect to the server.";
            return null;
        }
        byte[] buffer = new byte[2048];
        int received = s.Receive(buffer);
        string response = new ASCIIEncoding().GetString(buffer, 0, received);
        SplitData data = response.GetFirst();
        if (data.CommandType == "Error") {
            ErrorText.text = "Invalid login credentials.";
            return null;
        }
        return data.Values.GetFirst().Values;
    }

}
