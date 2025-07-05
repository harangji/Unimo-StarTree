using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarStartNode : AStarPathNode
{
    static private readonly int maxIterSTATIC = 2000;
    static private readonly int connectionNodeNumSTATIC = 2;
    static private readonly float nodeMinDistSTATIC = 3f;
    private List<AStarPathNode> openNodes;
    private List<AStarPathNode> closeNodes;

    private AStarGoalNode goalNode;
    private List<Vector3> pathPositions;
    new void Start()
    {
        blockerMask = LayerMask.GetMask("AStarBlocker");
        //StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { FindGoal(); FindPath(); }, 2f));
    }
    new void OnValidate()
    { }
    public FlowerController_Lobby FindGoal()
    {
        List<AStarGoalNode> goals = ManagerSTATIC.AllGoalNodes();
        goals.Sort(new NodePriority(transform.position));
        for (int i = 0; i < goals.Count; i++)
        {
            if (goals[i].IsTarget())
            {
                goalNode = goals[i];
                return goalNode.GetComponent<FlowerController_Lobby>();
            }
        }
        return null;
    }
    public List<Vector3> FindPath()
    {
        if (goalNode == null) { return new List<Vector3>(); }
        goalNode.InitGoalNode(this);
        if (goalNode.ConnectedNodes.Count == 0) { return new List<Vector3>(); }
        initState();
        if (ConnectedNodes.Count == 0) { return new List<Vector3>(); }
        int iter = 0;
        while (true)
        {
            renewNodeSet();
            AStarPathNode newClose = FindMinimumGNode();
            openNodes.Remove(newClose);
            closeNodes.Add(newClose);
            iter++;
            if (newClose is AStarGoalNode)
            { break; }
            if (iter > maxIterSTATIC) { return new List<Vector3>(); }
        }
        this.parentNode = null;
        AStarPathNode finalNode = goalNode;
        pathPositions = new List<Vector3>();
        while(finalNode != null)
        {
            pathPositions.Add(finalNode.transform.position);
            finalNode = finalNode.parentNode;
        }
        disconnectNodes();
        goalNode.DisconnectGoalNode();
        //StartCoroutine(movePathCoroutine());
        pathPositions.Reverse();
        return pathPositions;
    }
    private void initState()
    {
        openNodes = new List<AStarPathNode>();
        closeNodes = new List<AStarPathNode>();
        GScore = 0;
        HScore = 0;
        FScore = 0;
        findConnectedNodes();
        
        closeNodes.Add(this);
    }
    private void renewNodeSet()
    {
        foreach(AStarPathNode node in closeNodes)
        {
            foreach(AStarPathNode cnode in node.ConnectedNodes)
            {
                if(closeNodes.Contains(cnode)) { continue; }
                if (!openNodes.Contains(cnode))
                {
                    openNodes.Add(cnode);
                    cnode.SetParent(node);
                    cnode.CalculateG(node, node.GScore);
                    cnode.CalculateH(goalNode);
                    cnode.CalculateF();
                }
                else
                {
                    float prevG = cnode.GScore;
                    float newG = cnode.TestNewG(node, node.GScore);
                    if (newG < prevG)
                    {
                        cnode.SetParent(node);
                        cnode.CalculateG(node, node.GScore);
                        cnode.CalculateF();
                    }
                }
            }
        }
    }
    private AStarPathNode FindMinimumGNode()
    {
        AStarPathNode minNode = openNodes[0];
        for (int i = 1; i<openNodes.Count; i++)
        {
            if (openNodes[i]<minNode)
            {
                minNode = openNodes[i];
            }
        }
        return minNode;
    }
    private void findConnectedNodes()
    {
        ConnectedNodes = new List<AStarPathNode>();
        List<AStarPathNode> nodes = ManagerSTATIC.DeepCopiedList();
        nodes.Add(goalNode);
        nodes.Sort(new NodePriority(transform.position));

        for (int i =0; i<nodes.Count; i++)
        {
            if (checkAchivable(nodes[i].transform.position, nodes[i] is AStarGoalNode == true))
            {
                ConnectedNodes.Add(nodes[i]);
                nodes[i].ConnectedNodes.Add(this);
            }
            if (ConnectedNodes.Count >= connectionNodeNumSTATIC) { break; }
        }
    }
    private void disconnectNodes()
    {
        openNodes = new List<AStarPathNode>();
        closeNodes = new List<AStarPathNode>();
        foreach (var node in ConnectedNodes)
        {
            if (node.ConnectedNodes.Contains(this))
            { node.ConnectedNodes.Remove(this); }
        }
        ConnectedNodes = new List<AStarPathNode>();
    }
    private bool checkAchivable(Vector3 position, bool isGoal)
    {
        Vector3 startPos = transform.position;
        if (!isGoal && (position - startPos).magnitude < nodeMinDistSTATIC) { return false; }
        if (Physics.Raycast(startPos, (position - startPos).normalized, (position - startPos).magnitude, blockerMask))
        {
            return false;
        }
        return true;
    }
    //private IEnumerator movePathCoroutine()
    //{
    //    for (int i = pathPositions.Count - 2; i >= 0; i--)
    //    {
    //        transform.position = pathPositions[i];
    //        Debug.Log(pathPositions[i]);
    //        yield return new WaitForSeconds(1f);
    //    }
    //    yield break;
    //}
#if UNITY_EDITOR
    new public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif
}
