using UnityEngine;
using UnityEngine.UI;

public class UIUnlockAllButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            CosumePanelManager.Instance.UnlockAll();
        });
    }
}