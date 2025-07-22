using System.Collections;
using UnityEngine;

public class BeeTailInvincibilityEffect : MonoBehaviour, IBoomBoomEngineEffect
{
    [SerializeField] private float duration;

    private PlayerStatManager mStatManager;
    private bool bIsBuffActive = false;

    /// <summary>
    /// �ʿ��� ������ �ֽ� PlayerStatManager�� ������
    /// </summary>
    private PlayerStatManager StatManager
    {
        get
        {
            if (mStatManager == null)
            {
                mStatManager = PlaySystemRefStorage.playerStatManager;
            }
            return mStatManager;
        }
    }

    public void ExecuteEffect()
    {
        if (!bIsBuffActive)
        {
            StartCoroutine(ApplyInvincibilityBuff());
        }
    }

    private IEnumerator ApplyInvincibilityBuff()
    {
        bIsBuffActive = true;

        StatManager.SetTemporaryInvincibility(true);

        yield return new WaitForSeconds(duration);

        StatManager.SetTemporaryInvincibility(false);

        bIsBuffActive = false;
    }
}