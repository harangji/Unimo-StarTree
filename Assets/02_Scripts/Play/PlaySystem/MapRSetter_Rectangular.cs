using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRSetter_Rectangular : MapRangeSetter
{
    [SerializeField] private float mapHeight = 0f;
    [SerializeField] private float mapWidth = 35f * 0.33333f;
    private float maxX, minX;
    private float maxY, minY;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        maxX = mapWidth / 2f;
        minX = -maxX;
        maxY = mapHeight / 2f;
        minY = -maxY;
    }

    protected override void setMaxRanges()
    {
        MaxRange = Mathf.Max(mapHeight, mapWidth)/2f;
    }
    public override bool IsInMap(Vector3 point)
    {
        Vector2 pos = new Vector2(point.x, point.z);
        bool isOut = true;
        isOut &= pos.x < maxX;
        isOut &= pos.x > minX;
        isOut &= pos.y < maxY;
        isOut &= pos.y > minY;

        return isOut;
    }
    public override Vector3 FindNearestPoint(Vector3 point)
    {
        Vector2 pos = new Vector2(point.x, point.z);
        if (pos.x > maxX) { pos.x = maxX; }
        else if (pos.x < minX) { pos.x = minX; }
        if (pos.y > maxY) { pos.y = maxY; }
        else if (pos.y < minY) { pos.y = minY; }
        Vector3 newpoint = new Vector3(pos.x, point.y, pos.y);
        return newpoint;
    }
}
