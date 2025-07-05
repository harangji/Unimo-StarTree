using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon003mislActivator : MonoBehaviour
{
    [SerializeField] private Mon003mislState_Action actionState;
    public void ActivateMissle(Transform player, Vector3 masterPos)
    {
        GetComponent<MonsterController>().InitEnemy(player);
        actionState.SetCenterPos(masterPos);
    }
}
