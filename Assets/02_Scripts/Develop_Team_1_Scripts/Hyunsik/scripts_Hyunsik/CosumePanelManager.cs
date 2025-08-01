using System.Collections.Generic;
using UnityEngine;

public class CosumePanelManager : MonoBehaviour
{
    public static CosumePanelManager Instance { get; private set; }

    private readonly List<Cosume_Base_Panel> mPanels = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(Cosume_Base_Panel panel)
    {
        if (!mPanels.Contains(panel))
            mPanels.Add(panel);
    }

    public void UnlockAll()
    {
        foreach (var panel in mPanels)
        {
            panel.ForceUnlock();
        }
    }
}