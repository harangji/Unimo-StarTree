using System;
using UnityEngine;

public class EditorModeInGame : MonoBehaviour
{
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

}
