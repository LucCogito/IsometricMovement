using UnityEngine;
using System.Collections;

public class PathfindingNode : IHeapNode<PathfindingNode> {
	
	public readonly Vector3 WorldPosition;
	public readonly int GridX;
	public readonly int GridY;
	public bool Walkable;
	public int GCost;
	public int HCost;
	public PathfindingNode Parent;
	
	public int FCost => GCost + HCost;


	public int HeapIndex 
	{
		get => _heapIndex;
		set => _heapIndex = value;
	}
	private int _heapIndex;

	public PathfindingNode(bool walkable, Vector3 worldPos, int gridX, int gridY) {
		Walkable = walkable;
		WorldPosition = worldPos;
		GridX = gridX;
		GridY = gridY;
	}

	public int CompareTo(PathfindingNode node) 
	{
		int compare = FCost.CompareTo(node.FCost);
		return compare == 0 ? -HCost.CompareTo(node.HCost) : -compare;
	}
}
