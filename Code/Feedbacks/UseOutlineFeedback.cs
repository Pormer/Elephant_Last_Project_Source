using Code.Utility.Properties;
using Code.Utility.Properties.Shaders;
using UnityEngine;

namespace Feedbacks
{
    public class UseOutlineFeedback : Feedback
    {
        [SerializeField] private ShaderPropertyManagerSO propertyManager;
        [SerializeField] private SpriteRenderer visual;
        [SerializeField] PropertyDataSO propertyData;
        [SerializeField] private bool useOutline;
        
        private Material _mat;

        private void Awake()
        {
            _mat = visual.material;
        }
        
        public override void PlayFeedback()
        {
            propertyManager.SetProperty(_mat, propertyData,  useOutline ? 1 : 0);
        }

        public override void StopFeedback()
        {
            propertyManager.SetProperty(_mat, propertyData, 0);
        }
    }
}