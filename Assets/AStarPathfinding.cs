using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarPathfinding {
	private List<Node> OpenList, ClosedList;
	private Node[,] _nodes;
	private Node EndLocation;

	void Start () {
		OpenList = new List<Node> ();
		ClosedList = new List<Node> ();
	}

	public void FindPath(Node start, Node end) {
		EndLocation = _nodes[(int)end.X, (int)end.Y];
		PathFinder(_nodes[(int)start.X, (int)start.Y]);
		PrintPath (EndLocation);
	}

	void PrintPath(Node node) {
		Debug.Log (node.X.ToString() + " : " + node.Y.ToString());
		if (node.Parent == null)
			return;
		PrintPath (node.Parent);
	}

	void PathFinder(Node node) {
		if (EndLocation.Parent != null)
			return;

		OpenList.Remove (node);
		ClosedList.Add (node);
		UpdateSurroundingNodes (node);
		if (OpenList.Count < 1)
			return;
		
		Node bestF = OpenList [0];
		foreach (Node possibleNextNode in OpenList)
			if (bestF.FCost > possibleNextNode.FCost)
				bestF = possibleNextNode;
			else if (bestF.FCost == possibleNextNode.FCost && bestF.HCost > possibleNextNode.HCost)
				bestF = possibleNextNode;
		PathFinder (bestF);
	}

	void UpdateSurroundingNodes(Node node){
		if (node.Left != null && !ClosedList.Contains (node.Left)) {
			OpenList.Add (node.Left);
			node.Left.UpdateNode (node, EndLocation);
		}
		if (node.Right != null && !ClosedList.Contains (node.Right)) {
			OpenList.Add (node.Right);
			node.Right.UpdateNode (node, EndLocation);
		}
		if (node.Up != null && !ClosedList.Contains (node.Up)) {
			OpenList.Add (node.Up);
			node.Up.UpdateNode (node, EndLocation);
		}
		if (node.Down != null && !ClosedList.Contains (node.Down)) {
			OpenList.Add (node.Down);
			node.Down.UpdateNode (node, EndLocation);
		}
	}

	public void ConvertMultiArray(GameObject[,] array){
		_nodes = new Node[array.GetLength (0), array.GetLength (1)];
		for (int x = 0; x < array.GetLength (0); x++)
			for (int y = 0; y < array.GetLength (1); y++) {
				_nodes [x, y] = array [x, y].GetComponent<Node> ();
			}
	}
}