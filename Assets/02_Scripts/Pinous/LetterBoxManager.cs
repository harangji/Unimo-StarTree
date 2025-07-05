using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxManager : MonoBehaviour
{
    // ���͹ڽ� �÷� ����
    public Color letterboxColor = Color.black;

    private Camera mainCamera;

    // �ø��� ���� ����� �ػ� ���� (���÷� 9:21 ���� ����)
    private readonly float flipFoldAspectRatio = 9f / 21f;

    void Start()
    {
        mainCamera = Camera.main;
        ApplyLetterboxForSpecificDevices();
    }

    void ApplyLetterboxForSpecificDevices()
    {
        // ���� ȭ�� ���� ��� (���� ����)
        float screenAspectRatio = (float)Screen.width / Screen.height;
        // �ø��� ���� ��� ����
        if (IsFlipOrFoldDevice(screenAspectRatio))
        {
            Debug.Log("�ø�/���� ��� ������: ���� ���͹ڽ� ����");
            AddLetterbox(screenAspectRatio);
        }
        else
        {
            Debug.Log("��Ÿ ���: ���͹ڽ� ������");
            RemoveLetterbox();
        }
    }

    bool IsFlipOrFoldDevice(float screenAspectRatio)
    {
        Debug.Log(screenAspectRatio);
        // �ø�/���� ����� ������ �´��� Ȯ�� (���� ����)
        if (screenAspectRatio <= flipFoldAspectRatio)
        {
            return true;
        }
        else return false;
    }

    void AddLetterbox(float screenAspectRatio)
    {
        // ���� ���͹ڽ� �߰� (���� �������� ���� ���͹ڽ� ����)
        float scaleHeight = flipFoldAspectRatio / screenAspectRatio;
        Rect rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
        mainCamera.rect = rect;
    }

    void RemoveLetterbox()
    {
        // �⺻ ī�޶� ������ ����
        mainCamera.rect = new Rect(0, 0, 1, 1);
    }

    void OnPreCull()
    {
        // ����� ���͹ڽ� �÷��� ����
        GL.Clear(true, true, letterboxColor);
    }
}
