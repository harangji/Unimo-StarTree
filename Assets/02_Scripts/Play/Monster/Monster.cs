using UnityEngine;

public class Monster : MonoBehaviour
{
    public int stage = 1;
    
    public int defaultDamage;
    public int skillDamage;

    //스킬을 사용할 때만 참으로 바뀌며, 데미지를 얼마나 줄지에 대한 논리값
    public bool bIsSkill = false;

    //todo 나중에 여기에 IDamageable 인터페이스 붙이고 메서드 추가하면 됨

    public int GetDamage()
    {
        return bIsSkill ? skillDamage : defaultDamage;
    }
}