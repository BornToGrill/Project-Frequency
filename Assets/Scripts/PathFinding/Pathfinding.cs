using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class Pathfinding {
	static List<Node> _openList = new List<Node> ();
	static List<TileController> _closedList = new List<TileController> ();

	public static List<TileController> FindPath(TileController startTile, TileController endTile) 
	{
		if (startTile == endTile)
			return new List<TileController> ();

		Node startNode = new Node (startTile);
		Node endNode = new Node (endTile);
		endNode = PathFinder(startNode, endNode, startNode, false);
		if (endNode.Tile != endTile) {
			_openList.Clear ();
			_closedList.Clear ();
			endNode = new Node (endTile);
			endNode = PathFinder(startNode, endNode, startNode, true);
		}

		_openList.Clear ();
		_closedList.Clear ();
		return ConvertNodesToList (endNode);
	}

	static Node PathFinder(Node currentNode, Node endNode, Node startNode, bool ignoreEnvironment)
	{
		if (currentNode.Tile == endNode.Tile)
			return currentNode;
		_openList.Remove (currentNode);
		_closedList.Add (currentNode.Tile);

		UpdateSurroundingNodes (currentNode, endNode, startNode, ignoreEnvironment);
		if (_openList.Count < 1)
			return currentNode;

		Node cheapestNode = _openList [0];
		foreach (Node nextNode in _openList)
			if (cheapestNode.FCost > nextNode.FCost)
				cheapestNode = nextNode;
			else if (cheapestNode.FCost == nextNode.FCost && cheapestNode.HCost > nextNode.HCost)
				cheapestNode = nextNode;

		return PathFinder (cheapestNode, endNode, startNode, ignoreEnvironment);
	}

	static void UpdateSurroundingNodes(Node currentNode, Node endNode, Node startNode, bool ignoreEnvironment)
	{
		GameObject unit = startNode.Tile.gameObject;
		TileController[] surroundingTiles = new [] {
			currentNode.Tile.Left,
			currentNode.Tile.Up,
			currentNode.Tile.Right,
			currentNode.Tile.Down
		};

		foreach (TileController tile in surroundingTiles) {
			if (tile != null && !_closedList.Contains(tile)) {
				if (ignoreEnvironment) {
					if (tile.IsTraversableUnitOnly (unit)) {
						Node node = new Node (tile);
						node.UpdateValues (currentNode, endNode);
						_openList.Add (node);
					}
				} else if (tile.IsTraversable(unit)) {
					Node node = new Node (tile);
					node.UpdateValues (currentNode, endNode);
					_openList.Add (node);
				}
			}
		}
	}

	static List<TileController> ConvertNodesToList( Node endNode)
	{
		Node currentNode = endNode;
		List<TileController> tileList = new List<TileController> ();
		while (currentNode.Parent != null)
		{
			tileList.Insert (0, currentNode.Tile);
			currentNode = currentNode.Parent;
		}
		tileList.Insert (0, currentNode.Tile);
		return tileList;
	}

	private class Node
	{
		public Node Parent;
		public TileController Tile;
		public int HCost, GCost, xdiff, ydiff;
		public int FCost
		{
			get { return GCost + HCost; }
		}

		public Node(TileController tile)
		{
			Tile = tile;
		}

		public void UpdateValues(Node parent, Node endNode)
		{
			GCost = parent.GCost + 1;
			HCost = (int)(Mathf.Abs (endNode.Tile.Position.x - Tile.Position.x) + Mathf.Abs (endNode.Tile.Position.y - Tile.Position.y));
			xdiff = (int)Mathf.Abs (endNode.Tile.Position.x - Tile.Position.x);
			ydiff = (int)Mathf.Abs (endNode.Tile.Position.y - Tile.Position.y);
			Parent = parent;
		}
	}
}