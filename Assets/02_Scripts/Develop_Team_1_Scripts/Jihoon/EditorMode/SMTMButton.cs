using UnityEngine;
using UnityEngine.UI;

public class SMTMButton : MonoBehaviour
{
    [SerializeField] private Button button;

    void Start()
    {
        button.onClick.AddListener(() => {
            Base_Manager.Data.UserData.Yellow += 1000000;
            Base_Manager.Data.UserData.Red += 1000000;
            Base_Manager.Data.UserData.Blue += 1000000;
        });
        
        button.gameObject.SetActive(EditorMode.Instance.isEditor);
    }
} 

