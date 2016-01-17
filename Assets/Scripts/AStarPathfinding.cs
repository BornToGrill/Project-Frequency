using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AStarPathfinding {
	static private List<Node> OpenList = new List<Node> ();
	static private List<Node> ClosedList = new List<Node> ();
	static private Node _endLocation;
	static private Node _startLocation;
	static public Node EndLocation {
		get { return _endLocation;}
		set { _endLocation = value;
			FindPath(StartLocation, _endLocation);
		}
	}
	static public Node StartLocation {
		get { return _startLocation; }
		set { _startLocation = value;
			FindPath(_startLocation, EndLocation);
		}
	}

	static public void FindPath(Node start, Node end) {
		if (start == null || end == null) {
			Debug.Log ("one is null");
			return;
		}
		if (start == end)
			return;
		PathFinder(start);
		PrintPath (EndLocation);
	}

	static void PrintPath(Node node) {
		Debug.Log (node.X.ToString() + " : " + node.Y.ToString());
		if (node.Parent == null)
			return;
		PrintPath (node.Parent);
	}

	static void PathFinder(Node node) {
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

	static void UpdateSurroundingNodes(Node node){
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
}