using System.Collections.Generic;
using UnityEngine;

public static class EngineIDMapping
{
    private static readonly Dictionary<int, int> idxToEngineID = new()
    {
        { 1, 20101 }, // ���
        { 2, 20303 }, // ����
        { 3, 20102 }, // ����
        { 4, 20204 }, // ������
        { 5, 20301 }, // ����
        { 6, 20202 }, // �ʱ��� 
        { 7, 20205 }, // �ҵ�
        { 8, 20302 }, // �ƿ챸������
        { 9, 20203 }, // ��
        { 10, 20402 }, // ������
        { 11, 20403 }, // ��������(����)
        { 12, 20401 }, // ��(��ƿ��)
        { 13, 20201 }, // �佺��
        { 14, 21101 }, // ��ũ��
        { 15, 21103 }, // ���̽�ũ��
        { 16, 21102 }, // �����
        { 17, 21202 }, // ����׸�
        { 18, 21302 }, // ��������
        { 19, 20413 }, // �𷡼�
        { 20, 20412 }, // ������
        { 21, 21301 }, // ������
        { 22, 21201 }, // ��������
        { 23, 20411 }, // ��������
        
        // ��� �߰�
    };

    public static int GetEngineID(int engineIndex)
    {
        if (idxToEngineID.TryGetValue(engineIndex, out int engineID))
            return engineID;

        Debug.LogError($"���� �ε��� {engineIndex}�� �ش��ϴ� ���� ID�� ã�� �� �����ϴ�.");
        return 21101;  // �⺻��
    }
}