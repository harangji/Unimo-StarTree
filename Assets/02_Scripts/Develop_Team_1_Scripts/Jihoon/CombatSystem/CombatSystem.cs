using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    private const int MAX_EVENT_PROCESS_COUNT = 10;

    public class Callbacks
    {
        //CombatEvent가 발생하면의 의미로 쓸거임
        public Action<CombatEvent> OnCombatEvent;
        public Action<HealEvent> OnHealEvent;
    }

    public static CombatSystem Instance;

    private Dictionary<Collider, IDamageAble> monsterDic
        = new Dictionary<Collider, IDamageAble>();

    private Queue<InGameEvent> inGameEventQueue = new Queue<InGameEvent>();

    public readonly Callbacks Events = new Callbacks();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        int processCount = 0;

        while (inGameEventQueue.Count > 0 && processCount < MAX_EVENT_PROCESS_COUNT)
        {
            var inGameEvent = inGameEventQueue.Dequeue();

            switch (inGameEvent.Type)
            {
                case InGameEvent.EventType.Combat:
                    var combatEvent = inGameEvent as CombatEvent;
                    inGameEvent.Receiver.TakeDamage(combatEvent);
                    Events.OnCombatEvent?.Invoke(combatEvent);
                    break;
                case InGameEvent.EventType.Heal:
                    var healEvent = inGameEvent as HealEvent;
                    inGameEvent.Receiver.TakeHeal(healEvent);
                    Events.OnHealEvent?.Invoke(healEvent);
                    break;
                default:
                    break;
            }

            processCount++;
        }
    }

    public void RegisterMonster(IDamageAble monster)
    {
        if (monsterDic.TryAdd(monster.MainCollider, monster) == false)
        {
            Debug.LogWarning($"{monster.GameObject.name}가 등록되어 있습니다." +
                             $"{monsterDic[monster.MainCollider]}를 대체합니다");
            monsterDic[monster.MainCollider] = monster;
        }
    }

    public void RegisterMonster(Collider collider, IDamageAble monster)
    {
        if (monsterDic.TryAdd(collider, monster) == false)
        {
            Debug.LogWarning($"{monster.GameObject.name}가 등록되어 있습니다." +
                             $"{monsterDic[collider]}를 대체합니다");
            monsterDic[collider] = monster;
        }
    }

    public IDamageAble GetMonsterOrNull(Collider collider)
    {
        if (monsterDic.ContainsKey(collider))
        {
            return monsterDic[collider];
        }

        return null;
    }

    public void AddInGameEvent(InGameEvent inGameEvent)
    {
        inGameEventQueue.Enqueue(inGameEvent);
    }
}