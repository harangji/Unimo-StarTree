using System;
using UnityEngine;

public class EditorModeInGame : MonoBehaviour
{
    [SerializeField] private GameObject spawnerControllerPanel;
    
    private void Start()
    {
        if (!EditorMode.Instance.isEditor)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnClickInvincible()
    {
        var editorMode = EditorMode.Instance;

        editorMode.isInvincible = !editorMode.isInvincible;

        string status = editorMode.isInvincible ? "Ȱ��ȭ" : "��Ȱ��ȭ";

        Debug.Log($"[������ ���] ���� ��� {status}");
    }
    
    public void OnClickShowDamage()
    {
        var editorMode = EditorMode.Instance;

        editorMode.isShowDamage = !editorMode.isShowDamage;

        string status = editorMode.isShowDamage ? "Ȱ��ȭ" : "��Ȱ��ȭ";

        Debug.Log($"[������ ���] ������ ǥ�� {status}");
    }
    
    public void OnClickSpawnerController()
    {
        var editorMode = EditorMode.Instance;

        editorMode.isShowSpawnerController = !editorMode.isShowSpawnerController;
        
        spawnerControllerPanel.GetComponent<SpawnerControllerUI>().Initialize();
        spawnerControllerPanel.SetActive(editorMode.isShowSpawnerController);
        
        string status = editorMode.isShowSpawnerController ? "Ȱ��ȭ" : "��Ȱ��ȭ";

        Debug.Log($"[������ ���] ������ ��Ʈ�ѷ� ǥ�� {status}");
    }
}
