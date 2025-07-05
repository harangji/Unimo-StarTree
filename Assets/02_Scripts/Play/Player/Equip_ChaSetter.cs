using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Equip_ChaSetter : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    private void Awake()
    {
        if (pivot == null) { pivot = transform.Find("pivot_Character"); }
        if (gameObject.name == "EQ013_Toaster(Clone)") pivot.transform.parent = transform.Find("rig_CH");
    }
    public void InstCharacter(GameObject character, out Animator charAnim, out PlayerFaceController faceCtrl)
    {
        GameObject cha = Instantiate(character, pivot);
        charAnim = cha.GetComponent<Animator>();
        faceCtrl = cha.GetComponent<PlayerFaceController>();
    }
    public void InstCharacterAsync(string chakey, Action loadedAction, Action<GameObject> animInitAction)
    {
        Addressables.InstantiateAsync(chakey, pivot).Completed += (op) =>
        {
            if (op.Status != AsyncOperationStatus.Succeeded) { return; }
            GameObject chaobj = op.Result.gameObject;
            if (loadedAction != null) { loadedAction.Invoke(); }
            if (animInitAction != null) { animInitAction.Invoke(chaobj); }
        };
    }
}
