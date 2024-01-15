using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapNode<T> {

	private T[] _nodes;
	private int _nodesCount;

	public int Count => _nodesCount;

	public Heap(int maxHeapSize) => _nodes = new T[maxHeapSize];
	
	public void Add(T node) 
	{
		node.HeapIndex = _nodesCount;
		_nodes[_nodesCount] = node;
		SortUp(node);
		_nodesCount++;
	}

	public T RemoveFirst() 
	{
		T firstNode = _nodes[0];
		_nodesCount--;
		_nodes[0] = _nodes[_nodesCount];
		_nodes[0].HeapIndex = 0;
		SortDown(_nodes[0]);
		return firstNode;
	}

	public void UpdateNode(T node) => SortUp(node);

	public bool Contains(T node) => Equals(_nodes[node.HeapIndex], node);

	void SortDown(T node) 
	{
		while (true) 
		{
			int childIndexLeft = node.HeapIndex * 2 + 1;
			int childIndexRight = node.HeapIndex * 2 + 2;
			if (childIndexLeft < _nodesCount) 
			{
				int swapIndex = childIndexLeft;
				if (childIndexRight < _nodesCount) 
					if (_nodes[childIndexLeft].CompareTo(_nodes[childIndexRight]) < 0) 
						swapIndex = childIndexRight;
				if (node.CompareTo(_nodes[swapIndex]) < 0) 
					Swap (node,_nodes[swapIndex]);
				else 
					return;
			}
			else 
				return;
		}
	}
	
	void SortUp(T node) 
	{
		int parentIndex = (node.HeapIndex-1)/2;
		while (true) 
		{
			T parentNode = _nodes[parentIndex];
			if (node.CompareTo(parentNode) > 0) 
				Swap (node,parentNode);
			else 
				break;
			parentIndex = (node.HeapIndex-1)/2;
		}
	}
	
	void Swap(T nodeA, T nodeB) 
	{
		_nodes[nodeA.HeapIndex] = nodeB;
		_nodes[nodeB.HeapIndex] = nodeA;
		int nodeAIndex = nodeA.HeapIndex;
		nodeA.HeapIndex = nodeB.HeapIndex;
		nodeB.HeapIndex = nodeAIndex;
	}
}
