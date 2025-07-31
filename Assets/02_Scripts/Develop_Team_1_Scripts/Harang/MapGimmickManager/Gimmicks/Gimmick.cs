using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

public enum eGimmickGrade
{
    Nomal,
    Enhanced,
    Elite,
    Legendary,
}

public enum eGimmicks
{
    BlackHole,
    RedZone
}

public enum eGimmickType
{
    Dangerous,
    Helpful
}

[Serializable]
public class ToTsvGimmickData : IKeyedData
{
    //CsvHelper�� ������ ������Ƽ�� �ڵ� ������ �� �ִ�.
    [field: SerializeField, ReadOnly] public string Key { get; set; }
    [field: SerializeField, ReadOnly] public string GimmickName { get; set; }
    
    [field: SerializeField, TypeConverter(typeof(IntArrayConverter)), ReadOnly] public int[] Costs { get; set; }
    [field: SerializeField, TypeConverter(typeof(IntArrayConverter)), ReadOnly] public int[] Weights { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] Durations_s { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] EffectValue1 { get; set; }
    [field: SerializeField, TypeConverter(typeof(FloatArrayConverter)), ReadOnly] public float[] EffectValue2 { get; set; }
}



public abstract class Gimmick : MonoBehaviour
{
    private static readonly int COLOR = Shader.PropertyToID("_Color");

    [Header("��� ����")] 
    
    [LabelText("��� �̸�"), Tooltip("��� ���� �� UI�� ǥ���� ��� �̸�."), Required]
    public string gimmickName;

    [LabelText("��� ������"), Tooltip("��� ���� �� UI�� ǥ���� ������."), Required]
    public Sprite gimmickIcon;
    
    //ReadOnly
    [LabelText("��� ���"), Tooltip("����� ������ �� �������� �Ҵ�Ǵ� ����� ���"), ReadOnly]
    protected eGimmickGrade emGimmickGrade { get; set; } // �������� ������ ��� ���

    [LabelText("��� Ÿ��"), Tooltip("�طο� ���, �̷ο� ��� ����"), ReadOnly]
    public abstract eGimmickType eGimmickType { get;}
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected int mGimmickCost { get; set; }
    
    [LabelText("��� ���� �ð�"), ReadOnly] 
    protected float mGimmickDuration { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 1"), ReadOnly]
    protected float mGimmickEffectValue1 { get; set; }
    
    [LabelText("��� ȿ�� ��ġ 2"), ReadOnly]
    protected float mGimmickEffectValue2 { get; set; }
    
    [SerializeField] private ParticleSystem[] mParticleSystems;
    private ParticleSystemRenderer[] mParticleSystemRenderers { get; set; }
    
    protected bool mbReadyExecute = false;
    
    public void Awake()
    {
        // if(mParticleSystems == null || mParticleSystems.Length > 0) return;
        
        mParticleSystemRenderers = new ParticleSystemRenderer[mParticleSystems.Length];

        for (int i = 0; i < mParticleSystems.Length; i++)
        {
            mParticleSystemRenderers[i] = mParticleSystems[i].GetComponent<ParticleSystemRenderer>();
            
            Material mat = mParticleSystemRenderers[i].material;

            if (!mat.HasProperty(COLOR)) continue;
            
            Color color = mat.color;
            color.a = 0f;
            mat.color = color;
        }
    }
    
    public abstract void ActivateGimmick(); //����
    public abstract void DeactivateGimmick(); //����

    //gimmickGrade�� �ڱ� �ڽ��� �ʱ�ȭ //��� ����, �غ� �ܰ�
    public void InitializeGimmick(GimmickInitializer gimmickInitializer, eGimmickGrade gimmickGrade)
    {
        emGimmickGrade = gimmickGrade;
        mGimmickCost = gimmickInitializer.Costs[(int)gimmickGrade];
        mGimmickDuration = gimmickInitializer.Durations_s[(int)gimmickGrade];
        mGimmickEffectValue1 = gimmickInitializer.EffectValue1[(int)gimmickGrade];
        mGimmickEffectValue2 = gimmickInitializer.EffectValue2[(int)gimmickGrade];
    }
    
    private readonly Dictionary<eGimmickGrade, string> mGradeTextTable = new()
        {
            { eGimmickGrade.Nomal, "�Ϲ�" },
            { eGimmickGrade.Enhanced, "��ȭ" },
            { eGimmickGrade.Elite, "����" },
            { eGimmickGrade.Legendary, "����" },
        };

    public void SetGimmickTMP(TextMeshProUGUI modeTxt)
    {
        if (mGradeTextTable.TryGetValue(emGimmickGrade, out string value))
        {
            modeTxt.SetText($"[{value}] {gimmickName}"); // ex) [����] ��Ȧ
        }
        else
        {
            modeTxt.text = gimmickName;
        }
    }
    
    //������ ��Ÿ����
    protected Task FadeAll(bool fadeIn, float duration = 1f)
    {
        float targetAlpha = fadeIn? 1f : 0f;
        
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        Sequence fadeSequence = DOTween.Sequence();
        
        foreach (ParticleSystemRenderer particleSystemRenderer in mParticleSystemRenderers)
        {
            Material mat = particleSystemRenderer.material;
            
            if (!mat.HasProperty(COLOR)) continue;
            
            Color color = mat.color;

            float startAlpha = color.a;

            Tween tween = DOTween.To(() => startAlpha, x =>
            {
                startAlpha = x;
                color.a = x;
                mat.color = color;
            }, targetAlpha, duration);
            
            fadeSequence.Join(tween); // ���ÿ� ����ǵ��� �������� �߰�
        }
        fadeSequence.OnComplete(() =>
        {
            MyDebug.Log($"Fade to {targetAlpha} Complete");
            tcs.SetResult(true);
        });

        return tcs.Task;
    }
}
