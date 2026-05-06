using System;
using UnityEngine;

namespace Code.Effects.SkillEffect
{
    public class BeamEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem middleBeamEffect;
        
        private ParticleSystem.MainModule mainParticle;
        private ParticleSystem.MainModule middleParticle;

        private float mainLifeTime;
        
        private void Awake()
        {
            mainParticle = GetComponent<ParticleSystem>().main;
            middleParticle = middleBeamEffect.main;
        }

        public void SetUpBeamEffect(float lifeTime, float distance)
        {
            middleParticle.startLifetime = lifeTime;
            mainParticle.startLifetime = lifeTime;
        }
    }
}