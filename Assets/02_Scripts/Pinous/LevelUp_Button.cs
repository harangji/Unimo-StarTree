using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    bool isPush = false;
    float timer = 0.0f;
    Coroutine coroutine;
    public void EXP_UP_Button()
    {
        InitLevelUpBUtton();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), .25f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        EXP_UP_Button();
        coroutine = StartCoroutine(Push_Coroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    public void OnDisable()
    {
        isPush = false;
        if (coroutine != null) StopCoroutine(coroutine);
        timer = 0.0f;
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            isPush = false;
            if (coroutine != null) StopCoroutine(coroutine);
            timer = 0.0f;
        }

        if (isPush)
        {
            Base_Manager.instance.saveTimer = 0.0f;
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                EXP_UP_Button();
            }
        }
    }

    IEnumerator Push_Coroutine()
    {
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }

    private void InitLevelUpBUtton()
    {
        int nextLevel = Base_Manager.Data.UserData.Level + 2;
        
        double expNow = Base_Manager.Data.UserData.EXP;
        double expAdd = Base_Manager.Data.EXP_GET;
        double expMax = Base_Manager.Data.EXP_SET;

        bool isLevelUp = (expNow + expAdd >= expMax);
        bool isGradeUp = (nextLevel == 100 || nextLevel == 300 || nextLevel == 700 || nextLevel == 1000);

        double requiredCost = 0;
        
        // 레벨업 + 진화 조건일 경우
        if (isLevelUp && isGradeUp)
        {
            requiredCost = RewardCalculator.GetGradeUpCost();
            if (Base_Manager.Data.UserData.Yellow < requiredCost && Base_Manager.Data.UserData.Red < requiredCost)
            {
                Canvas_Holder.instance.Get_Toast("NM");
                return;
            }
            
            Base_Manager.Data.UserData.Yellow -= requiredCost;
            Base_Manager.Data.UserData.Red -= requiredCost;
            Base_Manager.Data.LevelUP();
            Sound_Manager.instance.Play(Sound.Effect, "effect_00");
        }
        else
        {
            requiredCost = RewardCalculator.GetLevelUpCost();
            if (Base_Manager.Data.UserData.Yellow < requiredCost)
            {
                Canvas_Holder.instance.Get_Toast("NM");
                return;
            }
            
            Base_Manager.Data.UserData.Yellow -= requiredCost;
            Base_Manager.Data.LevelUP();
            Sound_Manager.instance.Play(Sound.Effect, "effect_00");
        }
        
        Debug.Log($"재화 소모 비용 ::: {requiredCost}, 다음 레벨 ::: {nextLevel}");
    }
}
