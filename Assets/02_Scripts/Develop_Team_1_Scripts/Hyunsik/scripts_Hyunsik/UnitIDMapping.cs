using System.Collections.Generic;
using UnityEngine;

public static class UnitIDMapping
{
    private static readonly Dictionary<int, int> charToUnitID = new()
    {
        { 1, 10101 }, // ���
        { 2, 10303 }, // ����
        { 3, 10102 }, // ����
        { 4, 10204 }, // ������
        { 5, 10301 }, // ����
        { 6, 10202 }, // �ʱ���
        { 7, 10205 }, // �ҵ�
        { 8, 10302 }, // �ƿ챸������
        { 9, 10203 }, // ��
        { 10, 10402 }, // ������
        { 11, 10403 }, // ��������(����)
        { 12, 10401 }, // ��(��ƿ��)
        { 13, 10201 }, // �佺��
    };

    public static int GetUnitID(int charIndex)
    {
        if (charToUnitID.TryGetValue(charIndex, out int unitID))
            return unitID;

        Debug.LogError($"ĳ���� �ε��� {charIndex}�� �ش��ϴ� ���� ID�� ã�� �� �����ϴ�.");
        return 10101;  // ���� �⺻�� (����)
    }
}