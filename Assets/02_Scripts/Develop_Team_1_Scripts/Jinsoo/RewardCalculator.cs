/// <summary>
/// 스테이지 최초 보상 및 방치 보상 계산 클래스 (계산 전용이기 때문에 static 처리했습니다) <= Jinsu
/// </summary>
public static class RewardCalculator
{
    /// <summary>
    /// 노란별꿀 보상로직 계산
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static double GetYellowReward(int stage)
    {
        double reward = 17500 * stage;
        if (stage % 5 == 0)
            reward *= 2;
        return reward;
        
        // 이전 보상 로직
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
    /// 주황별꿀 보상로직 계산
    /// </summary>
    /// <param name="stage"></param>
    /// <returns></returns>
    public static double GetRedReward(int stage)
    {
        if (stage % 10 != 0)
            return 0;

        return 25000 * (stage / 10);
        
        // 이전 보상 로직
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

    /// <summary>
    /// Alta 레벨에 따른 노랑별꿀 방치 획득량
    /// </summary>
    /// <returns></returns>
    public static double GetYfByAltaLevel()
    {
        int level = Base_Manager.Data.UserData.Level + 1;
        if (level < 1) return 0;

        double yfValue = 0;

        if (level < 100)
            yfValue = 1100 * level;
        else if (level < 300)
            yfValue = 220000 + 2200 * (level - 100);
        else if (level < 700)
            yfValue = 1320000 + 4400 * (level - 300);
        else if (level < 1000)
            yfValue = 6160000 + 8800 * (level - 700);
        else if (level == 1000)
            yfValue = 17600000;
        
        return yfValue;
    }
    
    /// <summary>
    /// Alta 레벨에 따른 주황별꿀 방치 획득량
    /// </summary>
    /// <returns></returns>
    public static double GetOfByAltaLevel()
    {
        int level = Base_Manager.Data.UserData.Level + 1;
        if (level < 1) return 0;

        double ofValue = 0;

        if (level < 100)
            ofValue = 13.5 * level;
        else if (level < 300)
            ofValue = 2700 + 27 * (level - 100);
        else if (level < 700)
            ofValue = 16200 + 54 * (level - 300);
        else if (level < 1000)
            ofValue = 75600 + 108 * (level - 700);
        else if (level == 1000)
            ofValue = 216000;

        return ofValue;
    }
    
    /// <summary>
    /// Alta 강화 시 소모될 재화 계산
    /// </summary>
    /// <returns></returns>
    public static double GetLevelUpCost()
    {
        var level = Base_Manager.Data.UserData.Level + 1;
        
        const int baseCost = 250000;
        const int delta = 25;
        const int commonDiff = 50;
        
        int result = baseCost + delta * (level - 1) + (commonDiff * (level - 1) * (level - 2)) / 2;

        return result;
    }

    /// <summary>
    /// Alta 진화 시 소모될 재화 계산
    /// </summary>
    /// <returns></returns>
    public static double GetGradeUpCost()
    {
        var level = Base_Manager.Data.UserData.Level + 2;
        int result = 0;

        switch (level)
        {
            case 100 : result = 500000; break;
            case 300 : result = 1500000; break;
            case 700 : result = 3500000; break;
            case 1000 : result = 5000000; break;
        }
        return result;
    }
}