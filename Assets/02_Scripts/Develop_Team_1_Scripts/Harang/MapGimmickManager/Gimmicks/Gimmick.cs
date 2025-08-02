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
    RedZone,
    AreaActivator,
    UniqueFlower,
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
    [SerializeField] private Renderer[] mObjectRenderers;

    private ParticleSystemRenderer[] mParticleSystemRenderers;
    
    protected bool mbReadyExecute = false;
    protected bool mbDeactivateStart = false;
    protected float mbTimeElapsed = 0f;
    
    public void Awake()
    {
        if (mParticleSystems != null && mParticleSystems.Length > 0)
        {
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


        if (mObjectRenderers != null && mObjectRenderers.Length > 0)
        {
            for (int i = 0; i < mObjectRenderers.Length; i++)
            {
                Material mat = mObjectRenderers[i].material;

                if (!mat.HasProperty(COLOR)) continue;
                
                Color color = mat.color;
                color.a = 0f;
                mat.color = color;
            }
        }
    }

    protected void Update()
    {
        mbTimeElapsed += Time.deltaTime;

        if (mbTimeElapsed >= mGimmickDuration && !mbDeactivateStart) //���� �ð� ��� �� ��Ȱ�� && ���� ������ �� ����
        {
            mbDeactivateStart = true;
            DeactivateGimmick();
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

    private Sequence fadeSequence;
    
    //������ ��Ÿ����
    protected Task FadeAll(bool fadeIn, float duration = 1.5f)
    {
        fadeSequence = DOTween.Sequence();
        fadeSequence.Pause();
        
        float targetAlpha = fadeIn? 0.4f : 0f;
        
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        
        foreach (ParticleSystemRenderer particleSystemRenderer in mParticleSystemRenderers)
        {
            Material mat = particleSystemRenderer.material;
            RenderFloatChanger(mat, targetAlpha, duration);
        }

        foreach (Renderer objectRenderer in mObjectRenderers)
        {
            Material mat = objectRenderer.material;
            RenderFloatChanger(mat, targetAlpha, duration);
        }
        
        fadeSequence.OnComplete(() =>
        {
            tcs.SetResult(true);
        });
        
        fadeSequence.Play();
        
        return tcs.Task;
    }

    private void RenderFloatChanger(Material mat, float targetAlpha, float duration)
    {
        if (!mat.HasProperty(COLOR)) return;
        
        Color mColor = mat.color;

        float mStartAlpha = mColor.a;
        
        Tween tween = DOTween.To(() => mStartAlpha, x =>
        {
            mStartAlpha = x;
            mColor.a = x;
            mat.color = mColor;
        }, targetAlpha, duration);
            
        fadeSequence.Join(tween); // ���ÿ� ����ǵ��� �������� �߰�
    }
}
