
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "SpawnInfo", menuName = "Scriptable Object/SpawnInfo")]
public class SpawnSO : AParsingData<SpawnData>
{
    // /// <summary>
    // /// 현재 스테이지 숫자가 이 데이터의 범위에 포함되는지 검사합니다.
    // /// </summary>
    // /// <param name="stage">현재 스테이지 번호</param>
    // /// <returns>포함 여부</returns>
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