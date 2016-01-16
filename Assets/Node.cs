using UnityEngine;
using System.Collections;
using System;

public class Node : MonoBehaviour {
	private int _fcost;
	public int X, Y;
	public Node Left, Right, Up, Down, Parent;
	public int HCost, GCost;
	public int FCost {
		private set { _fcost = value; }
		get { return GCost + HCost; }
	}

	public void clear(){
		HCost = 0;
		GCost = 0;
		Parent = null;
	}

	public void UpdateNode(Node parent, Node endLocation){
		GCost = parent.GCost + 1;
		HCost = (int)(Math.Abs (endLocation.X - X) + Math.Abs (endLocation.Y - Y));
		Parent = parent;
	}
}
