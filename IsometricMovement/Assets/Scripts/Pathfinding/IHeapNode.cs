using System;

public interface IHeapNode<T> : IComparable<T> 
{
	public int HeapIndex { get; set; }
}
