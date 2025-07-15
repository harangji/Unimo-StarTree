using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRotationStuner : MonoBehaviour
{
    private float rotStunTime = 4f;
    private float stunRotation = 2500f;
    private float accumRotation = 0f;
    private float rDecayPerSec = 200f;
    private float prevRot = 0f;
    private PlayerStatManager statManager;
    private void Start()
    {
        statManager = GetComponent<PlayerStatManager>();
        prevRot = transform.rotation.eulerAngles.y;
    }
    // Update is called once per frame
    void Update()
    {
        float newangle = transform.rotation.eulerAngles.y;
        float diff = newangle - prevRot;
        if (Mathf.Abs(diff) > 180f) 
        {
            if (diff > 0) { diff -= 360f; }
            else { diff += 360f; }
        }
        accumRotation += diff;
        float sign = (accumRotation > 0f) ? 1f : -1f;
        accumRotation -= sign * rDecayPerSec * Time.deltaTime;
        prevRot = newangle;
        if (Mathf.Abs(accumRotation) > stunRotation)
        {
            // statManager.Hit(rotStunTime, transform.position);
            accumRotation = 0f;
        }
    }
}
