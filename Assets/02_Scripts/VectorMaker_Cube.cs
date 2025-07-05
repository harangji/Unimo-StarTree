using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMaker_Cube : MonoBehaviour
{
    public bool GetCollision = false;
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Map"))
        {
            GetCollision = true;
        }
    }
}
