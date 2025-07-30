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

        string str = isEditor ? "Ȱ��ȭ" : "��Ȱ��ȭ";
        Debug.Log($"[������ ���] : ������ ��� {str}");
    }
}
