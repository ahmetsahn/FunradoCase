using Runtime.Enums;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Model.Scriptable
{
    [CreateAssetMenu(fileName = "FrogData", menuName = "Scriptable Object/FrogData", order = 0)]
    public class FrogSo : ScriptableObject
    {
        public ColorType ColorType;
        
        public GameObject DestroyParticle;
    }
}