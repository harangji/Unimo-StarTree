using UnityEngine;

public class GameDataSOHolder : SingletonBehaviour<GameDataSOHolder>
{
    [field: SerializeField] public GimmicParsingDataSO GimmicParsingDataSo { get; private set; }
}
