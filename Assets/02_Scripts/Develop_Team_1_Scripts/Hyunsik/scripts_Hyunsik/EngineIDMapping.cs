using System.Collections.Generic;
using UnityEngine;

public static class EngineIDMapping
{
    private static readonly Dictionary<int, int> idxToEngineID = new()
    {
        { 1, 20101 }, // 라비
        { 2, 20303 }, // 소포
        { 3, 20102 }, // 곰곰
        { 4, 20204 }, // 점분이
        { 5, 20301 }, // 고등어
        { 6, 20202 }, // 너구리 
        { 7, 20205 }, // 둠둠
        { 8, 20302 }, // 아우구스투스
        { 9, 20203 }, // 알
        { 10, 20402 }, // 프리모
        { 11, 20403 }, // 도베르만(머핀)
        { 12, 20401 }, // 쿠(마틸다)
        { 13, 20201 }, // 토스터
        { 14, 21101 }, // 오크통
        { 15, 21103 }, // 아이스크림
        { 16, 21102 }, // 마녀솥
        { 17, 21202 }, // 개밥그릇
        { 18, 21302 }, // 쓰레기통
        { 19, 20413 }, // 모래성
        { 20, 20412 }, // 욕조통
        { 21, 21301 }, // 엘프컵
        { 22, 21201 }, // 마술모자
        { 23, 20411 }, // 구름구름
        
        // 계속 추가
    };

    public static int GetEngineID(int engineIndex)
    {
        if (idxToEngineID.TryGetValue(engineIndex, out int engineID))
            return engineID;

        Debug.LogError($"엔진 인덱스 {engineIndex}에 해당하는 엔진 ID를 찾을 수 없습니다.");
        return 21101;  // 기본값
    }
}