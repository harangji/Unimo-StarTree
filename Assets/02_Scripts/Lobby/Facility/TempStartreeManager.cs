using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TempStartreeManager : MonoBehaviour
{
    [SerializeField] private FacilityLvUpTable lvUpTable;
    [SerializeField] private TextMeshProUGUI tierText;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private Transform treeTransform;
    // Start is called before the first frame update
    void Start()
    {
        IDLELvManager idle = FindAnyObjectByType<IDLELvManager>();
        idle.SubscribeLvUpAction(checkTierUp);
        idle.SubscribeLvUpAction(checkUpgrade);
        checkTierUp(0);
        checkUpgrade(0);
    }

    private void checkTierUp(int newLv)
    {
        if (newLv == 0) { return; }
        int oriTier = lvUpTable.CalculateTier(newLv - 1);
        int newTier = lvUpTable.CalculateTier(newLv);
        if (oriTier != newTier)
        {
            tierText.text = $"Tree Tier {newTier}";
            //treeTransform.localScale = (newTier + 1) * 2 * Vector3.one;
        }
    }
    private void checkUpgrade(int newLv)
    {
        List<bool> isUpgrades = lvUpTable.NextUpList(newLv);
        if (isUpgrades[0]) 
        {
            upgradeText.text = $"Tier will increase!";
            return; 
        }
        for (int i = isUpgrades.Count - 1; i > 0; i--)
        {
            if (isUpgrades[i])
            {
                upgradeText.text = $"Upgrade {i} will be applied!";
                if (i == 1) { treeTransform.localScale += 0.1f * Vector3.one; }
                break;
            }
        }
        var accumes = lvUpTable.CalculateUpAccumPerTier(newLv);
    }
}
