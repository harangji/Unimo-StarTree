using System;
using UnityEngine;

public class BoomBoomEngineCallbackSystem : MonoBehaviour
{
    public class EngineActiveCallback
    {
        public event Action<int, PlayerStatManager> OnEngineActivated;

        public void ActivateEngine(int skillID, PlayerStatManager player)
        {
            Debug.Log($"ºØºØ¿£Áø ¾×Æ¼ºê ¹ßµ¿ (SkillID={skillID})");
            OnEngineActivated?.Invoke(skillID, player);
        }
    }

    public static EngineActiveCallback ActiveCallback = new EngineActiveCallback();

    [SerializeField] private BoomBoomEngineEffectController effectController;

    private void Start()
    {
        ActiveCallback.OnEngineActivated += HandleEngineActivated;
    }

    private void HandleEngineActivated(int skillID, PlayerStatManager player)
    {
        effectController.ActivateEffect(skillID, player);
    }
}