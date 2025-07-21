using System;
using System.Linq;
using UnityEngine;

public class BossSkillCoolTimeManager : MonoBehaviour
{
    [SerializeField] private float[] skillCoolTime = new float[4];

    private MonsterController controller;

    private float[] currentTime = new float[4];

    public static bool[] isSkillDo = new bool[4];

    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            currentTime[i] = 0;
            isSkillDo[i] = false;
        }

        controller = GetComponent<MonsterController>();
    }

    void Update()
    {
        //시간 추가 로직
        for (int i = 0; i < 4; i++)
        {
            currentTime[i] += Time.deltaTime;
        }

        for (int i = 3; i >= 0; i--)
        {
            if (currentTime[i] >= skillCoolTime[i] && isSkillDo.All(b => b == false)) //스킬 쿨다임이 다 찼는가? and 진행중인 스킬이 있는가?
            {
                //다 찼다면
                Debug.Log($"[Boss Pattern] {i + 1}번 스킬 발동!");
                //일단 쿨 초기화
                currentTime[i] = 0;

                //해당 스킬 진행중이라고 bool값 변경
                isSkillDo[i] = true;

                //엑션으로 상태 변경
                controller.EnemyAction();
                return;
            }
        }
    }
}