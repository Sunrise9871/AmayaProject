using System;
using UnityEngine;

namespace Cards
{
    [Serializable]
    public class CardData
    {
        [field: SerializeField] public string Identifier { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        [field: SerializeField] public float SpriteRotatedByDegrees { get; private set; }
    }
}