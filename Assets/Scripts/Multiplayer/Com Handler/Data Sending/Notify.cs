using NetworkLibrary;

public enum MoveType {
    Empty,
    Merge,
    Attack
};

class Notify {


    private TcpClient _tcpClient;
    private string _guid;

    internal Notify(TcpClient tcpClient, string guid) {
        _tcpClient = tcpClient;
        _guid = guid;
    }

    internal void GameStart() {
        _tcpClient.Send(string.Format("[Notify:GameStart:{0}]", _guid));
    }

    internal void Move(MoveType type, TileController start, TileController stop) {
        string moveType = type == MoveType.Empty
            ? "MoveToEmpty"
            : type == MoveType.Merge ? "MoveToMerge" : type == MoveType.Attack ? "MoveToAttack" : "";

        _tcpClient.Send(string.Format("[Notify:{0}:{1}|({2}:{3})|({4}:{5})]", moveType, _guid, start.Position.x,
            start.Position.y, stop.Position.x, stop.Position.y));
    }

    internal void Attack(TileController start, TileController target) {
        _tcpClient.Send(string.Format("[Notify:Attack:{0}|({1}:{2})|({3}:{4})]", _guid, start.Position.x,
            start.Position.y, target.Position.x, target.Position.y));
    }

    internal void CreateUnit(TileController target, string unitType) {
        _tcpClient.Send(string.Format("[Notify:UnitCreated:{0}|({1}:{2})|{3}]", _guid, target.Position.x,
            target.Position.y, unitType));
    }

    internal void SplitUnit(TileController start, TileController stop, int amount) {
        _tcpClient.Send(string.Format("[Notify:SplitUnit:{0}|({1}:{2})|({3}:{4})|{5}]", _guid, start.Position.x,
            start.Position.y, stop.Position.x, stop.Position.y, amount));
    }

    internal void CashChanged(int newValue) {
        _tcpClient.Send(string.Format("[Notify:CashChanged:{0}|{1}]", _guid, newValue));
    }

    internal void GameWon() {
        _tcpClient.Send(string.Format("[Notify:GameWon:{0}]", _guid));
    }

    internal void GameLost() {
        _tcpClient.Send(string.Format("[Notify:GameLost:{0}]", _guid));
    }

    internal void GameLoaded() {
        _tcpClient.Send(string.Format("[Notify:GameLoaded:{0}]", _guid));
    }

    internal void LobbyReady(bool ready) {
        _tcpClient.Send(string.Format("[Notify:PlayerReady:{0}|{1}]", _guid, ready));
    }
}
