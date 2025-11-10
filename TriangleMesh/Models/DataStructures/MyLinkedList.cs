using System;
using System.Collections;
using System.Collections.Generic;

namespace TriangleMesh.Models.DataStructures;

public class MyLinkedList<T> : IEnumerable<T>
{
    public MyLinkedListNode<T>? Head { get; private set; }
    public MyLinkedListNode<T>? Tail { get; private set; }
    public int Count { get; private set; }

    public MyLinkedList() 
    { }

    public MyLinkedList(IEnumerable<T> collection)
    {
        foreach (var element in collection)
            PushBack(element);
    }
    
    public void PushFront(T value)
    {
        if (Head == null)
            Head = Tail = new MyLinkedListNode<T>(value);
        else
        {
            Head = new MyLinkedListNode<T>(value, Head, null);
            Head.Next!.Prev = Head;
        }

        Count++;
    }

    public void PushBack(T value)
    {
        if (Tail == null)
            Head = Tail = new MyLinkedListNode<T>(value);
        else
        {
            Tail = new MyLinkedListNode<T>(value, null, Tail);
            Tail.Prev!.Next = Tail;
        }

        Count++;
    }
    
    public void PushBack(MyLinkedList<T> collection)
    {
        if (collection.Count <= 0)
            return;
        
        if (Tail == null)
        {
            Head = collection.Head;
            Tail = collection.Tail;
            Count = collection.Count;
        }
        else
        {
            Tail.Next = collection.Head;
            Tail.Next!.Prev = Tail;
            Tail = collection.Tail;
            Count += collection.Count;
        }
        
        collection.Head = null;
        collection.Tail = null;
        collection.Count = 0;
    }

    public T PopFront()
    {
        ThrowIfEmpty();

        var result = Head!.Value;
        if (Head == Tail)
            Head = Tail = null;
        else
        {
            Head = Head.Next;
            Head!.Prev = null;
        }

        Count--;
        return result;
    }

    public T PopBack()
    {
        ThrowIfEmpty();

        var result = Tail!.Value;
        if (Head == Tail)
            Head = Tail = null;
        else
        {
            Tail = Tail.Prev;
            Tail!.Next = null;
        }

        Count--;
        return result;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        var node = Head;
        while (node != null)
        {
            yield return node.Value;
            node = node.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    private void ThrowIfEmpty()
    {
        if (Head == null)
            throw new IndexOutOfRangeException("The collection is empty!");
    }
}