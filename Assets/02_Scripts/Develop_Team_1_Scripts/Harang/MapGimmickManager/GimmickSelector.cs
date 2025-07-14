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
//     [SerializeField] private List<Gimmick> specialModes; // 스페셜 모드 스크립트 (오브젝트로 만들어놓고 할당)
//     
//     //경고 연출s
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
//     //     new Gimmick("특수한 꽃", 100),
//     //     new Gimmick("장판밟기", 80),
//     //     new Gimmick("지역 활성화", 70),
//     //     new Gimmick("블랙홀", 60),
//     //     new Gimmick("레드존", 50)
//     // };
//     
//     // 가중치 기반 추첨
//     public void PickRandomGimmick(List<Gimmick> candidates) //스테이지 받기
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
//         // fallback (예외 케이스)
//         return candidates[candidates.Count - 1];
//         StartCoroutine(triggerModeCoroutine(mode));
//     }
//     
//     private IEnumerator triggerModeCoroutine(int idx) //코루틴으로 모드 진행
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
//             // specialModes.RemoveAt(idx); //내보낸 모드 지우기
//         }
//         
//         warningPopup.SetActive(true);
//         yield return new WaitForSecondsRealtime(2.5f);
//         warningPopup.SetActive(false);
//         sceneCtrl.ResumeGame();
//         yield break;
//     }
// }