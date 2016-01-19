using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StatusBarMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
	Text btn;

	void Start() {
		btn = gameObject.GetComponent<Text>();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		btn.color = Color.red;
	}

	public void OnPointerExit(PointerEventData eventData) {
		btn.color = Color.black;
	}

	public void OnPointerClick(PointerEventData eventData) {
		SceneManager.LoadScene ("MainMenu");
	}
}