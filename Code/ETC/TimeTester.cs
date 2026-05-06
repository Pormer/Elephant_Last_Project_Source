using UnityEngine;

namespace Code.ETC
{
    public class TimeTester : MonoBehaviour
    {
        [SerializeField] private float targetTimeScale;

        [ContextMenu("Apply time scale")]
        private void ApplyTimeScale()
        {
            Time.timeScale = targetTimeScale;
        }
    }
}