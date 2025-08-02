using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterFactory : MonoBehaviour
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

    #region ���� ���� �Լ�

    /// <summary>
    /// Ȯ�� ������� ���͸� ��ȯ�մϴ�. ���� ���� �Լ��̹Ƿ� �����̰� �ʼ��� �ʿ��մϴ�.
    /// </summary>
    /// <param name="allowedPattern">���� ���� �� (���� �׷� Ȯ�� ���� ����)</param>
    /// <returns>��ȯ�� �� ���� ��Ʈ�ѷ�</returns>
    public abstract MonsterController SpawnMonster(int allowedPattern);

    /// <summary>
    /// Ȯ��(rate)�� ���� �ʿ��� �ڽ�Ʈ�� ��ȯ�մϴ�. ���� ���� �Լ��̹Ƿ� �����̰� �ʼ��� �ʿ��մϴ�.
    /// </summary>
    /// <param name="rate">������ ������ rate ��</param>
    /// <param name="allowedPattern">���� ���� �� (���� �׷� Ȯ�� ���� ����)</param>
    /// <returns>�ش� rate�� ���� �Ҹ� �ڽ�Ʈ</returns>
    protected abstract int GetCostFromRate(int rate, int allowedPattern);

    #endregion

    #region ���� �Լ�    

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

    #endregion

    #region ��� �Լ�

    /// <summary>
    /// ������ ù ��° ������ ��ȯ�ϴ� �⺻ �����Դϴ�.
    /// </summary>
    /// <param name="allowedPattern">���� ���� ��</param>
    /// <returns>��ȯ�� �� ���� ��Ʈ�ѷ�</returns>
    protected MonsterController DefaultSpawn(int allowedPattern)
    {
        Vector3 pos = FindGenPosition();
        Quaternion quat = SetGenRotation(pos);
        MonsterController controller = Instantiate(monsterPattern1, pos, quat).GetComponent<MonsterController>();
        controller.InitEnemy(PlayerTransform);

        return controller;
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
    
    /// <summary>
    /// ���� StageManager���� ���̵� �ڿ��� �Һ� �õ��մϴ�.
    /// </summary>
    /// <param name="cost">�Ҹ��� ���̵� ��ġ</param>
    /// <returns>���� ����</returns>
    protected bool TryConsumeDifficulty(int cost)
    {
        var stageMgr = PlaySystemRefStorage.stageManager;
        if (stageMgr == null || !stageMgr.TryConsumeDifficulty(cost))
        {
            Debug.Log("�Ҵ��� ���̵� ��ġ�� �����մϴ�.");
            return false;
        }
        return true;
    }

    #endregion
}