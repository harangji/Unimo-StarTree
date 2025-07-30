using UnityEngine;

public class EditorMode : SingletonBehaviour<EditorMode>
{
    public bool isEditor = false;
    public bool isInvincible = false;
    public bool isShowDamage = false;
    public bool isShowSpawnerController = false;
    
    public void OnClickEditorMode()
    {
        isEditor = !isEditor;

        string str = isEditor ? "활성화" : "비활성화";
        Debug.Log($"[개발자 모드] : 개발자 모드 {str}");
    }
}
