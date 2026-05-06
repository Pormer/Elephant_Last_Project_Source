using System;
using Code.Entities;
using Code.Entities.Modules;
using Code.Extensions;
using Code.Stage;
using DG.Tweening;
using GondrLib.Dependencies;
using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Code.Cameras
{
    //나중에 구조 바꾸기
    public class CamTransitionTrigger : MonoBehaviour
    {
        [SerializeField] private CinemachineCamOwner originCam;
        [SerializeField] private CinemachineCamOwner targetCam;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private bool isLockInput;
        [SerializeField] private bool isRollback;
        [SerializeField] private float focusDuration;

        [Inject] private CameraManager _camManager;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.IsSameLayer(targetLayer))
            {
                _camManager.TransitionCam(originCam, targetCam);
                if(isLockInput) playerInput.SetEnabled(false);

                if (isRollback == false)
                {
                    Release();
                    return;
                }

                DOVirtual.DelayedCall(focusDuration, HandleFocusEnd);
            }
        }

        private void HandleFocusEnd()
        {
            _camManager.TransitionCam(targetCam, originCam);
            Release();
        }

        private void Release()
        {
            if(isLockInput) playerInput.SetEnabled(true);
            Destroy(gameObject);
        }
    }
}