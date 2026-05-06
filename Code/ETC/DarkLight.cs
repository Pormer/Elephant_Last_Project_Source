using UnityEngine;

namespace Code.ETC
{
    public class DarkLight : MonoBehaviour
    {
        [SerializeField] private GameObject light;
        [SerializeField] private GameObject dark;
        
        private void Awake()
        {
            light.SetActive(false);
            dark.SetActive(false);
        }
        
        public void Light()
        {
            light.SetActive(true);
        }
        
        public void Dark()
        {
            dark.SetActive(true);
        }
    }
}