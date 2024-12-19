using System;
using UnityEngine;

namespace Levels
{
    [Serializable]
    public class LevelData
    {
        [field: SerializeField] public int RowsCount { get; private set; }
        [field: SerializeField] public int ColumnsCount { get; private set; }
    }
}