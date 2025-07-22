public class ActiveBuff
{
    public SCharacterStat StatBonus { get; private set; }
    public float DurationRemaining { get; private set; }
    public bool IsExpired => DurationRemaining <= 0f;

    public ActiveBuff(SCharacterStat statBonus, float duration)
    {
        StatBonus = statBonus;
        DurationRemaining = duration;
    }

    public void UpdateBuff(float deltaTime)
    {
        DurationRemaining -= deltaTime;
    }
}