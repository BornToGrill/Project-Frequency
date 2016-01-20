using UnityEngine;
using System;

public class Node : MonoBehaviour {

	internal int X, Y;
	internal Node Left, Right, Up, Down, Parent;
	internal int HCost, GCost;
	internal int FCost {
		get { return GCost + HCost; }
	}

	[Obsolete("To be implemented")]
	internal void clear(){
		HCost = 0;
		GCost = 0;
		Parent = null;
	}

	internal void UpdateNode(Node parent, Node endLocation){
		GCost = parent.GCost + 1;
		HCost = (int)(Math.Abs (endLocation.X - X) + Math.Abs (endLocation.Y - Y));
		Parent = parent;
	}
}