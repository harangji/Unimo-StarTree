using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RemoveADS : UI_Base
{
    public void GetRemoveADS()
    {
        Base_Mng.IAP.Purchase("removeads");
    }

    public void GetRemoveAllADS()
    {
        Base_Mng.IAP.Purchase("revemoads_all");
    }
}
