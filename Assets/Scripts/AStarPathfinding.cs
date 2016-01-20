using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AStarPathfinding {

	private static List<Node> OpenList = new List<Node> ();
	private static List<Node> ClosedList = new List<Node> ();

	public static List<Node> FindPath(Node startNode, Node endNode) {
		if (startNode == null || endNode == null) {
			return null;
		}
		if (startNode == endNode)
			return null;
		
		PathFinder(startNode, endNode);
		return ConvertNodesToList (startNode, endNode);
	}

	static List<Node> ConvertNodesToList(Node startNode, Node endNode) {
		Node currentNode = endNode;
		List<Node> nodeList = new List<Node> ();
		while (currentNode != startNode) {
			nodeList.Add (currentNode);
			currentNode = currentNode.Parent;
		}
		nodeList.Add (startNode);
		return nodeList;
	}

	static void PathFinder(Node currentNode, Node endNode) {
		if (currentNode == endNode)
			return;
		OpenList.Remove (currentNode);
		ClosedList.Add (currentNode);
		UpdateSurroundingNodes (currentNode, endNode);
		if (OpenList.Count < 1)
			return;
		
		Node cheapestNode = OpenList [0];
		foreach (Node nextNode in OpenList)
			if (cheapestNode.FCost >nextNode.FCost)
				cheapestNode = nextNode;
			else if (cheapestNode.FCost == nextNode.FCost && cheapestNode.HCost > nextNode.HCost)
				cheapestNode =nextNode;
		
		PathFinder (cheapestNode, endNode);
	}

	static void UpdateSurroundingNodes(Node node, Node endNode){
		if (node.Left != null && !ClosedList.Contains (node.Left)) {
			OpenList.Add (node.Left);
			node.Left.UpdateNode (node, endNode);
		}
		if (node.Right != null && !ClosedList.Contains (node.Right)) {
			OpenList.Add (node.Right);
			node.Right.UpdateNode (node, endNode);
		}
		if (node.Up != null && !ClosedList.Contains (node.Up)) {
			OpenList.Add (node.Up);
			node.Up.UpdateNode (node, endNode);
		}
		if (node.Down != null && !ClosedList.Contains (node.Down)) {
			OpenList.Add (node.Down);
			node.Down.UpdateNode (node, endNode);
		}
	}
}