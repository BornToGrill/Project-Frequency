using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class WinCondition : MonoBehaviour {

    private Player _baseOwner;
    public static Player Winner;
    public static Player[] Losers;

    void Start() {
        BaseUnit unit = gameObject.GetComponent<BaseUnit>();
        _baseOwner = unit.Owner;
    }

    public void BaseDestroyed() {
        GameObject board = GameObject.Find("Board");
        if (board != null) {
            StateController mpController = board.GetComponent<StateController>();
            if (mpController == null) {
                GameController gc = board.GetComponent<GameController>();
                gc.RemovePlayer(_baseOwner);
                if (gc.Players.Count <= 1) {
                    Winner = gc.Players[0];
                    Losers = gc.AllPlayers.Where(x => x != Winner).ToArray();

                    SceneManager.LoadScene("WinScreen");
                }
            }
            else
                mpController.ServerComs.Notify.GameWon();
        }
    }
}
