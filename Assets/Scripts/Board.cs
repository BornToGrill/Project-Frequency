using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	GameObject CreateTile(int posX, int posY) {
		GameObject go = Instantiate (TilePrefab, new Vector3(posX, posY, 0), new Quaternion()) as GameObject;
		go.transform.parent = transform;
		TileController tile = go.GetComponent<TileController> ();
		tile.Environment = GetEnvironment (posX, posY);
		Node node = go.GetComponent<Node> ();
		node.X = posX;
		node.Y = posY;
		return go;
	}

    internal void OnTileSelected(GameObject tile) {

        TileController secondController = tile.GetComponent<TileController>();

        // TODO: Not traversable. Not a valid spawn point etc.
        // TODO: In BaseUnit, TileSelected method (with second selected tile) That checks what the base unit can do.

        if (SelectedTile != null) {
            TileController firstController = SelectedTile.GetComponent<TileController>();
            if (firstController.Unit != null && secondController.GetTraversable(firstController.Unit)) {
                // TODO: Move unit
                SelectedTile.GetComponent<SelectionController>().OnObjectDeselect(SelectedTile);
                return;
            }

            if (firstController.Unit != null && secondController.Unit != null) {
                if (firstController.Unit.Owner != secondController.Unit.Owner) {
                    // TODO: Range check. Maybe in attack func?
                    if (firstController.Unit is LandUnit)
                        secondController.Unit.DamageUnit(((LandUnit) firstController.Unit).Damage);
                    DeselectTile();
                    return;
                }
                DeselectTile();
                return;
            }
            DeselectTile();
        }
        else {
            // Temporary
            SelectedTile = tile;
            tile.GetComponent<SpriteRenderer>().color = Color.black;
            // // // // //
            // secondController.OnFirstTileSelect();
        }
    }

    private void DeselectTile() {
        if (SelectedTile == null)
            return;
        SelectedTile.GetComponent<SelectionController>().OnObjectDeselect(SelectedTile); //TODO: Remove object parameter. Can be called from selectioncontroller.gameObject
        SelectedTile = null;
    }
}