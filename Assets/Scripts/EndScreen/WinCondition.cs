using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class WinCondition : MonoBehaviour {
    
    private Player _baseOwner;
    public Player Winner;
    public Player[] Losers;

    void Start() {
        BaseUnit unit = gameObject.GetComponent<BaseUnit>();
        _baseOwner = unit.Owner;
    }

    public void BaseDestroyed() {
        GameObject board = GameObject.Find("Board");
        if (board != null) {
            GameController gc = board.GetComponent<GameController>();
            StateController mpController = board.GetComponent<StateController>();
            gc.RemovePlayer(_baseOwner);
            if (mpController == null) {
                if (gc.Players.Count <= 1) {
                    GameObject endData = GameObject.Find("EndGameData");
                    if (endData != null)
                        Destroy(endData);
                    endData = new GameObject("EndGameData");
                    WinCondition cond = endData.AddComponent<WinCondition>();
                    cond.Winner = gc.Players[0];
                    cond.Losers = gc.AllPlayers.Where(x => x != Winner).ToArray();
                    DontDestroyOnLoad(cond);
                    SceneManager.LoadScene("WinScreen");
                }
            }
            else {
                if(gc.Players.Count <= 1)
                    mpController.ServerComs.Notify.GameWon();
            }
        }
    }
}
