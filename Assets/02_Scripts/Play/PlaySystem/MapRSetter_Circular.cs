using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRSetter_Circular : MapRangeSetter
{
    [SerializeField] private float mapRadius = 17.5f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        CamMover_ST001 cammover = FindAnyObjectByType<CamMover_ST001>(); //Should set cam range in Map setter to control execution order
        if (cammover != null) { cammover.setMaximumRange(MaxRange); }
    }
    protected override void setMaxRanges()
    {
        MaxRange = mapRadius;
    }
    public override bool IsInMap(Vector3 point)
    {
        Vector2 pos = new Vector2(point.x, point.z);
        return pos.magnitude <= MaxRange;
    }
    public override Vector3 FindNearestPoint(Vector3 point)
    {
        Vector2 pos = new Vector2(point.x, point.z);
        float angle = pos.AngleInXZ();
        Vector3 newpoint = new Vector3(mapRadius * Mathf.Cos(angle), 0f, mapRadius * Mathf.Sin(angle));
        return newpoint;
    }
}
