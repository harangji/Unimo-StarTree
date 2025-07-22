using System;
using UnityEngine;

public class Pattern1Bullet : MonoBehaviour, IDamageAble
{
    [SerializeField] private int damage;
    [SerializeField] private Collider mainCollider;

    public Collider MainCollider => mainCollider;
    public GameObject GameObject => gameObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerStatManager>(out var player))
            {
                Vector3
                    hitPos = transform.position; //other.ClosestPoint(transform.position + new Vector3(0f, 1.5f, 0f))
                hitPos.y = 0f;
    
                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = this,
                    Receiver = player,
                    Damage = damage,
                    HitPosition = hitPos,
                    Collider = other,
                    CanBeStunned = true,
                };
    
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            
            Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(CombatEvent combatEvent)
    {
        throw new NotImplementedException();
    }

    public void TakeHeal(HealEvent combatEvent)
    {
        throw new NotImplementedException();
    }
}