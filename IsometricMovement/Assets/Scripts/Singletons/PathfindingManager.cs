using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingManager : Singleton<PathfindingManager>
{
	[SerializeField] private PathfindingGrid _grid;

	private Queue<PathRequest> _pathRequestQueue = new Queue<PathRequest>();
	private PathRequest _currentPathRequest;
	private bool _isProcessingPath;

    public void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[]> callback)
	{ 
		PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
		_pathRequestQueue.Enqueue(newRequest);
		TryProcessNext();
	}

	private IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;
		PathfindingNode startNode = _grid.NodeFromWorldPoint(startPos);
		PathfindingNode targetNode = _grid.NodeFromWorldPoint(targetPos);
		if (startNode.Walkable && targetNode.Walkable)
		{
			Heap<PathfindingNode> openSet = new Heap<PathfindingNode>(_grid.MaxSize);
			HashSet<PathfindingNode> closedSet = new HashSet<PathfindingNode>();
			openSet.Add(startNode);
			while (openSet.Count > 0)
			{
				PathfindingNode currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);
				if (currentNode == targetNode)
				{
					pathSuccess = true;
					break;
				}
				foreach (PathfindingNode neighbour in _grid.GetNeighbours(currentNode))
				{
					if (!neighbour.Walkable || closedSet.Contains(neighbour))
						continue;
					int newMovementCostToNeighbour = currentNode.GCost + GetGridDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
					{
						neighbour.GCost = newMovementCostToNeighbour;
						neighbour.HCost = GetGridDistance(neighbour, targetNode);
						neighbour.Parent = currentNode;
						if (!openSet.Contains(neighbour))
							openSet.Add(neighbour);
					}
				}
			}
		}
		yield return null;
		if (pathSuccess)
			waypoints = RetracePath(startNode, targetNode);
		_currentPathRequest.Callback(waypoints);
		_isProcessingPath = false;
		TryProcessNext();
	}

	void TryProcessNext()
	{
		if (!_isProcessingPath && _pathRequestQueue.Count > 0)
		{
			_currentPathRequest = _pathRequestQueue.Dequeue();
			_isProcessingPath = true;
			StartCoroutine(FindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd));
		}
	}

	private int GetGridDistance(PathfindingNode nodeA, PathfindingNode nodeB)
	{
		int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
		int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);
		return dstX > dstY ? 14 * dstY + 10 * (dstX - dstY) : 14 * dstX + 10 * (dstY - dstX);
	}

	private Vector3[] RetracePath(PathfindingNode startNode, PathfindingNode endNode)
	{
		List<PathfindingNode> path = new List<PathfindingNode>();
		PathfindingNode currentNode = endNode;
		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		Vector3[] waypoints = SimplifyPath(path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	private Vector3[] SimplifyPath(List<PathfindingNode> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;
		for (int i = 1; i < path.Count; i++)
		{
			Vector2 directionNew = new Vector2(path[i - 1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
			if (directionNew != directionOld)
				waypoints.Add(path[i].WorldPosition);
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	private struct PathRequest
	{
		public Vector3 PathStart;
		public Vector3 PathEnd;
		public Action<Vector3[]> Callback;

		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[]> callback)
		{
			PathStart = start;
			PathEnd = end;
			Callback = callback;
		}
	}
}
