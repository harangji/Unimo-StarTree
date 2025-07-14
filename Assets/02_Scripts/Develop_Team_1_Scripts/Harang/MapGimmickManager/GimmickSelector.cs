// using System;
// using System.Collections;
// using System.Collections.Generic;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// using Random = Unity.Mathematics.Random;
//
// public class GimmickSelector : MonoBehaviour
// {
//     [SerializeField] private List<Gimmick> specialModes; // ����� ��� ��ũ��Ʈ (������Ʈ�� �������� �Ҵ�)
//     
//     //��� ����s
//     [SerializeField] private GameObject warningPopup;
//     [SerializeField] private List<TextMeshProUGUI> modeText;
//     [SerializeField] private List<Image> modeIcon;
//     [SerializeField] private AudioSource bgmSource; 
//     [SerializeField] private AudioClip modeBGM;
//     [SerializeField] private Color goodModeTxtColor;
//     [SerializeField] private Color goodModeImgColor;
//     
//     void Start()
//     {
//         PickRandomGimmick();
//     }
//
//
//     // public List<Gimmick> gimmicks = new List<Gimmick>()
//     // {
//     //     new Gimmick("Ư���� ��", 100),
//     //     new Gimmick("���ǹ��", 80),
//     //     new Gimmick("���� Ȱ��ȭ", 70),
//     //     new Gimmick("��Ȧ", 60),
//     //     new Gimmick("������", 50)
//     // };
//     
//     // ����ġ ��� ��÷
//     public void PickRandomGimmick(List<Gimmick> candidates) //�������� �ޱ�
//     {
//         int totalWeight = 0;
//         foreach (var gimmick in candidates)
//             totalWeight += gimmick.Weight;
//
//         float rand = UnityEngine.Random.Range(0f, totalWeight);
//         float cumulative = 0f;
//
//         foreach (var gimmick in candidates)
//         {
//             cumulative += gimmick.Weight;
//             if (rand < cumulative)
//                 return gimmick;
//         }
//
//         // fallback (���� ���̽�)
//         return candidates[candidates.Count - 1];
//         StartCoroutine(triggerModeCoroutine(mode));
//     }
//     
//     private IEnumerator triggerModeCoroutine(int idx) //�ڷ�ƾ���� ��� ����
//     {
//         yield return new WaitForSeconds(PlayProcessController.InitTimeSTATIC);
//         PlaySceneController sceneCtrl = FindAnyObjectByType<PlaySceneController>();
//         sceneCtrl.PauseGame();
//         int modenum = (GameManager.Instance.CurrentStage() < 40) ? 1 :
//             (GameManager.Instance.CurrentStage() < 100) ? 2 : 3;
//         if (modenum > 3) { modenum = 3; }
//         for (int i = 0; i < modenum; i++)
//         {
//             Random.InitState(GameManager.Instance.CurrentStage());
//             // int idx = Random.Range(0, specialModes.Count);
//             specialModes[idx].SetModeName(modeText[i]);
//             modeIcon[i].sprite = specialModes[idx].ModeIcon;
//             if (specialModes[idx].ModeCode.Equals("mode7") || specialModes[idx].ModeCode.Equals("mode8"))
//             {
//                 modeText[i].color = goodModeTxtColor;
//                 modeIcon[i].color = goodModeImgColor;
//             }
//             modeIcon[i].gameObject.SetActive(true);
//             specialModes[idx].ExcuteGimmick(); 
//             // specialModes.RemoveAt(idx); //������ ��� �����
//         }
//         
//         warningPopup.SetActive(true);
//         yield return new WaitForSecondsRealtime(2.5f);
//         warningPopup.SetActive(false);
//         sceneCtrl.ResumeGame();
//         yield break;
//     }
// }