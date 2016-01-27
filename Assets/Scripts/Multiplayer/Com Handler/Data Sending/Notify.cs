using NetworkLibrary;

class Notify {


    private TcpClient _tcpClient;
    private string _guid;

    internal Notify(TcpClient tcpClient, string guid) {
        _tcpClient = tcpClient;
        _guid = guid;
    }

    internal void MoveToEmpty(TileController start, TileController stop) {
        _tcpClient.Send(string.Format("[Notify:MoveToEmpty:{0}|({1}:{2})|({3}:{4})]", _guid, start.Position.x,
            start.Position.y, stop.Position.x, stop.Position.y));
    }

    internal void CreateUnit(TileController target, string unitType) {
        _tcpClient.Send(string.Format("[Notify:UnitCreated:{0}|({1}:{2})|{3}]", _guid, target.Position.x,
            target.Position.y, unitType));
    }
}
