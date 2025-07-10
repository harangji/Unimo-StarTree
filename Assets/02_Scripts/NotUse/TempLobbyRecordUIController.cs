// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
//
// public class TempLobbyRecordUIController : MonoBehaviour
// {
//     [SerializeField] private List<TextMeshProUGUI> recordTexts;
//     // private GameRecordManager recordManager;
//     // Start is called before the first frame update
//     void Start()
//     {
//         // recordManager = FindAnyObjectByType<GameRecordManager>();
//         ResetAllGameRecords();
//     }
//
//     public void ResetAllGameRecords()
//     {
//         for (int i = 0; i < recordTexts.Count; i++)
//         {
//             recordTexts[i].text = StringMethod.ToCurrencyString(recordManager.GetBestRecord(i));
//         }
//     }
// }
