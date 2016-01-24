using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public static class Pathfinding {
	static List<Node> _openList = new List<Node> ();
	static List<TileController> _closedList = new List<TileController> ();

	public static PathFindingResult FindPath(TileController startTile, TileController endTile) {
	    if (startTile == endTile)
	        return new PathFindingResult() {
                Path = new List<TileController>(),
                ValidPath = true,
                FoundEndPoint = true
	        };

	    Node end = GetEndNode(startTile, endTile, (tile, unit) => {
	        if (tile == endTile)
	            return true;
	        return tile.IsTraversable(unit);
	    });
	    if (end.Tile == endTile)
            return new PathFindingResult() {
                Path = ConvertNodesToList(end),
                FoundEndPoint = true,
                ValidPath = true
            };
	    end = GetEndNode(startTile, endTile, (tile, unit) => {
	        if (tile == endTile)
	            return true;
	        return tile.IsTraversableUnitOnly(unit);
	    });
	    if (end.Tile == endTile)
	        return new PathFindingResult() {
	            Path = ConvertNodesToList(end),
                FoundEndPoint = true,
                ValidPath = false
	        };
	    end = GetEndNode(startTile, endTile, (tile, unit) => true);
        return new PathFindingResult() {
            Path = ConvertNodesToList(end),
            FoundEndPoint = false,
            ValidPath = false
        };
	}

    static Node GetEndNode(TileController start, TileController end, Func<TileController, GameObject, bool> traversableCheck) {
        _openList = new List<Node>();
        _closedList = new List<TileController>();
        Node startNode = new Node(start);
        return PathFinder(startNode, new Node(end), startNode, traversableCheck);
    }

    static Node PathFinder(Node currentNode, Node endNode, Node startNode,
        Func<TileController, GameObject, bool> traversableCheck) {
        if (currentNode.Tile == endNode.Tile)
            return currentNode;
        _openList.Remove(currentNode);
        _closedList.Add(currentNode.Tile);

        UpdateSurroundingNodes(currentNode, endNode, startNode, traversableCheck);
        if (_openList.Count < 1)
            return currentNode;

        Node cheapestNode = _openList[0];
        foreach (Node nextNode in _openList)
            if (cheapestNode.FCost > nextNode.FCost)
                cheapestNode = nextNode;
            else if (cheapestNode.FCost == nextNode.FCost && cheapestNode.HCost > nextNode.HCost)
                cheapestNode = nextNode;

        return PathFinder(cheapestNode, endNode, startNode, traversableCheck);
    }

    static void UpdateSurroundingNodes(Node currentNode, Node endNode, Node startNode,
        Func<TileController, GameObject, bool> traversableCheck) {
        GameObject unit = startNode.Tile.Unit.gameObject;
        TileController[] surroundingTiles = { currentNode.Tile.Left, currentNode.Tile.Up, currentNode.Tile.Right, currentNode.Tile.Down };

        foreach (TileController tile in surroundingTiles) {
            if (tile != null && !_closedList.Contains(tile) && traversableCheck(tile, unit)) {
                Node node = new Node(tile);
                node.UpdateValues(currentNode, endNode);
                _openList.Add(node);
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
		//tileList.Insert (0, currentNode.Tile); // Excluding players own position
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

public class PathFindingResult {
    
    public List<TileController> Path { get; set; }
    public bool FoundEndPoint { get; set; }
    public bool ValidPath { get; set; }

}