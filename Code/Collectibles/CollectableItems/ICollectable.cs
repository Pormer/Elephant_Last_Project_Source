using Code.Collectibles.Core;
using UnityEngine;

namespace Code.Collectibles.CollectableItems
{
    public interface ICollectable
    {
        void StartCollecting(Transform collectorTrm);
        void StopCollecting();
        void CollectComplete();
    }
}