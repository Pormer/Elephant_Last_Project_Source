using System;
using Cysharp.Threading.Tasks;
using EventSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Interactable
{
    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiEventChannel;
        [Space]
        [SerializeField] private Transform teleportEndPoint;
        [SerializeField] private float fadeDelay;
        public UnityEvent OnTeleport;
        
        
        public async void HandleTeleport(Collider other)
        {
            try
            {
                uiEventChannel.RaiseEvent(UIEvents.UIFadeInOutEvent.Initializer(()=>Teleport(other.transform)));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void Teleport(Transform target)
        {
            OnTeleport.Invoke();
            target.position = teleportEndPoint.position;
        }
    }
}
