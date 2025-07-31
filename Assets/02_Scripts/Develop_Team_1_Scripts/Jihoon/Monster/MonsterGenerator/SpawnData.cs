using CsvHelper.Configuration.Attributes;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class SpawnData : IKeyedData
{
    [field: SerializeField, ReadOnly] public string Key { get; set; }
    [field: SerializeField, ReadOnly] public int Species { get; set; }
    [field: SerializeField, ReadOnly] public int Pattern { get; set; }

    [field: SerializeField, TypeConverter(typeof(IntArrayConverter)), ReadOnly] public int[] MonsterSpawnRates { get; set; }
}