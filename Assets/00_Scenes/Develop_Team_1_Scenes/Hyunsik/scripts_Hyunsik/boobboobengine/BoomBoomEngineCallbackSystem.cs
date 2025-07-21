using System;
using UnityEngine;

public class BoomBoomEngineCallbackSystem : MonoBehaviour
{
    public class EngineActiveCallback
    {
        public event Action<int> OnEngineActivated;

        public void ActivateEngine(int skillID)
        {
            Debug.Log($"ºØºØ¿£Áø ¾×Æ¼ºê ¹ßµ¿ (SkillID={skillID})");
            OnEngineActivated?.Invoke(skillID);
        }
    }

    public static EngineActiveCallback ActiveCallback = new EngineActiveCallback();

    [SerializeField] private BoomBoomEngineEffectController effectController;

    private void Start()
    {
        ActiveCallback.OnEngineActivated += HandleEngineActivated;
    }

    private void HandleEngineActivated(int skillID)
    {
        effectController.ActivateEffect(skillID);
    }
}