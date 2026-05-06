using UnityEngine;

namespace Code.Combat.KnockBacks
{
    [CreateAssetMenu(fileName = "KnockBackData", menuName = "SO/Combat/KnockBackData", order = 0)]
    public class KnockBackDataSO : ScriptableObject
    {
        public bool useAirBone;
        
        public float horizontalPower;
        public float verticalPower;
        
        public Vector3 CalculateKnockback(float xDirection)
        {
            // useAirBone이 true면 무조건 수직 파워가 있어야하고
            // 수직 파워가 있으면 무조건 useAirBone이 켜저 있어야함
            Debug.Assert(useAirBone == (Mathf.Abs(verticalPower) > 0), "Air bone Setting is invalid");

            float offsetX = useAirBone ? Random.Range(0f, 0.3f) : 0;
            float offsetY = useAirBone ? Random.Range(0f, 0.2f) : 0;
            float offsetZ = useAirBone ? Random.Range(-0.05f, 0.05f) : 0;
            
            Vector3 knockBackVelocity = new Vector3(Mathf.Sign(xDirection) * (horizontalPower + offsetX), verticalPower + offsetY, offsetZ);
            return knockBackVelocity;
        }
    }
}