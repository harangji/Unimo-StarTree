using System;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class SpawnData : IKeyedData
{
    [field: SerializeField, ReadOnly] public string Key { get; set; }
    [field: SerializeField, ReadOnly] public int Species { get; set; }
    [field: SerializeField, ReadOnly] public int Pattern { get; set; }
}