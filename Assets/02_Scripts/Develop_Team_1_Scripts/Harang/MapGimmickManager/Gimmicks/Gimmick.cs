using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum Grade
{
    Nomal,
    Enhanced,
    Elite,
    Legendary,
}

public class Gimmick : MonoBehaviour
{
    [field: SerializeField] public string GimmickID { get; set; }
    [field: SerializeField] public Sprite GimmickIcon { get; set; }
    [field: SerializeField] public string GimmickName { get; set; }
    [field: SerializeField] public int Cost { get; set; }
    public Grade GimmickGrade { get; set; }
    [field: SerializeField] public string RequiredTime { get; set; }
    [field: SerializeField] public string Duration { get; set; }
    [field: SerializeField] public string Weight { get; set; }
    
    public virtual void ExcuteGimmick(){}
    
    public void SetModeName(TextMeshProUGUI modeTxt)
    {
        modeTxt.text = GimmickName;
    }

    public void SetGrade(int currentStage)
    {
        
    }
}
