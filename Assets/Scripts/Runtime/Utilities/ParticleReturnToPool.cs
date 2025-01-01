using Ahmet.ObjectPool;
using UnityEngine;

namespace Runtime.Utilities
{
    public class ParticleReturnToPool : MonoBehaviour
    {
        private void OnParticleSystemStopped()
        {
            ObjectPoolManager.ReturnObjectToPool(gameObject);
        }
    }
}