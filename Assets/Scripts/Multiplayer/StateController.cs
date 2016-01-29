using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateController : MonoBehaviour, IInvokable, INotifiable {

    public GameObject[] Units;

    internal CommunicationHandler ServerComs;
    private GameController _gameController;

    internal string Guid;
    internal int CornerId;

	void Start () {
	    _gameController = GetComponent<GameController>();
	    //ServerComs = new CommunicationHandler(this, this, null);
	    SessionData session = GameObject.Find("Lobby Settings").GetComponent<SessionData>();
	    ServerComs = session.ServerCom;
	    Guid = session.Guid;
	    CornerId = session.OwnId;
	    ServerComs.SetGuid(Guid);
	    ServerComs.SetProcessor(new DataProcessor(this, this, null));
	}

    public void Send(string message) {
        ServerComs.SendTcp(message);
    }

    #region IInvokable Implementation Members

    public void SetPlayers(string[] names, int[] ids) {
        for (int i = 0; i < names.Length; i++)
            _gameController.Players.Single(x => x.PlayerId == ids[i]).Name = names[i];
    }

    public void CreateUnit(int targetX, int targetY, string unitType, int ownerId) {
        _gameController.MultiplayerActionQueue.Enqueue(() => {
            //TODO: Add locks
            Board board = _gameController.GetComponent<Board>();
            TileController target = board._tiles[targetX, targetY].GetComponent<TileController>();
            if (target.Unit != null) {
                target.Unit.StackSize++;
                return;
            }
            GameObject go =
                (GameObject)
                    Instantiate(Units.Single(x => x.name == unitType), new Vector3(targetX, targetY),
                        Quaternion.identity);
            target.Unit = go.GetComponent<BaseUnit>();
            target.Unit.Owner = _gameController.Players.Find(x => x.PlayerId == ownerId);
            target.Unit.GetComponent<SpriteRenderer>().color = target.Unit.Owner.Color;
        });
    }

    public void MoveToEmpty(int startX, int startY, int endX, int endY) {
        _gameController.MultiplayerActionQueue.Enqueue(() => {
            Board board = _gameController.GetComponent<Board>();
            TileController start = board._tiles[startX, startY].GetComponent<TileController>();
            TileController stop = board._tiles[endX, endY].GetComponent<TileController>();
            PathFindingResult result = Pathfinding.FindPath(start, stop);
            WaterUnitEventController waterEvent = start.Unit.GetComponent<WaterUnitEventController>();
            if (waterEvent != null) {
                waterEvent.MoveToEmpty(start, result.Path);
            }
            else {
                LandUnitEventController unitEvent = start.Unit.GetComponent<LandUnitEventController>();
                unitEvent.MoveToEmpty(start, result.Path);
            }
        });
    }

    public void MoveToMerge(int startX, int startY, int endX, int endY) {
        _gameController.MultiplayerActionQueue.Enqueue(() => {
            Board board = _gameController.GetComponent<Board>();
            TileController start = board._tiles[startX, startY].GetComponent<TileController>();
            TileController stop = board._tiles[endX, endY].GetComponent<TileController>();
            PathFindingResult result = Pathfinding.FindPath(start, stop);
            WaterUnitEventController waterEvent = start.Unit.GetComponent<WaterUnitEventController>();
            if (waterEvent != null) {
                waterEvent.MoveToMerge(start, result.Path);
            }
            else {
                LandUnitEventController unitEvent = start.Unit.GetComponent<LandUnitEventController>();
                unitEvent.MoveToMerge(start, result.Path);
            }
        });
    }

    public void MoveToAttack(int startX, int startY, int endX, int endY) {
        _gameController.MultiplayerActionQueue.Enqueue(() => {
            Board board = _gameController.GetComponent<Board>();
            TileController start = board._tiles[startX, startY].GetComponent<TileController>();
            TileController stop = board._tiles[endX, endY].GetComponent<TileController>();
            PathFindingResult result = Pathfinding.FindPath(start, stop);
            WaterUnitEventController waterEvent = start.Unit.GetComponent<WaterUnitEventController>();
            if (waterEvent != null) {
                waterEvent.MoveToAttack(start, result.Path);
            }
            else {
                LandUnitEventController unitEvent = start.Unit.GetComponent<LandUnitEventController>();
                unitEvent.MoveToAttack(start, result.Path);
            }
        });
    }

    public void CashChanged(int id, int newValue) {
        _gameController.MultiplayerActionQueue.Enqueue(() => {
            _gameController.Players.Find(x => x.PlayerId == id).MoneyAmount = newValue;
        });
    }

    #endregion

    #region INotifiable Implementation Members

    public void EndTurn(string name, int id) {
        _gameController.QueueMultiplayerAction(() => _gameController.NextTurn(id));
    }

    public void GameWon(int winner, int[] losers) {
        WinCondition.Winner = _gameController.AllPlayers.Find(x => x.PlayerId == winner);
        if(losers != null)
            WinCondition.Losers = _gameController.AllPlayers.Where(x => losers.Any(c => c == x.PlayerId)).ToArray();
        _gameController.QueueMultiplayerAction(() => {
            SceneManager.LoadScene("WinScreen");
        });

    }
    #endregion
}
