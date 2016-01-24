using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {


    public void OnPointerClick(PointerEventData e) {
        ThrowErrorIfNecessary();
        gameObject.transform.parent.gameObject.GetComponent<Board>().OnTileClicked(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ThrowErrorIfNecessary();
        gameObject.transform.parent.gameObject.GetComponent<Board>().OnTileEnter(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ThrowErrorIfNecessary();
        gameObject.transform.parent.gameObject.GetComponent<Board>().OnTileLeave(gameObject);
    }


    internal void OnObjectDeselect() {
        var tileControl = gameObject.GetComponent<TileController>();
        tileControl.Environment = tileControl.Environment;
        GameObject.Find("ActionBar").GetComponent<ActionBarController>().Clear();
    }

    private void ThrowErrorIfNecessary() {
        if(!gameObject.name.Contains("Tile"))
            throw new InvalidOperationException(GetType().FullName + " class is to be used on the Tile Prefab only.");
    }


}