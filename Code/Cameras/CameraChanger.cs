using EventSystem;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Cameras
{
    public class CameraChanger : MonoBehaviour
    {
        [SerializeField] private CinemachineCamOwner targetCam;
        [SerializeField] private GameEventChannelSO cameraChannel;
        
        public void ChangeCamera() => cameraChannel.RaiseEvent(CameraEvents.FollowCamChangeEvent.Initializer(targetCam));
    }
}