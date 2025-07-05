using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSET : MonoBehaviour
{
    public string BGMNAME;
    void Start()
    {
        Sound_Manager.instance.Play(Sound.Bgm, BGMNAME);
    }

}
