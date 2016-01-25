using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour, IPointerClickHandler {

	public GameController gameController;

	public void OnPointerClick(PointerEventData eventData)
	{
		gameController.NextTurn ();
	}
}