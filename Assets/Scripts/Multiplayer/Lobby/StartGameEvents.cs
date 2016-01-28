using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class StartGameEvents : MonoBehaviour, IPointerClickHandler {

    public void OnPointerClick(PointerEventData eventData) {
        GameObject go = GameObject.Find("Canvas");
        LobbyController lobby = go.GetComponent<LobbyController>();
        lobby.ComHandler.Notify.GameStart();
    }
}
