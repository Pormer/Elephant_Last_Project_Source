using Code.Modules;
using UnityEngine;

namespace Code.Contexts.Summons
{
    public class AfterImageContext : SummonContext
    {
        public Sprite Sprite { get; set; }
        public bool IsFacingRight { get; set; }
        public float Duration { get; set; }
        public float FadeTime { get; set; }
        public bool IsBlink { get; set; }
        
        public AfterImageContext(ModuleOwner owner, Vector3 position, Vector3 rotation, Sprite sprite, bool isFacingRight, float duration, float fadeTime, bool isBlink) : base(owner, position, rotation)
        {
            Sprite = sprite;
            IsFacingRight = isFacingRight;
            Duration = duration;
            FadeTime = fadeTime;
            IsBlink = isBlink;
        }
    }
}