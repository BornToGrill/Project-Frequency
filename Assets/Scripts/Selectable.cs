using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerClickHandler {
	private EventSystem _eventSystem;
	private GameObject go;

	void start() {
		go = GameObject.Find ("EventSystem");
		_eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
	}

	public void OnPointerClick(PointerEventData data) {
		_eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		if (_eventSystem != null) {
			_eventSystem.SetSelectedGameObject (gameObject);
		}
	}
}