
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SpawnInfo", menuName = "Scriptable Object/SpawnInfo")]
public class SpawnSO : AParsingData<SpawnData>
{
    // /// <summary>
    // /// ���� �������� ���ڰ� �� �������� ������ ���ԵǴ��� �˻��մϴ�.
    // /// </summary>
    // /// <param name="stage">���� �������� ��ȣ</param>
    // /// <returns>���� ����</returns>
    // public bool IsMatch(int stage)
    // {
    //     if (Key.Contains("~"))
    //     {
    //         var split = Key.Split('~');
    //         int min = int.Parse(split[0]);
    //         int max = int.Parse(split[1]);
    //         return stage >= min && stage <= max;
    //     }
    //     else
    //     {
    //         return int.Parse(Key) == stage;
    //     }
    // }
}