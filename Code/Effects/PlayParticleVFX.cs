using UnityEngine;

namespace Code.Effects
{
    public class PlayParticleVFX : MonoBehaviour, IPlayableVFX
    {
        [field:SerializeField] public string VFXName { get; private set; }
        [SerializeField] private bool isOnPosition;
        [SerializeField] private bool lockRotation;
        
        private ParticleSystem _particle;

        protected void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void PlayVFX(Vector3 position, Quaternion rotation)
        {
            if(isOnPosition == false)
                transform.position = position;
            if(lockRotation == false)
                transform.rotation = rotation;

            _particle.Play(true);
        }

        public void StopVFX()
        {
            _particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        
        public void SoftStopVFX()
        {
            _particle.Stop(true);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(VFXName) == false)
                gameObject.name = VFXName;
        }
    }
}