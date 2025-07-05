using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacilityBehaviour : MonoBehaviour
{
    [SerializeField] protected FacilityLvUpTable lvUpTable;

    // Start is called before the first frame update
    protected void Start()
    {
        IDLELvManager idle = FindAnyObjectByType<IDLELvManager>();
        idle.SubscribeLvUpAction(checkTierUp);
        idle.SubscribeLvUpAction(checkUpgrade);
        idle.SubscribeUpListFunc(nextUpListText);
    }

    virtual protected void checkTierUp(int newLv)
    { }
    virtual protected void checkUpgrade(int newLv)
    { }
    virtual protected string nextUpListText(int newLv)
    {
        return string.Empty;
    }
}
