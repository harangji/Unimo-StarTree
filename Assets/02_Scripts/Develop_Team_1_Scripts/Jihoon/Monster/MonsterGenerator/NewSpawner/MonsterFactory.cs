using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFactory : MonoBehaviour
{
    protected class PatternGroup
    {
        public int Remaining;
        public int Cost;
    }

    protected List<PatternGroup> ActiveGroups = new List<PatternGroup>();

    [SerializeField] protected GameObject monsterPattern1;
    [SerializeField] protected GameObject monsterPattern2;
    [SerializeField] protected GameObject monsterPattern3;
    protected Transform PlayerTransform;

    private void Start()
    {
        PlayerMover mover = FindAnyObjectByType<PlayerMover>();
        if (mover != null)
        {
            PlayerTransform = mover.transform;
        }
    }

    /// <summary>
    /// ������ ������ �������� ���͸� �����մϴ�.
    /// </summary>
    /// <param name="allowedPattern">���� ���� �ε���</param>
    /// <returns>������ ���� ��Ʈ�ѷ�</returns>
    public virtual MonsterController SpawnMonster(int allowedPattern)
    {
        Vector3 pos = FindGenPosition();
        Quaternion quat = SetGenRotation(pos);
        MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        controller.InitEnemy(PlayerTransform);

        return controller;
    }

    /// <summary>
    /// ���͸� ������ ��ġ�� ����մϴ�.
    /// </summary>
    /// <returns>���� ��ġ ����</returns>
    protected virtual Vector3 FindGenPosition()
    {
        return Vector3.zero;
    }

    /// <summary>
    /// ���� ��ġ�� �´� ȸ�� ���� ����մϴ�.
    /// </summary>
    /// <param name="genPos">���� ��ġ</param>
    /// <returns>���� ȸ�� ���ʹϾ�</returns>
    protected virtual Quaternion SetGenRotation(Vector3 genPos)
    {
        return Quaternion.identity;
    }

    /// <summary>
    /// ���Ͱ� �ı��� �� ȣ��Ǵ� �ݹ��� ����մϴ�.
    /// </summary>
    /// <param name="ctrl">��� ���� ��Ʈ�ѷ�</param>
    /// <param name="group">�Ҽӵ� ���� �׷�</param>
    protected void RegisterDestroyCallback(MonsterController ctrl, PatternGroup group)
    {
        ctrl.OnDestroyed += (monster) =>
        {
            group.Remaining--;
            if (group.Remaining == 0)
            {
                PlaySystemRefStorage.stageManager.RestoreDifficulty(group.Cost);
                ActiveGroups.Remove(group);
            }
        };
    }
}