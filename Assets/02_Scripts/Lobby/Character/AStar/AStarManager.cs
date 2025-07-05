using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarManager : MonoBehaviour
{
    private List<AStarPathNode> allPathNodes = new List<AStarPathNode>();
    private List<AStarGoalNode> allGoalNodes = new List<AStarGoalNode>();
    // Start is called before the first frame update
    void Awake()
    {
        AStarPathNode.ManagerSTATIC = this;
    }
    public void RegisterRegularNode(AStarPathNode node)
    {
        allPathNodes.Add(node);
    }
    public void RegisterGoalNode(AStarGoalNode node)
    {
        allGoalNodes.Add(node);
    }
    public void RemoveGoalNode(AStarGoalNode node)
    {
        if (allGoalNodes.Contains(node))
        {
            allGoalNodes.Remove(node);
        }
    }
    public int NumofRegularNodes()
    {
        return allPathNodes.Count;
    }
    public List<AStarPathNode> DeepCopiedList()
    {
        List<AStarPathNode> deepcopy = new List<AStarPathNode>();
        foreach(var node in allPathNodes)
        {
            deepcopy.Add(node);
        }
        return deepcopy;
    }
    public List<AStarGoalNode> AllGoalNodes()
    {
        return allGoalNodes;
    }
}

public class NodePriority : IComparer<AStarPathNode>
{
    private Vector3 origin;

    public int Compare(AStarPathNode node1, AStarPathNode node2)
    {
        float dist1 = (origin - node1.transform.position).magnitude;
        float dist2 = (origin - node2.transform.position).magnitude;

        return dist1.CompareTo(dist2);
    }
    public NodePriority(Vector3 origin)
    {
        this.origin = origin;
    }
}

