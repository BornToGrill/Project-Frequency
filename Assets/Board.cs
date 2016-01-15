﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Environment {Swamp, Ice, Desert, Forest, Water, Island};

public class Board : MonoBehaviour {
	public GameObject TilePrefab;
	public int BoardDimensions;
	private GameObject[,] _tiles;

	// Use this for initialization
	void Start () {
		_tiles = new GameObject[BoardDimensions, BoardDimensions];
		for (int y = 0; y < _tiles.GetLength (0); y++)
			for (int x = 0; x < _tiles.GetLength (1); x++)
				_tiles [y, x] = CreateTile (y, x);
		GetSurroundingTiles();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GetSurroundingTiles() {
		for (int y = 0; y < _tiles.GetLength (0); y++)
			for (int x = 0; x < _tiles.GetLength (1); x++) {
				Tile tile = _tiles [y, x].GetComponent<Tile> ();
				if (x > 0)
					tile.Left = _tiles [y, x - 1].GetComponent<Tile> ();
				if (x < BoardDimensions-1)
					tile.Right = _tiles [y, x + 1].GetComponent<Tile> ();
				if (y > 0)
					tile.Up = _tiles [y - 1, x].GetComponent<Tile> ();
			}
	}

	Environment GetEnvironment(int posX, int posY) {
		if (posY < Math.Floor(BoardDimensions * 0.4f)) {
			if (posX < Math.Floor(BoardDimensions * 0.4f))
				return Environment.Desert;
			if (posX > BoardDimensions * 0.6f)
				return Environment.Forest;
			return Environment.Water;
		}
		if (posY > BoardDimensions * 0.6f) {
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
		Tile tile = go.GetComponent<Tile> ();
		tile.Environment = GetEnvironment (posX, posY);
		return go;
	}
}