using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InGameEvent
{
    public enum EventType
    {
        Unknown,
        Combat,
        Heal,
    }
    
    public IDamageAble Sender { get; set; }
    public IDamageAble Receiver { get; set; }
    public abstract EventType Type { get; }
}

public class CombatEvent : InGameEvent
{
    public int Damage { get; set; }
    public Vector3 HitPosition { get; set; }
    public Collider Collider { get; set; }
    public bool CanBeStunned { get; set; }
    public bool IsStrongKnockback { get; set; }
    public bool IsTimeReduceMod { get; set; } = false; // 시간을 감소시키는 모드인지
    public float TimeReduceAmount { get; set; } = 0f; // 시간을 얼마나 감소시켜야 하는지 추가.
    public override EventType Type => EventType.Combat;
}

public class HealEvent : InGameEvent
{
    public float Heal { get; set; }
    public override EventType Type => EventType.Heal;
}