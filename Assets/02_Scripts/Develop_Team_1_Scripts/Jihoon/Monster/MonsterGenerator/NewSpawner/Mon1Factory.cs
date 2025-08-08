using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mon1Factory : MonsterFactory
{
    private float genRadius = 20f;
    private float randDirAngle = 40f;

    #region ������ �޼���

    public override MonsterController SpawnMonster(int allowedPattern)
    {
        int cost = GetCostFromRate(Random.Range(0, 100), allowedPattern);
        
        if (!TryConsumeDifficulty(cost)) return null;

        int spawnCount = cost < 5 ? 1 : 8;
        var group = new PatternGroup { Remaining = spawnCount, Cost = cost };
        ActiveGroups.Add(group);

        var prefab = GetPrefabFromRate(cost);
        float offset = cost <= 6 ? 6.5f : 7.5f;

        return spawnCount == 1
            ? SpawnSingle(prefab, group, out var single) ? single : null
            : SpawnGroup(prefab, offset, group);
    }
    
    protected override int GetCostFromRate(int rate, int allowedPattern)
    {
        switch (allowedPattern)
        {
            case 1:
                return 3;
            case 2:
                return rate switch
                {
                    < 70 => 3,
                    _ => 5
                };
            case 3:
                return rate switch
                {
                    < 60 => 3,
                    < 85 => 5,
                    _ => 6
                };
        }
        
        return 0;
    }
    
    protected override Vector3 FindGenPosition()
    {
        Random.InitState(DateTime.Now.Millisecond);
        float rand = Random.Range(0f, 1f);
        float radius = (rand > 0.995f)
            ? genRadius
            : 0.4f * genRadius * Mathf.Sqrt(-Mathf.Log(1 - rand)) + 1.5f;

        float angle = 2f * Mathf.PI * Random.Range(0f, 1f);
        Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle)) + PlayerTransform.position;

        if (!PlaySystemRefStorage.mapSetter.IsInMap(pos))
        {
            pos = PlaySystemRefStorage.mapSetter.FindNearestPoint(pos);
        }

        return pos;
    }

    protected override Quaternion SetGenRotation(Vector3 genPos)
    {
        Quaternion lookRot = Quaternion.LookRotation(PlayerTransform.position - genPos, Vector3.up);
        float randAngle = (Random.Range(0, 2) * 2 - 1) * Mathf.Pow(Random.Range(0f, 1f), 2f) * randDirAngle;

        if (randAngle < 0f) randAngle += 360f;

        return lookRot * Quaternion.Euler(0f, randAngle, 0f);
    }

    #endregion

    #region ���� ���� �޼���
    
    /// <summary>
    /// rate ���� ������� ����� �������� �����մϴ�.
    /// </summary>
    /// <param name="cost">�������� ������ cost ��</param>
    /// <returns>���õ� ���� ������</returns>
    private GameObject GetPrefabFromRate(int cost)
    {
        return cost <= 5 ? monsterPattern1 : monsterPattern3;
    }

    /// <summary>
    /// ���� ���͸� ��ȯ�ϰ� �׷쿡 ����մϴ�.
    /// </summary>
    /// <param name="prefab">��ȯ�� ������</param>
    /// <param name="group">��ȯ�� ���� �׷� ����</param>
    /// <param name="ctrl">��ȯ�� ���� ��Ʈ�ѷ� (out)</param>
    /// <returns>��ȯ ���� ����</returns>
    private bool SpawnSingle(GameObject prefab, PatternGroup group, out MonsterController ctrl)
    {
        ctrl = DefaultSpawn(0);
        if (ctrl == null) return false;

        ctrl.InitEnemy(PlayerTransform);
        RegisterDestroyCallback(ctrl, group);
        return true;
    }

    /// <summary>
    /// 8������ ���͸� �߽� ��ġ�� �������� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="prefab">��ȯ�� ���� ������</param>
    /// <param name="offset">�߽����κ����� �Ÿ�</param>
    /// <param name="group">��ȯ�� ���� �׷� ����</param>
    /// <returns>ù ��°�� ������ ���� ��Ʈ�ѷ�</returns>
    private MonsterController SpawnGroup(GameObject prefab, float offset, PatternGroup group)
    {
        Vector3 center = ClampToArena(PlayerTransform.position, PlaySystemRefStorage.mapSetter.MaxRange - 7.2f);
        MonsterController primary = null;

        for (int i = 0; i < 8; i++)
        {
            float angle = Mathf.PI / 4f * i;
            Vector3 pos = center + offset * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            Quaternion rot = Quaternion.LookRotation(center - pos, Vector3.up);

            var ctrl = Instantiate(prefab, pos, rot).GetComponent<MonsterController>();
            ctrl.InitEnemy(PlayerTransform);
            RegisterDestroyCallback(ctrl, group);

            if (primary == null) primary = ctrl;
        }

        return primary;
    }

    /// <summary>
    /// �� ���� ������ ��� ��� �߽ɿ��� maxRadius �̳��� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="position">�÷��̾� ���� ��ġ</param>
    /// <param name="maxRadius">���� �ִ� ������</param>
    /// <returns>���ѵ� ��ġ</returns>
    private Vector3 ClampToArena(Vector3 position, float maxRadius)
    {
        return position.magnitude > maxRadius ? position.normalized * maxRadius : position;
    }

    #endregion
}
