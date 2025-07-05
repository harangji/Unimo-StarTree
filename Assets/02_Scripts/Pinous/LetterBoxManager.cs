using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxManager : MonoBehaviour
{
    // 레터박스 컬러 설정
    public Color letterboxColor = Color.black;

    private Camera mainCamera;

    // 플립과 폴드 기기의 해상도 비율 (예시로 9:21 비율 가정)
    private readonly float flipFoldAspectRatio = 9f / 21f;

    void Start()
    {
        mainCamera = Camera.main;
        ApplyLetterboxForSpecificDevices();
    }

    void ApplyLetterboxForSpecificDevices()
    {
        // 현재 화면 비율 계산 (세로 기준)
        float screenAspectRatio = (float)Screen.width / Screen.height;
        // 플립과 폴드 기기 감지
        if (IsFlipOrFoldDevice(screenAspectRatio))
        {
            Debug.Log("플립/폴드 기기 감지됨: 상하 레터박스 적용");
            AddLetterbox(screenAspectRatio);
        }
        else
        {
            Debug.Log("기타 기기: 레터박스 미적용");
            RemoveLetterbox();
        }
    }

    bool IsFlipOrFoldDevice(float screenAspectRatio)
    {
        Debug.Log(screenAspectRatio);
        // 플립/폴드 기기의 비율에 맞는지 확인 (세로 기준)
        if (screenAspectRatio <= flipFoldAspectRatio)
        {
            return true;
        }
        else return false;
    }

    void AddLetterbox(float screenAspectRatio)
    {
        // 상하 레터박스 추가 (세로 기준으로 상하 레터박스 적용)
        float scaleHeight = flipFoldAspectRatio / screenAspectRatio;
        Rect rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
        mainCamera.rect = rect;
    }

    void RemoveLetterbox()
    {
        // 기본 카메라 비율로 복원
        mainCamera.rect = new Rect(0, 0, 1, 1);
    }

    void OnPreCull()
    {
        // 배경을 레터박스 컬러로 설정
        GL.Clear(true, true, letterboxColor);
    }
}
