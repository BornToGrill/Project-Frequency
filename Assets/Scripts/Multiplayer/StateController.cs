using System.Linq;
using UnityEngine;

public class StateController : MonoBehaviour, IInvokable, INotifiable {

    internal CommunicationHandler ServerComs;
    private GameController _gameController;

    internal string Guid;
    internal int CornerId;

	void Start () {
	    _gameController = GetComponent<GameController>();
	    ServerComs = new CommunicationHandler(this);
	}

    public void Send(string message) {
        ServerComs.SendTcp(message);
    }

    #region IInvokable Implementation Members
    public void Authenticated(string guid, int id) {
        Guid = guid;
        CornerId = id;
        Debug.Log(Guid + " : " + CornerId);
        ServerComs.SetGuid(guid);
    }

    public void PlayerConnected(int id, string playerName) {
        Player newPlayer = _gameController.Players.SingleOrDefault(x => x.PlayerId == id);
        newPlayer.Name = playerName;
        Debug.Log("Player connected : " + playerName);
    }

    public void SetPlayers(string[] names, int[] ids) {
        for (int i = 0; i < names.Length; i++)
            _gameController.Players.Single(x => x.PlayerId == ids[i]).Name = names[i];
        foreach (var element in _gameController.Players)
            Debug.Log("ID : " + element.PlayerId + "  |   name : " + element.Name);
    }

    public void StartGame() {

    }
    #endregion

    #region INotifiable Implementation Members

    public void EndTurn(string name, int id) {
        _gameController.NextTurn(id);
    }
    #endregion
}
