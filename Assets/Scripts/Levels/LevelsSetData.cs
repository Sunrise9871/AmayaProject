using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "New LevelsSetData", menuName = "Levels Set Data", order = 10)]
    public class LevelsSetData : ScriptableObject
    {
        [field: SerializeField] public LevelData[] LevelsData { get; private set; }
    }
}