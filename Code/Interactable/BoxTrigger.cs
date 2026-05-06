using UnityEngine;
using UnityEngine.Events;

namespace Code.Interactable
{
    [RequireComponent(typeof(BoxCollider))]
    public class BoxTrigger : MonoBehaviour
    {
        public UnityEvent<Collider> OnTriggerEnterEvent;
        
        [SerializeField] private bool isOneShot;
        
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);

            if(isOneShot)
                gameObject.SetActive(false);
        }
    }
}