using System.Collections;
using System.Collections.Generic;

public class GraphNode<T>
{
    private T _value;
    public T Value
    {
        get
        {
            return _value;
        }
    }

    private List<GraphNode<T>> _neighbours;


    public GraphNode(T value)
    {
        this._value = value;
        _neighbours = new List<GraphNode<T>>();
    }

    public IList<GraphNode<T>> Neighbours
    {
        get
        {
            return _neighbours;
        }
    }

    public bool AddNeighbour(GraphNode<T> neighbour)
    {
        if (_neighbours.Contains(neighbour))
        {
            return false; //don't add already exiting neighbour
        }
        else
        {
            _neighbours.Add(neighbour);
            return true;
        }
    }

    public bool RemoveNeighbour(GraphNode<T> neighbour)
    {
        if (_neighbours.Contains(neighbour))
        {
            _neighbours.Remove(neighbour);
            return true;
        }
        else
        {

            return false; //don't remove non-exiting neighbour
        }
    }

    public void RemoveAllNeighbours()
    {
        for (int i = 0; i < _neighbours.Count; i++)
        {
            _neighbours.RemoveAt(i);
        }
    }

}
