using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour, IPointerClickHandler {

	public GameController gameController;

	public void OnPointerClick(PointerEventData eventData) {
	    GameObject go = GameObject.Find("Board");
	    if (go.GetComponent<StateController>() == null)
	        gameController.NextTurn();
            gameController.GetComponent<Board> ().DeselectTile (DeselectStatus.Both, null);
	    else {
	        StateController state = go.GetComponent<StateController>();
            state.ServerComs.Invoke.TurnEnd();
	    }
}