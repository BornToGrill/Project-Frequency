using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {


    public void OnPointerClick(PointerEventData e) {
        TileController tile = gameObject.GetComponent<TileController>();
        if (tile == null)
            throw new InvalidOperationException(
                "SelectionController class is to be used on the Tile Prefab with a TileController");
        Board board = tile.gameObject.transform.parent.gameObject.GetComponent<Board>();
        board.OnTileSelected(tile.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData) {

    }

    public void OnPointerExit(PointerEventData eventData) {

    }


    internal void OnObjectDeselect() {
        var tileControl = gameObject.GetComponent<TileController>();
        tileControl.Environment = tileControl.Environment;
        GameObject.Find("ActionBar").GetComponent<ActionBarController>().Clear();
    }


}