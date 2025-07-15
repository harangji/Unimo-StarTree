using Sirenix.OdinInspector;
using UnityEngine;

public class TestOdin : MonoBehaviour
{
// Delayed and DelayedProperty attributes are virtually identical...
    [Delayed]
    [OnValueChanged("OnValueChanged")]
    public int DelayedField;

// ... but the DelayedProperty can, as the name suggests, also be applied to properties.
    [ShowInInspector, DelayedProperty]
    [OnValueChanged("OnValueChanged")]
    public string DelayedProperty { get; set; }

    private void OnValueChanged()
    {
        Debug.Log("Value changed!");
    }
}
