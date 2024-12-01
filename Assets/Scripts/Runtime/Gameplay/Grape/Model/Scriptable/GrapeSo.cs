using UnityEngine;

namespace Runtime.Gameplay.Grape.Model.Scriptable
{
    [CreateAssetMenu(fileName = "GrapeData", menuName = "Scriptable Object/GrapeData", order = 0)]
    public class GrapeSo : ScriptableObject
    {
        public Material FailedMaterial;
    }
}