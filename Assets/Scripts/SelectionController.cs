using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectionController : MonoBehaviour, IPointerClickHandler {


    public void OnPointerClick(PointerEventData e) {
        TileController tile = gameObject.GetComponent<TileController>();
        if (tile == null)
            throw new InvalidOperationException(
                "SelectionController class is to be used on the Tile Prefab with a TileController");
        Board board = tile.gameObject.transform.parent.gameObject.GetComponent<Board>();
        board.OnTileSelected(tile.gameObject);

        // TODO:
    }

    internal void OnObjectDeselect(GameObject tile) {
        var tileControl = tile.GetComponent<TileController>();
        tileControl.Environment = tileControl.Environment;
    }


}