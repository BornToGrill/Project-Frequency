using System.Net;
using UnityEngine;

public class SessionData : MonoBehaviour {

    public int OwnId { get; set; }
    public string Guid { get; set; }
    public TempPlayer[] Players { get; set; }
    public IPEndPoint LobbyIp { get; set; }
    internal CommunicationHandler ServerCom { get; set; }
}
