using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Managers
{
    public class SoundManager
    {
        private readonly AudioSource _audioSource;
        
        private readonly AudioClip[] _audioClips;
        
        public SoundManager(SoundManagerConfig config)
        {
            _audioSource = config.AudioSource;
            _audioClips = config.AudioClips;
        }
        
        public void PlayPopSound(AudioClipType audioClipType)
        {
            _audioSource.PlayOneShot(_audioClips[(int) audioClipType]);
        }
    }
    
    [Serializable]
    public struct SoundManagerConfig
    {
        public AudioSource AudioSource;
        
        public AudioClip[] AudioClips;
    }
}