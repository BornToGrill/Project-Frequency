using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour {
	public GameObject TilePrefab;
	public int BoardDimensions;
	public GameObject[,] _tiles;

	void Awake ()
	{
		_tiles = new GameObject[BoardDimensions, BoardDimensions];
		for (int x = 0; x < _tiles.GetLength (0); x++)
			for (int y = 0; y < _tiles.GetLength (1); y++)
				_tiles [x, y] = CreateTile (x, y);
		GetSurroundingTiles();
	}


	private bool triggercheck = true;

	void Update() {
		if (triggercheck && Input.GetKeyDown("space")) {
			triggercheck = false;
			List<TileController> x = Pathfinding.FindPath (_tiles [0, 0].GetComponent<TileController>(), _tiles [5, 5].GetComponent<TileController>());
			Debug.Log (x.Count);
			foreach (TileController tile in x) {
				tile.Environment = Environment.None;
			}
		}
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
}