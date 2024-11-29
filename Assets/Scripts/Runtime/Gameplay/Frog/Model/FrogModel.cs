using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Model
{
    public class FrogModel
    {
        public ColorType ColorType;
        
        public readonly int AnimationHashMouthOpen = Animator.StringToHash("MouthOpen");
        public readonly int AnimationHashMouthClose = Animator.StringToHash("MouthClose");
        
        public FrogModel(FrogModelConfig config)
        {
            ColorType = config.ColorType;
        }
    }
    
    [Serializable]
    public struct FrogModelConfig
    {
        public ColorType ColorType;
    }
}