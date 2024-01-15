using UnityEngine;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour {

	public bool DisplayGridGizmos;

	[SerializeField] private LayerMask _unwalkableMask;
	[SerializeField] private Vector2 _gridWorldSize;
	[SerializeField] private float _nodeRadius;

	private PathfindingNode[,] _grid;
	private float _nodeDiameter;
	private int _gridSizeX, _gridSizeY;

	public int MaxSize => _gridSizeX * _gridSizeY;

	void Awake() 
	{
		_nodeDiameter = _nodeRadius*2;
		_gridSizeX = Mathf.RoundToInt(_gridWorldSize.x/_nodeDiameter);
		_gridSizeY = Mathf.RoundToInt(_gridWorldSize.y/_nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() 
	{
		_grid = new PathfindingNode[_gridSizeX,_gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * _gridWorldSize.x/2 - Vector3.forward * _gridWorldSize.y/2;
		for (int x = 0; x < _gridSizeX; x ++) {
			for (int y = 0; y < _gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,_nodeRadius,_unwalkableMask));
				_grid[x,y] = new PathfindingNode(walkable,worldPoint, x,y);
			}
		}
	}

	public List<PathfindingNode> GetNeighbours(PathfindingNode node) 
	{
		List<PathfindingNode> neighbours = new List<PathfindingNode>();
		for (int x = -1; x <= 1; x++) 
		{
			for (int y = -1; y <= 1; y++) 
			{
				if (x == 0 && y == 0)
					continue;
				int checkX = node.GridX + x;
				int checkY = node.GridY + y;
				if (checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
					neighbours.Add(_grid[checkX,checkY]);
			}
		}
		return neighbours;
	}

	public PathfindingNode NodeFromWorldPoint(Vector3 worldPosition) 
	{
		float percentX = (worldPosition.x + _gridWorldSize.x/2) / _gridWorldSize.x;
		float percentY = (worldPosition.z + _gridWorldSize.y/2) / _gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);
		int x = Mathf.RoundToInt((_gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((_gridSizeY-1) * percentY);
		return _grid[x,y];
	}
	
	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(_gridWorldSize.x,1,_gridWorldSize.y));
		if (_grid != null && DisplayGridGizmos) {
			foreach (PathfindingNode n in _grid) {
				Gizmos.color = (n.Walkable)?Color.white:Color.red;
				Gizmos.DrawCube(n.WorldPosition, Vector3.one * (_nodeDiameter-.1f));
			}
		}
	}
}