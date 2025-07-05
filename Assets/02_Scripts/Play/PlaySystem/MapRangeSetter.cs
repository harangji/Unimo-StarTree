using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRangeSetter : MonoBehaviour
{
    public float MaxRange { get; protected set; } = 10f;
    protected void Awake()
    {
        PlaySystemRefStorage.mapSetter = this;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        setMaxRanges();
    }

    virtual protected void setMaxRanges()
    {

    }
    virtual public bool IsInMap(Vector3 point)
    {
        return true;
    }
    
    virtual public Vector3 FindNearestPoint(Vector3 point)
    {
        return Vector3.zero;
    }
}
