﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DeselectStatus { First, Second, Both, None }

public class Board : MonoBehaviour {
	public GameObject TilePrefab;
	public int BoardDimensions { get; private set; }
	public GameObject[,] _tiles;

	internal GameObject SelectedTile;

	void Awake () {
		BoardDimensions = 18;
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
		if (posY <= 4)
		{
			if (posX <= 6)
				return Environment.Desert;
			if (posX >= 11)
				return Environment.Ice;
		}
		if (posY == 5) {
			if (posX <= 5)
				return Environment.Desert;
			if (posX >= 12)
				return Environment.Ice;
		}
		if (posY == 6) {
			if (posX <= 4)
				return Environment.Desert;
			if (posX >= 13)
				return Environment.Ice;
		}
		if (posY == 11) {
			if (posX <= 4)
				return Environment.Forest;
			if (posX >= 13)
				return Environment.Swamp;
		}
		if (posY == 12) {
			if (posX <= 5)
				return Environment.Forest;
			if (posX >= 12)
				return Environment.Swamp;
		}
		if (posY >= 13)
		{
			if (posX <= 6)
				return Environment.Forest;
			if (posX >= 11)
				return Environment.Swamp;
		}
		if (posX >= 7 && posX <= 10 && posY >=7 && posY <=10)
			return Environment.Island;
		return Environment.Water;
	}

	internal void OnTileClicked(GameObject tile) {

		if (SelectedTile == null) {
			TileController selected = tile.GetComponent<TileController>();
			if (selected.Unit != null) {
				DeselectStatus status = selected.Unit.GetComponent<EventControllerBase>().OnSelected(tile);
				SelectedTile = tile;
				DeselectTile(status, tile);
			}
		}
		else {
			TileController first = SelectedTile.GetComponent<TileController>();
			DeselectStatus status = first.Unit.GetComponent<EventControllerBase>().OnClicked(SelectedTile, tile);
			DeselectTile(status, tile);

		}
	}

	internal void OnTileEnter(GameObject tile) {
		if (SelectedTile != null) { 
		    if (SelectedTile.GetComponent<TileController>().Unit == null) {
		        DeselectTile(DeselectStatus.Both, tile);
		        return;
		    }
			SelectedTile.GetComponent<TileController>().Unit.GetComponent<EventControllerBase>().OnMouseEnter(SelectedTile, tile);
		}
	}

	internal void OnTileLeave(GameObject tile) {
		if (SelectedTile != null) {
		    if (SelectedTile.GetComponent<TileController>().Unit == null) {
		        DeselectTile(DeselectStatus.Both, tile);
		        return;
		    }
			SelectedTile.GetComponent<TileController>().Unit.GetComponent<EventControllerBase>().OnMouseLeave(SelectedTile, tile);
		}
	}

	internal void DeselectTile(DeselectStatus status, GameObject lastSelected) {
		switch (status) {
		case DeselectStatus.First:
			if (SelectedTile != null) {
				SelectedTile.GetComponent<SelectionController>().OnObjectDeselect();
				SelectedTile = null;
			}
			break;
		case DeselectStatus.Second:
		    if (lastSelected != null)
		        lastSelected.GetComponent<SelectionController>().OnObjectDeselect();
		    break;
		case DeselectStatus.Both:
			if(lastSelected != null)
				lastSelected.GetComponent<SelectionController>().OnObjectDeselect();
			if(SelectedTile != null)
				SelectedTile.GetComponent<SelectionController>().OnObjectDeselect();
			SelectedTile = null;
			break;
		case DeselectStatus.None:
			break;
		}
	}
}