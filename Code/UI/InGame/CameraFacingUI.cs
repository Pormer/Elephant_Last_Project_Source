using UnityEngine;

namespace Code.UI.InGame
{
    public class CameraFacingUI : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private bool reverseForward = true;

        private Transform _cameraTrm;

        private void Awake()
        {
            CacheCamera();
        }

        private void OnEnable()
        {
            CacheCamera();
            FaceCamera();
        }

        private void LateUpdate()
        {
            if (_cameraTrm == null)
            {
                CacheCamera();
            }

            FaceCamera();
        }

        private void CacheCamera()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }

            _cameraTrm = targetCamera != null ? targetCamera.transform : null;
        }

        private void FaceCamera()
        {
            if (_cameraTrm == null)
            {
                return;
            }

            transform.rotation = reverseForward
                ? Quaternion.LookRotation(-_cameraTrm.forward, _cameraTrm.up)
                : Quaternion.LookRotation(_cameraTrm.forward, _cameraTrm.up);
        }
    }
}
