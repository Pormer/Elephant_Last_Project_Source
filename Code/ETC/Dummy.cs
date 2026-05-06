using Code.Entities;
using UnityEngine;

namespace Code.ETC
{
    public class Dummy : Entity
    {
        protected override void HandleStopBound()
        {
            
        }

        public override bool CheckIsAirborne()
        {
            return false;
        }
    }
}