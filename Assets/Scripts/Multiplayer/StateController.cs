using System.Linq;
using UnityEngine;

public class StateController : MonoBehaviour, IInvokable {

    private CommunicationHandler _comHandler;
    private GameController _gameController;

    public string Guid;
    public int CornerId;

	void Start () {
	    _gameController = GetComponent<GameController>();
	    _comHandler = new CommunicationHandler(this);
	}

    public void Authenticated(string guid, int id) {
        Guid = guid;
        CornerId = id;
        Debug.Log(Guid + " : " + CornerId);
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

    public void GameStart() {
        
    }
}
