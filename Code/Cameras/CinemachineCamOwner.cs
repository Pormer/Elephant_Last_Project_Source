using Code.Modules;
using Unity.Cinemachine;
using UnityEngine;

namespace Code.Cameras
{
    public class CinemachineCamOwner : ModuleOwner
    {
        [field: SerializeField] public CinemachineCamera Cam { get; private set; }
    }
}