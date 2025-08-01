using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon3Factory : MonsterFactory
{
    private float innerRadius = 2.5f;
    private float outerRadius = 8f;
    
    #region ������ �޼���

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);

        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost switch
        {
            2 => 1,
            4 => 5,
            _ => 8
        };
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        if (cost == 2)
        {
            var ctrl = DefaultSpawn(0);
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);
        }
        else if (cost == 4)
        {
            StartPattern2(group);
        }
        else
        {
            StartPattern3(group);
        }

        return null;
    }

    protected override int GetCostFromRate(int rate, int allowedPattern)
    {
        switch (allowedPattern)
        {
            case 1:
                return 2;
            case 2:
                return rate switch
                {
                    < 70 => 2,
                    _ => 4
                };
            case 3:
                return rate switch
                {
                    < 60 => 2,
                    < 85 => 4,
                    _ => 7
                };
        }

        return 0;
    }

    protected override Vector3 FindGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        rand *= rand;
        float radius = innerRadius + rand * (outerRadius - innerRadius);
        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);

        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        pos += PlayerTransform.position;
        if (PlaySystemRefStorage.mapSetter.IsInMap(pos) == false)
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        return pos;
    }

    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        float rand = Random.Range(-20f, 20f);
        Quaternion quat = Quaternion.Euler(0f, 180f + rand, 0f);
        return quat;
    }
    
    #endregion

    #region ���� ���� �޼���

    /// <summary>
    /// ���� 2: �߽��� �������� 5������ ���͸� ������ ���·� �����մϴ�.
    /// </summary>
    /// <param name="group">��ȯ�� ���Ͱ� ���� ���� �׷�</param>
    private void StartPattern2(PatternGroup group)
    {
        float angle;
        Vector3 center = PlayerTransform.position;
        if (center.magnitude > PlaySystemRefStorage.mapSetter.MaxRange - 6f)
        {
            center *= (PlaySystemRefStorage.mapSetter.MaxRange - 6f) / center.magnitude;
        }

        for (int i = 0; i < 5; i++)
        {
            angle = 0.4f * Mathf.PI * i;
            Vector3 pos = center + 4.8f * new Vector3(Mathf.Sin(angle), 0f, Mathf.Cos(angle));
            MonsterController controller = Instantiate(monsterPattern1, pos, Quaternion.identity)
                .GetComponent<MonsterController>();
            controller.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(controller, group);
        }
    }

    /// <summary>
    /// ���� 3: 8���⿡ ���͸� ��ȯ�ϰ�, �Ϻ� ������ ������ �����̽��� ���� ȿ���� �ο��մϴ�.
    /// </summary>
    /// <param name="group">��ȯ�� ���Ͱ� ���� ���� �׷�</param>
    private void StartPattern3(PatternGroup group)
    {
        int firstBombCount = 0;
        Vector3 center = PlayerTransform.position;
        Vector3[] spawnOffsets = new Vector3[]
        {
            new Vector3(-3f, 0, 6f), // �տ�
            new Vector3(6f, 0, 3f), // ����
            new Vector3(3f, 0, -6f), // �ڿ�
            new Vector3(-6f, 0, -3f), // �޵�

            new Vector3(3f, 0, 6f), // �տ�
            new Vector3(6f, 0, -3f), // ����
            new Vector3(-3f, 0, -6f), // �ڿ�
            new Vector3(-6f, 0, 3f) // �޾�
        };

        var actions = new List<Mon003State_Action_C>();

        foreach (var spawnOffset in spawnOffsets)
        {
            var obj = Instantiate(monsterPattern1, center + spawnOffset, Quaternion.identity);
            MonsterController controller = obj.GetComponent<MonsterController>();
            controller.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(controller, group);

            if (firstBombCount < 4)
            {
                firstBombCount++;

                var component = obj.GetComponentInChildren<Mon003State_Action_C>();
                component.canBomb = false;

                actions.Add(component);
            }
        }

        StartCoroutine(DelayBomb(actions));
    }

    /// <summary>
    /// ������ Mon003State_Action_C ����Ʈ�� ���� ���� �ð� �� ���� ������ Ʈ�����մϴ�.
    /// </summary>
    /// <param name="actions">���� �� ���߽�ų �׼� ������Ʈ ����Ʈ</param>
    /// <returns>�ڷ�ƾ IEnumerator</returns>
    private IEnumerator DelayBomb(List<Mon003State_Action_C> actions)
    {
        yield return new WaitForSeconds(2.75f);

        foreach (var action in actions)
        {
            action.canBomb = true;
            action.OnTriggerAction();
        }
    }

    #endregion
}