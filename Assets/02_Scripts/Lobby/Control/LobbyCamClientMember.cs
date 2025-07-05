using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamClientMember : MonoBehaviour
{
    static protected LobbyClientMasterCtrl masterControllerSTATIC;
    protected bool isInControl = false;

    static public void SetMasterCtrlSTATIC(LobbyClientMasterCtrl master)
    {
        masterControllerSTATIC = master;
    }
    
}
