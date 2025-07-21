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
        Debug.Log("무언가와 충돌 중");
        
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerStatManager>(out var player))
            {
                Vector3
                    hitPos = transform.position; //other.ClosestPoint(transform.position + new Vector3(0f, 1.5f, 0f))
                hitPos.y = 0f;

                //todo 이 부분에서 나중에 컴벳 시스템으로 바꿔야 함 -> 지금은 그냥 Hit 메서드를 변경함
                var playerIDamageAble = GameObject.FindGameObjectWithTag("Player").GetComponent<IDamageAble>();

                CombatEvent combatEvent = new CombatEvent
                {
                    Sender = this,
                    Receiver = playerIDamageAble,
                    Damage = damage,
                    HitPosition = hitPos,
                    Collider = other
                };

                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
            
            Destroy(gameObject);
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
