namespace TriangleMesh.Models.DataStructures;

public class MyLinkedListNode<T>
{
    public T Value { get; }
    public MyLinkedListNode<T>? Next { get; set; }
    public MyLinkedListNode<T>? Prev { get; set; }
    
    public MyLinkedListNode(T value, MyLinkedListNode<T>? next = null, MyLinkedListNode<T>? prev = null)
    {
        Value = value;
        Next = next;
        Prev = prev;
    }
}