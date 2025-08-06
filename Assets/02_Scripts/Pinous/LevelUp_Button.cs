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
    public bool canPush { get; set; }= true;
    
    public void EXP_UP_Button()
    {
        InitLevelUpBUtton();
        transform.DORewind();
        transform.DOPunchScale(new Vector3(-0.2f, -0.2f, -0.2f), .25f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isPush) return;
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
        if (Base_Manager.Data.PendingGradeUp)
        {
            double gradeCost = RewardCalculator.GetGradeUpCost();
            if (Base_Manager.Data.UserData.Yellow < gradeCost || Base_Manager.Data.UserData.Red < gradeCost)
            {
                Canvas_Holder.instance.Get_Toast("NM");
                return;
            }

            Base_Manager.Data.UserData.Yellow -= gradeCost;
            Base_Manager.Data.UserData.Red -= gradeCost;
            Base_Manager.Data.GradeUp(); // 진화 실행

            Sound_Manager.instance.Play(Sound.Effect, "effect_00");
            return;
        }
        
        int nextLevel = Base_Manager.Data.UserData.Level + 2;
        double totalCost;
        double costPerClick;

        bool isGradeUp = (nextLevel == 100 || nextLevel == 300 || nextLevel == 700 || nextLevel == 1000);

        if (isGradeUp)
        {
            totalCost = RewardCalculator.GetGradeUpCost();
            costPerClick = totalCost / 5.0;
            if (Base_Manager.Data.UserData.Yellow < costPerClick && Base_Manager.Data.UserData.Red < costPerClick)
            {
                Canvas_Holder.instance.Get_Toast("NM");
                return;
            }

            Base_Manager.Data.UserData.Yellow -= costPerClick;
            Base_Manager.Data.UserData.Red -= costPerClick;
            Base_Manager.Data.LevelUP();
            Sound_Manager.instance.Play(Sound.Effect, "effect_00");
        }
        else
        {
            totalCost = RewardCalculator.GetLevelUpCost();
            costPerClick = totalCost / 5.0;
            if (Base_Manager.Data.UserData.Yellow < costPerClick)
            {
                Canvas_Holder.instance.Get_Toast("NM");
                return;
            }

            Base_Manager.Data.UserData.Yellow -= costPerClick;
            Base_Manager.Data.LevelUP();
            Sound_Manager.instance.Play(Sound.Effect, "effect_00");
        }

        Debug.Log($"재화 소모 비용 ::: {costPerClick}, 총비용 ::: {totalCost}, 다음 레벨 ::: {nextLevel}");
    }
}
