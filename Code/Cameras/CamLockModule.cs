using Code.Modules;
using Code.Utility.CinemachineExtension;
using UnityEngine;

namespace Code.Cameras
{
    public class CamLockModule : MonoBehaviour, IModule
    {
        private CinemachineCamOwner _owner;
        private LockAxisExtension _lockExtension;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner as CinemachineCamOwner;
            Debug.Assert(_owner != null, $"Owner is not cinemachine cam owner: {_owner}");
            _lockExtension = _owner.Cam.GetComponent<LockAxisExtension>();
        }

        public void SetLockPosX(float x) => _lockExtension.fixedXPosition = x;
        public void SetLockPosY(float y) => _lockExtension.fixedYPosition = y;
        public void SetLockPosZ(float z) => _lockExtension.fixedZPosition = z;
        
        public void SetLockX(bool isLock) => _lockExtension.useFixedXPosition = isLock;
        public void SetLockY(bool isLock) => _lockExtension.useFixedYPosition = isLock;
        public void SetLockZ(bool isLock) => _lockExtension.useFixedZPosition = isLock;
    }
}