using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventProcessorClient : MonoBehaviour
{
    private EventProcessor processor;
    // Start is called before the first frame update
    void Start()
    {
        processor = GetComponent<EventProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            processor.CallNextText();
        }
    }
}
