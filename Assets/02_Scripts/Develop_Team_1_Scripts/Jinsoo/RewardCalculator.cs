/// <summary>
/// �������� ���� ���� Ŭ���� (��� �����̱� ������ static ó���߽��ϴ�) <= Jinsu
/// </summary>
public static class RewardCalculator
{
    /// <summary>
    /// ������� ������� ���
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static double GetYellowReward(int stage)
    {
        double reward = 17500 * stage;
        if (stage % 5 == 0)
            reward *= 2;
        return reward;
        
        // ���� ���� ����
        // double reward = 1250 + (stage - 1) * 5;
        //
        // if (stage >= 101) reward += 2500;
        // if (stage >= 251) reward += 2500;
        // if (stage >= 601) reward += 2500;
        //
        // if (stage % 10 == 5)
        //     reward *= 2;
        //
        // return reward;
    }

    /// <summary>
    /// ��Ȳ���� ������� ���
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static double GetRedReward(int stage)
    {
        if (stage % 10 != 0)
            return 0;

        return 25000 * (stage / 10);
        
        // ���� ���� ����
        // int baseValue = 2 + ((stage - 1) / 10) * 2;
        //
        // if (stage >= 101) baseValue += 90;
        // if (stage >= 251) baseValue += 45;
        // if (stage >= 601) baseValue += 325;
        //
        // if (stage % 10 == 5)
        //     return baseValue * 2;
        //
        // return baseValue;
    }
}