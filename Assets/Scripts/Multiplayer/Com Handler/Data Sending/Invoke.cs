using NetworkLibrary;

internal class Invoke {

    private TcpClient _tcpClient;
    private string _guid;

    internal Invoke(TcpClient tcpClient, string guid) {
        _tcpClient = tcpClient;
        _guid = guid;
    }

    internal void TurnEnd() {
        _tcpClient.Send(string.Format("[Notify:EndTurn:{0}", _guid));
    }
}
