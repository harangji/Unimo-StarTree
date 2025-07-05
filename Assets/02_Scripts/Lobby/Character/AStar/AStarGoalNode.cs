using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGoalNode : AStarPathNode
{
    static private readonly int connectionNodeNumSTATIC = 2;
    static private readonly float nodeMinDistSTATIC = 3f;

    private bool hasTargeted = false;
    new void Start()
    {
        blockerMask = LayerMask.GetMask("AStarBlocker");
        ManagerSTATIC.RegisterGoalNode(this);
    }
    new void OnValidate()
    { }
    private void OnDisable()
    {
        ManagerSTATIC.RemoveGoalNode(this);
    }
    public bool IsTarget()
    {
        return !hasTargeted;
    }
    public void DisconnectGoalNode()
    {
        foreach(var node in ConnectedNodes)
        {
            if (node.ConnectedNodes.Contains(this))
            { node.ConnectedNodes.Remove(this);}
        }
        ConnectedNodes = new List<AStarPathNode>();
    }
    public void InitGoalNode(AStarStartNode start)
    {
        if (start == null) { return; }
        hasTargeted = true;
        findConnectedNodes(start);
    }
    public void CancelGoal()
    {
        hasTargeted = false;
        DisconnectGoalNode();
    }
    private void findConnectedNodes(AStarStartNode start)
    {
        ConnectedNodes = new List<AStarPathNode>();
        List<AStarPathNode> nodes = ManagerSTATIC.DeepCopiedList();
        nodes.Add(start);
        nodes.Sort(new NodePriority(transform.position));

        for (int i = 0; i < nodes.Count; i++)
        {
            if (checkAchivable(nodes[i].transform.position, nodes[i] is AStarStartNode == true))
            {
                ConnectedNodes.Add(nodes[i]);
                nodes[i].ConnectedNodes.Add(this);
            }
            if (ConnectedNodes.Count >= connectionNodeNumSTATIC) { break; }
        }
    }
    private bool checkAchivable(Vector3 position, bool isStart)
    {
        Vector3 startPos = transform.position;
        if (!isStart && (position - startPos).magnitude < nodeMinDistSTATIC) { return false; }
        if (Physics.Raycast(startPos, (position - startPos).normalized, (position - startPos).magnitude, blockerMask))
        {
            return false;
        }
        return true;
    }

#if UNITY_EDITOR
    new public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif
}
