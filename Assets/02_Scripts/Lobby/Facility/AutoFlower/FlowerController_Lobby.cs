using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController_Lobby : MonoBehaviour
{
    static public FlowerResourceGetManager resourceManager;
    private float destroyWaitTime = 2f;
    private FlowerFXController_Lobby fxController;
    // Start is called before the first frame update
    void Start()
    {
        fxController = GetComponent<FlowerFXController_Lobby>();
    }

    public void HarvestFlower(Transform unimoPos)
    {
        fxController.TriggerHarvestFX(unimoPos);
        resourceManager.GetResource();
        Destroy(gameObject, destroyWaitTime);
    }
    public void GrowFlower(float ratio)
    {
        fxController.AffectFlowerFX(ratio);
    }
    public void StartHarvest()
    {
        fxController.SetFlowerActivate();
    }
}
