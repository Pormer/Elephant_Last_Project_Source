using Unity.Cinemachine;
using UnityEngine;

namespace Code.Utility.CinemachineExtension
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // 메뉴에 노출되지 않게 설정
    public class LockAxisExtension : Unity.Cinemachine.CinemachineExtension
    {
        [Header("Lock Axis")]
        public bool useFixedXPosition;
        public bool useFixedYPosition;
        public bool useFixedZPosition;
        
        [Header("Fixed Position")]
        public float fixedXPosition = 0f;
        public float fixedYPosition = 0f;
        public float fixedZPosition = -10f;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                Vector3 pos = state.RawPosition;
                if(useFixedXPosition) pos.x = fixedXPosition;
                if(useFixedYPosition) pos.y = fixedYPosition;
                if(useFixedZPosition) pos.z = fixedZPosition;
                state.RawPosition = pos;
            }
        }
    }
}