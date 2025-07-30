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

        string status = editorMode.isInvincible ? "활성화" : "비활성화";

        Debug.Log($"[개발자 모드] 무적 모드 {status}");
    }
    
    public void OnClickShowDamage()
    {
        var editorMode = EditorMode.Instance;

        editorMode.isShowDamage = !editorMode.isShowDamage;

        string status = editorMode.isShowDamage ? "활성화" : "비활성화";

        Debug.Log($"[개발자 모드] 데미지 표시 {status}");
    }
    
    public void OnClickSpawnerController()
    {
        var editorMode = EditorMode.Instance;

        editorMode.isShowSpawnerController = !editorMode.isShowSpawnerController;
        
        spawnerControllerPanel.GetComponent<SpawnerControllerUI>().Initialize();
        spawnerControllerPanel.SetActive(editorMode.isShowSpawnerController);
        
        string status = editorMode.isShowSpawnerController ? "활성화" : "비활성화";

        Debug.Log($"[개발자 모드] 스포너 컨트롤러 표시 {status}");
    }
}
