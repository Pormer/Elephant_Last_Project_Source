using Code.Collectibles.CollectableItems;
using EventSystem;
using UnityEngine;

namespace Code.Tests.CollectorTests
{
    public class TestManaCollectableItem : CollectableItem
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        
        public override void CollectComplete()
        {
            var evt = PlayerEvents.PlayerAddManaEvent.Initializer(1);
            playerChannel.RaiseEvent(evt);
            
            Debug.Log("Collect complete");
        }
    }
}