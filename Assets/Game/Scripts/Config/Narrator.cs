using System;
using UnityEngine;

namespace Game.Scripts.Config
{
    [CreateAssetMenu(fileName = "Narrators", menuName = "Game/Narrators")]
    public class Narrators : ScriptableObject
    {
        [Serializable]
        public class Narrator
        {
            public string Name;
            public RectTransform Icon;
            public Color Color;
            public Color TextColor;
        }

        
        public Narrator Triangle;
        public Narrator Rebel;
        public Narrator Unknown;
        public Narrator Self;
    }
}