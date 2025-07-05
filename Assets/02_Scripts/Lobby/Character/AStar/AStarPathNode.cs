using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathNode : MonoBehaviour
{
    public static AStarManager ManagerSTATIC { get; set; }
    protected int blockerMask;

    public List<AStarPathNode> ConnectedNodes;
#if UNITY_EDITOR
    private List<AStarPathNode> copiedNodes;
#endif
    [HideInInspector] public float FScore;
    [HideInInspector] public float GScore;
    [HideInInspector] public float HScore;
    [HideInInspector] public AStarPathNode parentNode;
    // Start is called before the first frame update
    protected void Start()
    {
        ManagerSTATIC.RegisterRegularNode(this);
        blockerMask = LayerMask.GetMask("AStarBlocker");
        removeInachievableNodes();
    }
    protected void OnValidate()
    {
#if UNITY_EDITOR
        if(copiedNodes != null && copiedNodes.Count > 0)
        {
            foreach (var cnode in copiedNodes)
            {
                if (!ConnectedNodes.Contains(cnode))
                {
                    cnode.ConnectedNodes.Remove(this);
                }
            }
        }
        foreach (var node in ConnectedNodes)
        {
            if (!node.ConnectedNodes.Contains(this))
            {
                node.ConnectedNodes.Add(this);
            }
        }
        deepCopyNodeList();
#endif
    }
#if UNITY_EDITOR
    private void deepCopyNodeList()
    {
        copiedNodes = new List<AStarPathNode>();
        foreach (var node in ConnectedNodes)
        {
            copiedNodes.Add(node);
        }
    }
#endif
    public void AddConnection(AStarPathNode node)
    { ConnectedNodes.Add(node); }
    public void RemoveConnection(AStarPathNode node)
    { ConnectedNodes.Remove(node); }
    public void SetParent(AStarPathNode node)
    { parentNode = node; }
    public void CalculateH(AStarGoalNode goal)
    {
        HScore = (transform.position - goal.transform.position).magnitude;
    }
    public float TestNewG(AStarPathNode node, float prevG)
    {
        prevG += (transform.position - node.transform.position).magnitude;
        return prevG;
    }
    public void CalculateG(AStarPathNode node, float prevG)
    {
        GScore = prevG;
        GScore += (transform.position - node.transform.position).magnitude;
    }
    public void CalculateF()
    {
        FScore = GScore + HScore;
    }

    private void removeInachievableNodes()
    {
        for (int i = ConnectedNodes.Count - 1; i >= 0; i--)
        {
            if (checkAchivable(ConnectedNodes[i].transform.position) == false)
            {   
                ConnectedNodes.RemoveAt(i);
            }
        }
    }
    private bool checkAchivable(Vector3 position)
    {
        Vector3 startPos = transform.position;
        if (Physics.Raycast(startPos, (position - startPos).normalized, (position - startPos).magnitude, blockerMask))
        {
            return false;
        }
        return true;
    }
    public static bool operator >(AStarPathNode node1, AStarPathNode node2)
    {
        return node1.FScore > node2.FScore;
    }
    public static bool operator <(AStarPathNode node1, AStarPathNode node2)
    {
        return node1.FScore < node2.FScore;
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);

        UnityEditor.Handles.BeginGUI();
        GUI.color = Color.yellow;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(transform.position); //+0.1f*(Vector3.up + Vector3.right)
        string[] splitname = gameObject.name.Split(' ');
        string text = splitname[1];
        text.TrimStart('(');
        text.TrimEnd(')');
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - 20, -screenPos.y + view.position.height + 4, size.x, size.y), text); //(size.x / 2)
        UnityEditor.Handles.EndGUI();

        foreach (var node in ConnectedNodes)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
#endif
}
