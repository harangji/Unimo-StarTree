using System.Collections.Generic;
using UnityEngine;

public static class UnitIDMapping
{
    private static readonly Dictionary<int, int> charToUnitID = new()
    {
        { 1, 10101 }, // 라비
        { 2, 10303 }, // 소포
        { 3, 10102 }, // 곰곰
        { 4, 10204 }, // 점분이
        { 5, 10301 }, // 고등어
        { 6, 10202 }, // 너구리
        { 7, 10205 }, // 둠둠
        { 8, 10302 }, // 아우구스투스
        { 9, 10203 }, // 알
        { 10, 10402 }, // 프리모
        { 11, 10403 }, // 도베르만(머핀)
        { 12, 10401 }, // 쿠(마틸다)
        { 13, 10201 }, // 토스터
    };

    public static int GetUnitID(int charIndex)
    {
        if (charToUnitID.TryGetValue(charIndex, out int unitID))
            return unitID;

        Debug.LogError($"캐릭터 인덱스 {charIndex}에 해당하는 유닛 ID를 찾을 수 없습니다.");
        return 10101;  // 예비 기본값 (리비)
    }
}