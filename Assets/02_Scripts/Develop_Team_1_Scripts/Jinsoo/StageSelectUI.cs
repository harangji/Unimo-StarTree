using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ���� �߰��� UI ����
public class StageSelectUI : MonoBehaviour
{
    // ����� ���� Stage ��ȣ �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI mStageNumberText;
    // [SerializeField] private Text mLockText;
    
    private int mCurrentStage = 1;
    private int mMaxClearedStage = 0;

    private const int MaxStage = 1000;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mMaxClearedStage++;
            Debug.Log($"������ �������� ���� ::: {mMaxClearedStage}");
        }
    }

    // ���� ȭ��ǥ ������ ��, �������� ����
    public void OnClick_LeftArrow()
    {
        mCurrentStage = Mathf.Max(1, mCurrentStage - 1);
        UpdateUI();
    }

    // ������ ȭ��ǥ ������ ��, �������� ����
    public void OnClick_RightArrow()
    {
        if (mCurrentStage < Mathf.Min(MaxStage, mMaxClearedStage + 1))
        {
            mCurrentStage++;
            UpdateUI();
        }
        else
        {
            // ��� �������� �ȳ� �޽���
            // if (mLockText != null)
            //     mLockText.text = "���� ���������� ���� Ŭ�����ؾ� �մϴ�";
            Debug.Log("��� �־��");
        }
    }

    private void UpdateUI()
    {
        mStageNumberText.text = $"Stage {mCurrentStage}";
        
        // if (mLockText != null)
        //     mLockText.text = ""; // �޽��� �ʱ�ȭ
    }

    // ���۹�ư ������ ��, �������� ����
    public void OnClick_StartGame()
    {
        mMaxClearedStage = StageLoader.GetLastClearedStage();
        // ��� ���������� ���� �Ұ�
        if (mCurrentStage > mMaxClearedStage + 1)
        {
            // if (mLockText != null)
            //     mLockText.text = "�� ���������� ��� �ֽ��ϴ�";
            Debug.Log("��� �־��");
            return;
        }
        
        StageLoader.LoadStage(mCurrentStage);
    }
}