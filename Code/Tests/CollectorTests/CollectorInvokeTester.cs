using Code.Detectors;
using UnityEngine;

namespace Code.Tests.CollectorTests
{
    public class CollectorInvokeTester : MonoBehaviour
    {
        [SerializeField] private Collector collector;
        
        [ContextMenu("Test Collecting")]
        private void TestCollect()
        {
            collector.TryCollect();
        }
    }
}