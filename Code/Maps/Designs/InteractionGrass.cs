using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Maps.Designs
{
    [RequireComponent(typeof(MeshRenderer))]
    public class InteractionGrass : MonoBehaviour
    {
        private static readonly int PosPropertyHash = Shader.PropertyToID("_Pos");
        [SerializeField] private float addPosVale = 0.002f;
        
        private Material _grassMat;
        private HashSet<Transform> targets;
        private bool _emptyList = true;
        private float _pos = 0f;

        private void Awake()
        {
            targets = new HashSet<Transform>();
            _grassMat = GetComponent<MeshRenderer>().material;
        }

        private void FixedUpdate()
        {
            if (targets.Count <= 0)
            {
                if (!_emptyList)
                {
                    Vector3 currentGrassMatPos = _grassMat.GetVector(PosPropertyHash);
                    _grassMat.SetVector(PosPropertyHash, currentGrassMatPos + new Vector3(0, _pos, 0f));
                    _pos += addPosVale;
                }
                return;
            }
            
            _grassMat.SetVector(PosPropertyHash, targets.LastOrDefault()?.position ??  Vector3.zero);
        }

        private void OnTriggerEnter(Collider other)
        {
            targets.Add(other.transform);
            _emptyList = false;
            _pos = 0f;
        }

        private void OnTriggerExit(Collider other)
        {
            targets.Remove(other.transform);
        }
    }
}