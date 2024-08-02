using System.Collections;
using System.Collections.Generic;

public class Graph<T>
{
    List<GraphNode<T>> graphNodes = new List<GraphNode<T>>();

    public IList<GraphNode<T>> Nodes
    {
        get
        {
            return graphNodes;
        }
    }
    public bool AddNode(T value)
    {
        if(FindNodeInGraph(value) == null)
        {
            graphNodes.Add(new GraphNode<T>(value));
            return true; // add node
        }
        else
        {
            return false;
        }
    }

    public bool AddEdge(T value1, T value2)
    {
        GraphNode<T> graphNode1 = FindNodeInGraph(value1);
        GraphNode<T> graphNode2 = FindNodeInGraph(value2);
        if(graphNode1 == null || graphNode2 == null)
        {
            return false; //mising node
        }
        else if (graphNode1.Neighbours.Contains(graphNode2))
        {
            return false; //edge already exists
        }
        else
        {
            graphNode1.AddNeighbour(graphNode2);
            graphNode2.AddNeighbour(graphNode1);
            return true;
        }
    }

    public bool RemoveNode(T value)
    {
        GraphNode<T> removeNode = FindNodeInGraph(value);
        if (FindNodeInGraph(value) == null)
        {
            return false;
            
        }
        else
        {
            graphNodes.Remove(removeNode);
            foreach (GraphNode<T> graphNode in graphNodes)
            {
                graphNode.RemoveNeighbour(removeNode);
            }
            return true; // remove node
        }
    }

    public bool RemoveEdge(T value1, T value2)
    {
        GraphNode<T> graphNode1 = FindNodeInGraph(value1);
        GraphNode<T> graphNode2 = FindNodeInGraph(value2);
        if (graphNode1 == null || graphNode2 == null)
        {
            return false; //mising node
        }
        else if (!graphNode1.Neighbours.Contains(graphNode2))
        {
            return false;
        }
        else
        {
            graphNode1.RemoveNeighbour(graphNode2);
            graphNode2.RemoveNeighbour(graphNode1);
            return true;
        }
    }

    public void ClearAll()
    {
        foreach (GraphNode<T> graphNode in graphNodes)
        {
            graphNode.RemoveAllNeighbours();
        }

        graphNodes.Clear();
    }

    public GraphNode<T> FindNodeInGraph(T value)
    {
        foreach (GraphNode<T> graphNode in graphNodes)
        {
            if (graphNode.Value.Equals(value))
            {
                return graphNode;
            }
        }
        return null;
    }


}
