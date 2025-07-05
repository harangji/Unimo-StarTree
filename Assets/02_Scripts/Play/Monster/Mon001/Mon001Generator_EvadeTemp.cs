using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mon001Generator_EvadeTemp : MonsterGenerator
{
    private float genWidth = 11.5f; 

    // Start is called before the first frame update
    new void OnEnable()
    {
        base.OnEnable();
        genWidth = PlaySystemRefStorage.mapSetter.MaxRange;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    protected override Vector3 findGenPosition()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = genWidth * Random.Range(-0.5f, 0.5f);
        Vector3 pos = new Vector3(rand, 25f, 1f);
        return pos;
    }
    protected override Quaternion setGenRotation(Vector3 genPos)
    {
        Quaternion quat = Quaternion.LookRotation(Vector3.down, Vector3.back);
        return quat;
    }
}