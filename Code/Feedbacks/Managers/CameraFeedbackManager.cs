using DG.Tweening;
using EventSystem;
using UnityEngine;

namespace Code.Feedbacks.Managers
{
    public class CameraFeedbackManager : FeedbackManager
    {
        [SerializeField] private float minZoomValue = 10.0f;
        private float _defaultZoomValue;
        private Camera _camera;
        private Tween _cameraZoomTween;
        
        private void Awake()
        {
            if (Camera.main != null)
            {
                _camera = Camera.main;
                _defaultZoomValue = _camera.fieldOfView;
            }
            feedbackChannel.AddListener<CameraZoomFeedbackEvent>(HandleCameraZoom);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<CameraZoomFeedbackEvent>(HandleCameraZoom);
        }

        private void HandleCameraZoom(CameraZoomFeedbackEvent evt)
        {
            float zoomValue = Mathf.Max(_defaultZoomValue + evt.addZoomValue, minZoomValue);

            if (_cameraZoomTween != null)
            {
                _cameraZoomTween.Kill();
            }
            
            _cameraZoomTween = DOTween.To(() => _camera.fieldOfView, 
                    x => _camera.fieldOfView = x,
                    zoomValue, evt.duration * 0.5f)
                .SetLoops(2, LoopType.Yoyo).SetEase(evt.easeType);
        }
    }
}