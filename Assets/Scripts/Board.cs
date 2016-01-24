using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DeselectStatus { First, Second, Both, None }

public class Board : MonoBehaviour {
	public GameObject TilePrefab;
	public int BoardDimensions;
	public GameObject[,] _tiles;

    internal GameObject SelectedTile;

	void Awake () {
		_tiles = new GameObject[BoardDimensions, BoardDimensions];
		for (int x = 0; x < _tiles.GetLength (0); x++)
			for (int y = 0; y < _tiles.GetLength (1); y++)
				_tiles [x, y] = CreateTile (x, y);
		GetSurroundingTiles();

	}

	GameObject CreateTile(int posX, int posY)
	{
		GameObject go = Instantiate (TilePrefab, new Vector3(posX, posY, 0), new Quaternion()) as GameObject;
		go.transform.SetParent (transform);
		TileController tile = go.GetComponent<TileController> ();
		tile.Environment = GetEnvironment (posX, posY);
		tile.Position = new Vector2 (posX, posY);
		return go;
	}

	void GetSurroundingTiles()
	{
		for (int x = 0; x < _tiles.GetLength (0); x++)
		{
			for (int y = 0; y < _tiles.GetLength (1); y++)
			{
				TileController tile = _tiles [x, y].GetComponent<TileController> ();
				if (x > 0)
					tile.Left = _tiles [x - 1, y].GetComponent<TileController> ();
				if (x < BoardDimensions - 1)
					tile.Right = _tiles [x + 1, y].GetComponent<TileController> ();
				if (y > 0)
					tile.Up = _tiles [x, y - 1].GetComponent<TileController> ();
				if (y < BoardDimensions - 1)
					tile.Down = _tiles [x, y + 1].GetComponent<TileController> ();
			}
		}
	}

	Environment GetEnvironment(int posX, int posY)
	{
		if (posY < Math.Floor(BoardDimensions * 0.4f))
		{
			if (posX < Math.Floor(BoardDimensions * 0.4f))
				return Environment.Desert;
			if (posX > BoardDimensions * 0.6f)
				return Environment.Forest;
			return Environment.Water;
		}
		if (posY > BoardDimensions * 0.6f)
		{
			if (posX < Math.Floor(BoardDimensions * 0.4f))
				return Environment.Swamp;
			if (posX > BoardDimensions * 0.6f)
				return Environment.Ice;
			return Environment.Water;
		}
		if (posX < Math.Floor(BoardDimensions * 0.4f) || posX > BoardDimensions * 0.6f)
			return Environment.Water;
		return Environment.Island;
	}

    internal void OnTileClicked(GameObject tile) {

        if (SelectedTile == null) {
            TileController selected = tile.GetComponent<TileController>();
            if (selected.Unit != null) {
                DeselectStatus status = selected.Unit.OnFirstSelected(tile);
                SelectedTile = tile;
                DeselectTile(status, tile);
            }
        }
        else {
            TileController first = SelectedTile.GetComponent<TileController>();
            DeselectStatus status = first.Unit.OnSecondClicked(SelectedTile, tile);
            DeselectTile(status, tile);

        }
    }

    internal void OnTileEnter(GameObject tile) {
        if (SelectedTile == null) {
            //tile.GetComponent<TileController>().OnMouseEnter();
        }
        else {
            SelectedTile.GetComponent<TileController>().Unit.OnMouseEnter(SelectedTile, tile);
        }
    }

    internal void OnTileLeave(GameObject tile) {
        if (SelectedTile == null) {
            // Checking if null in case the tile has been selected.
            //tile.GetComponent<TileController>().OnMouseLeave();
        }
        else {
            SelectedTile.GetComponent<TileController>().Unit.OnMouseLeave(SelectedTile, tile);
        }
    }

    private void DeselectTile(DeselectStatus status, GameObject lastSelected) {
        switch (status) {
            case DeselectStatus.First:
                if (SelectedTile != null) {
                    SelectedTile.GetComponent<SelectionController>().OnObjectDeselect();//TODO: Remove object parameter. Can be called from selectioncontroller.gameObject
                    SelectedTile = null;
                }
                break;
            case DeselectStatus.Second:
                lastSelected.GetComponent<SelectionController>().OnObjectDeselect();
                break;
            case DeselectStatus.Both:
                lastSelected.GetComponent<SelectionController>().OnObjectDeselect();
                SelectedTile.GetComponent<SelectionController>().OnObjectDeselect();
                SelectedTile = null;
                break;
            case DeselectStatus.None:
                break;
        }
    }
}