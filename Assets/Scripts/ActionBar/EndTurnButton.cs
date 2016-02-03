using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour, IPointerClickHandler {

    public GameController gameController;

	private Animator _animator;

    public void OnPointerClick(PointerEventData eventData) {
        GameObject go = GameObject.Find("Board");
        gameController.GetComponent<Board>().DeselectTile(DeselectStatus.Both, null);
        if (go.GetComponent<StateController>() == null) {
            gameController.NextTurn();
        }
        else {
            StateController state = go.GetComponent<StateController>();
            state.ServerComs.Invoke.TurnEnd();
        }
    }

	void Start() {
		_animator = GetComponent<Animator> ();
	}

	void Update() {
        if(gameController.CurrentPlayer != null)
		    if (gameController.CurrentPlayer.Moves == 0)
		    	_animator.Play ("Shining");
		    else
		    	_animator.Play ("Default");
	}
}