using System;
using Code.Modules;
using EventSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Code.Cameras
{
    [Provide]
    public class CameraManager : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] private CinemachineCamOwner initCam;
        [SerializeField] private GameEventChannelSO combatChannel;
        [SerializeField] private GameEventChannelSO cameraChannel;

        private CinemachineCamOwner _currentFollowCam;

        private void Awake()
        {
            cameraChannel.AddListener<FollowCamChangeEvent>(HandleFollowCamChanged);
        }
        
        private void OnDestroy()
        {
            cameraChannel.RemoveListener<FollowCamChangeEvent>(HandleFollowCamChanged);
        }

        private void Start() => ChangeFollowCam(initCam);

        private void HandleFollowCamChanged(FollowCamChangeEvent evt) => ChangeFollowCam(evt.cam);

        public void ChangeFollowCam(CinemachineCamOwner cam)
        {
            if(cam is null) return;
            
            // 나중에 캠 전환 로직을 넣는다면 우선순위 변경도 모듈로 분리 고려

            CinemachineCamOwner prev = _currentFollowCam;
            _currentFollowCam = cam;
            TransitionCam(prev, _currentFollowCam);
        }

        public void TransitionCam(CinemachineCamOwner prev, CinemachineCamOwner next)
        {
            if(prev != null)
               prev.Cam.Priority = 0;
            
            if (next != null)
                next.Cam.Priority = 1;
        }
        
        public T GetFollowCamModule<T>() where T : IModule => _currentFollowCam.GetModule<T>();
    }
}