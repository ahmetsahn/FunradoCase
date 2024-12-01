using System;
using Runtime.Enums;
using Runtime.Gameplay.Frog.Model.Scriptable;
using UnityEngine;

namespace Runtime.Gameplay.Frog.Model
{
    public class FrogModel
    {
        public readonly int AnimationHashMouthOpen = Animator.StringToHash("MouthOpen");
        public readonly int AnimationHashMouthClose = Animator.StringToHash("MouthClose");

        public FrogSo Data;
        
        public FrogModel(FrogSo data)
        {
            Data = data;
        }
    }
}