using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RemoveADS : UI_Base
{
    public void GetRemoveADS()
    {
        Base_Manager.IAP.Purchase("removeads");
    }

    public void GetRemoveAllADS()
    {
        Base_Manager.IAP.Purchase("revemoads_all");
    }
}
