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
        //�ð� �߰� ����
        for (int i = 0; i < 4; i++)
        {
            currentTime[i] += Time.deltaTime;
        }

        for (int i = 3; i >= 0; i--)
        {
            if (currentTime[i] >= skillCoolTime[i] && isSkillDo.All(b => b == false)) //��ų ������� �� á�°�? and �������� ��ų�� �ִ°�?
            {
                //�� á�ٸ�
                Debug.Log($"[Boss Pattern] {i + 1}�� ��ų �ߵ�!");
                //�ϴ� �� �ʱ�ȭ
                currentTime[i] = 0;

                //�ش� ��ų �������̶�� bool�� ����
                isSkillDo[i] = true;

                //�������� ���� ����
                controller.EnemyAction();
                return;
            }
        }
    }
}