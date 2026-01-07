using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Locked,
    Available,
    Completed
}

public enum NodeType
{
    Fight,
    Boss,
    Merchant
}

[System.Serializable]
public class Nodes
{
    [SerializeField]protected int id;
    [SerializeField]protected int depth;
    [SerializeField]protected int path;
    
    // public List<Nodes> nextNodes { get; } = new();
    // public List<Nodes> previousNodes { get; } = new();
    
    private List<Nodes> nextNodes = new();
    private List<Nodes> previousNodes = new();
    public List<Nodes> NextNodes => nextNodes;
    public List<Nodes> PreviousNodes => previousNodes;
    public NodeType type { get; protected set;}

    public NodeState state { get; protected set; } = NodeState.Locked;

    public Nodes(int id, int depth, int path)
    {
        this.id = id;
        this.depth = depth;
        this.path = path;
    }

    public void AddNextNode(Nodes node)
    {
        if(nextNodes.Contains(node)) return;
        nextNodes.Add(node);
    }
    
    public void AddPreviousNode(Nodes node)
    {
        if(previousNodes.Contains(node)) return;
        previousNodes.Add(node);
    }

    public void ChangeState(NodeState state)
    {
        this.state = state;
    }

    public virtual void Execute()
    {
        
    }
}
