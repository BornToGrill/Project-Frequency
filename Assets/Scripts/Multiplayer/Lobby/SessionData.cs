using NetworkLibrary;
using UnityEngine;

public class SessionData : MonoBehaviour {

    public string OwnName { get; set; }
    public int OwnId { get; set; }
    public string Guid { get; set; }
    public string LobbyId { get; set; }
    public TempPlayer[] Players { get; set; }
    public TcpClient LobbyConnection { get; set; }
    internal CommunicationHandler ServerCom { get; set; }
}
